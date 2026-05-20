using MedicalScheduling.Domain.Primitives;

namespace MedicalScheduling.Presentation.Middlewares;

public sealed class ExceptionMiddleware
{
  private readonly RequestDelegate _next;
  private readonly ILogger<ExceptionMiddleware> _logger;

  public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
  {
    _next = next;
    _logger = logger;
  }

  public async Task InvokeAsync(HttpContext context)
  {
    try
    {
      await _next(context);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
      await HandleExceptionAsync(context, ex);
    }
  }

  private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
  {
    context.Response.ContentType = "application/json";

    var (statusCode, message) = exception switch
    {
      ArgumentException => (StatusCodes.Status400BadRequest, exception.Message),
      InvalidOperationException => (StatusCodes.Status409Conflict, exception.Message),
      _ => (StatusCodes.Status500InternalServerError, "Ocorreu um erro interno")
    };

    context.Response.StatusCode = statusCode;

    var error = new Error(nameof(ExceptionMiddleware), message);

    await context.Response.WriteAsJsonAsync(error);
  }
}