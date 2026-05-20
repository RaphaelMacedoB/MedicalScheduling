using System.Text.Json;
using MedicalScheduling.Application.Abstractions.Caching;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace MedicalScheduling.Infrastructure.Caching;

public sealed class RedisCacheService : ICacheService
{
  private static readonly JsonSerializerOptions JsonOptions = new()
  {
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
  };

  private readonly IDatabase _database;
  private readonly IConnectionMultiplexer _connection;
  private readonly RedisCacheOptions _options;
  private readonly ILogger<RedisCacheService> _logger;

  public RedisCacheService(
      IConnectionMultiplexer connection,
      IOptions<RedisCacheOptions> options,
      ILogger<RedisCacheService> logger)
  {
    _connection = connection;
    _database = connection.GetDatabase();
    _options = options.Value;
    _logger = logger;
  }

  public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
      where T : class
  {
    if (!_connection.IsConnected)
      return null;

    try
    {
      var value = await _database.StringGetAsync(PrefixedKey(key));
      if (value.IsNullOrEmpty)
        return null;

      return JsonSerializer.Deserialize<T>((string)value!, JsonOptions);
    }
    catch (Exception ex)
    {
      _logger.LogWarning(ex, "Failed to read cache key {CacheKey}", key);
      return null;
    }
  }

  public async Task SetAsync<T>(
      string key,
      T value,
      TimeSpan? expiration = null,
      CancellationToken cancellationToken = default)
      where T : class
  {
    if (!_connection.IsConnected)
      return;

    try
    {
      var payload = JsonSerializer.Serialize(value, JsonOptions);
      await _database.StringSetAsync(
          PrefixedKey(key),
          payload,
          expiration ?? TimeSpan.FromMinutes(_options.DefaultExpirationMinutes));
    }
    catch (Exception ex)
    {
      _logger.LogWarning(ex, "Failed to write cache key {CacheKey}", key);
    }
  }

  public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
  {
    if (!_connection.IsConnected)
      return;

    try
    {
      await _database.KeyDeleteAsync(PrefixedKey(key));
    }
    catch (Exception ex)
    {
      _logger.LogWarning(ex, "Failed to remove cache key {CacheKey}", key);
    }
  }

  private string PrefixedKey(string key) => $"{_options.InstanceName}{key}";
}
