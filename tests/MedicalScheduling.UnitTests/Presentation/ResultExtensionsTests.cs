using MedicalScheduling.Domain.Primitives;
using MedicalScheduling.Presentation.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace MedicalScheduling.UnitTests.Presentation;

public sealed class ResultExtensionsTests
{
  [Fact]
  public void ToActionResult_WhenSuccess_ShouldReturnNoContent()
  {
    var actionResult = Result.Success().ToActionResult();

    actionResult.Should().BeOfType<NoContentResult>();
  }

  [Fact]
  public void ToActionResult_WhenNotFound_ShouldReturnNotFoundObjectResult()
  {
    var error = new Error("Patient.NotFound", "Paciente não encontrado");

    var actionResult = Result.Failure(error).ToActionResult();

    actionResult.Should().BeOfType<NotFoundObjectResult>();
    ((NotFoundObjectResult)actionResult).Value.Should().Be(error);
  }

  [Fact]
  public void ToActionResult_WhenValidationError_ShouldReturnUnprocessableEntity()
  {
    var error = new Error("Validation.Failed", "Campo inválido");

    var actionResult = Result.Failure(error).ToActionResult();

    actionResult.Should().BeOfType<UnprocessableEntityObjectResult>();
  }

  [Fact]
  public void ToActionResult_WhenOtherError_ShouldReturnBadRequest()
  {
    var error = new Error("Patient.InvalidCpf", "CPF inválido");

    var actionResult = Result.Failure(error).ToActionResult();

    actionResult.Should().BeOfType<BadRequestObjectResult>();
  }

  [Fact]
  public void ToActionResultOfT_WhenSuccess_ShouldReturnOkWithValue()
  {
    var value = new { Name = "Maria" };

    var actionResult = Result.Success(value).ToActionResult();

    actionResult.Should().BeOfType<OkObjectResult>();
    ((OkObjectResult)actionResult).Value.Should().Be(value);
  }

  [Fact]
  public void ToActionResultOfT_WhenNotFound_ShouldReturnNotFoundObjectResult()
  {
    var error = new Error("Doctor.NotFound", "Médico não encontrado");

    var actionResult = Result.Failure<object>(error).ToActionResult();

    actionResult.Should().BeOfType<NotFoundObjectResult>();
  }

  [Fact]
  public void ToCreatedResult_WhenSuccess_ShouldReturnCreatedAtRoute()
  {
    var value = new { Id = Guid.NewGuid() };
    var routeValues = new { id = value.Id };

    var actionResult = Result.Success(value).ToCreatedResult("GetById", routeValues);

    actionResult.Should().BeOfType<CreatedAtRouteResult>();
    var created = (CreatedAtRouteResult)actionResult;
    created.RouteName.Should().Be("GetById");
    created.Value.Should().Be(value);
  }

  [Fact]
  public void ToCreatedResult_WhenValidationError_ShouldReturnUnprocessableEntity()
  {
    var error = new Error("Validation.Failed", "Dados inválidos");

    var actionResult = Result.Failure<object>(error)
        .ToCreatedResult("GetById", new { id = Guid.NewGuid() });

    actionResult.Should().BeOfType<UnprocessableEntityObjectResult>();
  }
}
