using MedicalScheduling.Domain.Repositories;
using MedicalScheduling.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MedicalScheduling.Infrastructure;

public static class DependencyInjection
{
  public static IServiceCollection AddInfrastructure(
      this IServiceCollection services,
      IConfiguration configuration)
  {
    services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

    services.AddScoped<IUnitOfWork, UnitOfWork>();
    services.AddScoped<IPatientRepository, PatientRepository>();
    services.AddScoped<IDoctorRepository, DoctorRepository>();
    services.AddScoped<IAppointmentRepository, AppointmentRepository>();
    services.AddScoped<ISpecialityRepository, SpecialityRepository>();

    return services;
  }
}