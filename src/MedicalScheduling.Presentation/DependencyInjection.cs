using MedicalScheduling.Presentation.Middlewares;
using Microsoft.OpenApi;

namespace MedicalScheduling.Presentation;

public static class DependencyInjection
{
  public static IServiceCollection AddPresentation(this IServiceCollection services)
  {
    services.AddControllers();

    services.AddEndpointsApiExplorer();

    services.AddSwaggerGen(options =>
    {
      options.SwaggerDoc("v1", new OpenApiInfo
      {
        Title = "Medical Scheduling API",
        Version = "v1",
        Description = "API para agendamento de consultas médicas"
      });
    });

    return services;
  }

  public static WebApplication UsePresentation(this WebApplication app)
  {
    app.UseMiddleware<ExceptionMiddleware>();

    if (app.Environment.IsDevelopment())
    {
      app.UseSwagger();
      app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.MapControllers();

    return app;
  }
}