using MedicalScheduling.Application.Abstractions.Caching;
using MedicalScheduling.Domain.Repositories;
using MedicalScheduling.Infrastructure.Caching;
using MedicalScheduling.Infrastructure.Persistence;
using MedicalScheduling.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace MedicalScheduling.Infrastructure;

public static class DependencyInjection
{
  public static IServiceCollection AddInfrastructure(
      this IServiceCollection services,
      IConfiguration configuration)
  {
    services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

    services.Configure<RedisCacheOptions>(configuration.GetSection(RedisCacheOptions.SectionName));

    var redisConnection = configuration.GetConnectionString("Redis")
        ?? throw new InvalidOperationException("Connection string 'Redis' is not configured.");

    var redisOptions = ConfigurationOptions.Parse(redisConnection);
    redisOptions.AbortOnConnectFail = false;

    services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(redisOptions));
    services.AddSingleton<ICacheService, RedisCacheService>();

    services.AddScoped<IUnitOfWork, UnitOfWork>();
    services.AddScoped<IPatientRepository, PatientRepository>();
    services.AddScoped<IDoctorRepository, DoctorRepository>();
    services.AddScoped<IAppointmentRepository, AppointmentRepository>();
    services.AddScoped<ISpecialityRepository, SpecialityRepository>();

    return services;
  }
}