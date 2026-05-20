using MedicalScheduling.Presentation.WebAPI.Middlewares;
using Microsoft.OpenApi;

namespace MedicalScheduling.Presentation.WebAPI;

public static class DependencyInjection
{
  public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
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

    services.AddCors(options =>
    {
      options.AddPolicy("Frontend", policy =>
      {
        var origins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
            ?? ["http://localhost:5173", "http://localhost:3000"];

        policy.WithOrigins(origins)
            .AllowAnyHeader()
            .AllowAnyMethod();
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

    app.UseCors("Frontend");
    app.UseHttpsRedirection();
    app.MapControllers();

    return app;
  }
}
