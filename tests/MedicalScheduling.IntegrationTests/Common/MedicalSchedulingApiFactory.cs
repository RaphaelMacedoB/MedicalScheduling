using MedicalScheduling.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;

namespace MedicalScheduling.IntegrationTests.Common;

public sealed class MedicalSchedulingApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
  private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder("postgres:16-alpine")
      .WithDatabase("medical_scheduling_test")
      .WithUsername("postgres")
      .WithPassword("postgres")
      .Build();

  private readonly RedisContainer _redis = new RedisBuilder("redis:7-alpine").Build();

  private bool _databaseInitialized;

  public async Task InitializeAsync()
  {
    await Task.WhenAll(_postgres.StartAsync(), _redis.StartAsync());
  }

  public new async Task DisposeAsync()
  {
    await _postgres.DisposeAsync();
    await _redis.DisposeAsync();
    await base.DisposeAsync();
  }

  protected override void ConfigureWebHost(IWebHostBuilder builder)
  {
    builder.UseEnvironment("Development");

    builder.ConfigureAppConfiguration((_, config) =>
    {
      config.AddInMemoryCollection(new Dictionary<string, string?>
      {
        ["ConnectionStrings:DefaultConnection"] = _postgres.GetConnectionString(),
        ["ConnectionStrings:Redis"] = _redis.GetConnectionString()
      });
    });
  }

  protected override IHost CreateHost(IHostBuilder builder)
  {
    var host = base.CreateHost(builder);

    if (!_databaseInitialized)
    {
      using var scope = host.Services.CreateScope();
      var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
      db.Database.Migrate();
      _databaseInitialized = true;
    }

    return host;
  }

  public HttpClient CreateAuthenticatedClient() => CreateClient(new WebApplicationFactoryClientOptions
  {
    AllowAutoRedirect = false
  });
}
