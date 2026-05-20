using MedicalScheduling.Domain.Primitives;
using Microsoft.AspNetCore.Mvc;

namespace MedicalScheduling.Presentation.WebAPI.Extensions;

public static class ResultExtensions
{
  public static IActionResult ToActionResult(this Result result)
  {
    if (result.IsSuccess)
      return new NoContentResult();

    return result.Error.Code switch
    {
      var code when code.EndsWith("NotFound") => new NotFoundObjectResult(result.Error),
      var code when code.StartsWith("Validation") => new UnprocessableEntityObjectResult(result.Error),
      _ => new BadRequestObjectResult(result.Error)
    };
  }

  public static IActionResult ToActionResult<T>(this Result<T> result)
  {
    if (result.IsSuccess)
      return new OkObjectResult(result.Value);

    return result.Error.Code switch
    {
      var code when code.EndsWith("NotFound") => new NotFoundObjectResult(result.Error),
      var code when code.StartsWith("Validation") => new UnprocessableEntityObjectResult(result.Error),
      _ => new BadRequestObjectResult(result.Error)
    };
  }

  public static IActionResult ToCreatedResult<T>(this Result<T> result, string routeName, object routeValues)
  {
    if (result.IsSuccess)
      return new CreatedAtRouteResult(routeName, routeValues, result.Value);

    return result.Error.Code switch
    {
      var code when code.EndsWith("NotFound") => new NotFoundObjectResult(result.Error),
      var code when code.StartsWith("Validation") => new UnprocessableEntityObjectResult(result.Error),
      _ => new BadRequestObjectResult(result.Error)
    };
  }
}