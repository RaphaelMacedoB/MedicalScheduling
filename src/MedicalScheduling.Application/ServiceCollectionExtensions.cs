namespace MedicalScheduling.Application;

using FluentValidation;
using MediatR;
using MedicalScheduling.Application.Behaviors;
using MedicalScheduling.Application.Common.Mappings;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddApplication(this IServiceCollection services)
  {
    var assembly = typeof(ServiceCollectionExtensions).Assembly;

    services.AddMediatR(cfg =>
        cfg.RegisterServicesFromAssembly(assembly));

    services.AddTransient(
        typeof(IPipelineBehavior<,>),
        typeof(ValidationBehavior<,>));

    services.AddValidatorsFromAssembly(assembly);

    services.AddAutoMapper(typeof(MappingProfile));

    return services;
  }
}