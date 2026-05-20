using FluentValidation;
using MediatR;
using MedicalScheduling.Application.Behaviors;
using MedicalScheduling.Application.Features.Patients.Commands.CreatePatient;
using MedicalScheduling.Application.Features.Patients.Commands.DeactivatePatient;
using MedicalScheduling.Application.Features.Patients.DTOs;
using MedicalScheduling.Domain.Primitives;
using MedicalScheduling.UnitTests.Common;

namespace MedicalScheduling.UnitTests.Application.Behaviors;

public sealed class ValidationBehaviorTests
{
  [Fact]
  public async Task Handle_WhenNoValidators_ShouldInvokeNext()
  {
    var behavior = new ValidationBehavior<DeactivatePatientCommand, Result>(
        Enumerable.Empty<IValidator<DeactivatePatientCommand>>());

    var nextCalled = false;
    RequestHandlerDelegate<Result> next = _ =>
    {
      nextCalled = true;
      return Task.FromResult(Result.Success());
    };

    var result = await behavior.Handle(
        new DeactivatePatientCommand(Guid.NewGuid()),
        next,
        CancellationToken.None);

    nextCalled.Should().BeTrue();
    result.IsSuccess.Should().BeTrue();
  }

  [Fact]
  public async Task Handle_WhenValidationFailsForResult_ShouldReturnValidationFailed()
  {
    var behavior = new ValidationBehavior<DeactivatePatientCommand, Result>(
        [new DeactivatePatientValidator()]);

    RequestHandlerDelegate<Result> next = _ => Task.FromResult(Result.Success());

    var result = await behavior.Handle(
        new DeactivatePatientCommand(Guid.Empty),
        next,
        CancellationToken.None);

    result.IsFailure.Should().BeTrue();
    result.Error.Code.Should().Be("Validation.Failed");
    result.Error.Message.Should().Contain("Id é obrigatório");
  }

  [Fact]
  public async Task Handle_WhenValidationFailsForResultOfT_ShouldReturnValidationFailed()
  {
    var behavior = new ValidationBehavior<CreatePatientCommand, Result<PatientDto>>(
        [new CreatePatientValidator()]);

    RequestHandlerDelegate<Result<PatientDto>> next = _ =>
        Task.FromResult(Result.Success(new PatientDto(
            Guid.NewGuid(), "", "", "", "", DateOnly.MinValue, true, DateTime.UtcNow)));

    var command = new CreatePatientCommand("", TestData.InvalidEmail, "", "", DateOnly.FromDateTime(DateTime.Today));

    var result = await behavior.Handle(command, next, CancellationToken.None);

    result.IsFailure.Should().BeTrue();
    result.Error.Code.Should().Be("Validation.Failed");
    result.Error.Message.Should().NotBeNullOrWhiteSpace();
  }
}
