using MedicalScheduling.Domain;
using MedicalScheduling.Application.Features.Patients.Commands.CreatePatient;
using MedicalScheduling.Domain.Repositories;
using MedicalScheduling.UnitTests.Common;
using Moq;

namespace MedicalScheduling.UnitTests.Application.Features.Patients.Commands.CreatePatient;

public sealed class CreatePatientHandlerTests
{
  private readonly Mock<IPatientRepository> _repository = new();
  private readonly Mock<IUnitOfWork> _unitOfWork = UnitOfWorkMockExtensions.CreateUnitOfWorkMock();
  private readonly CreatePatientHandler _handler;

  public CreatePatientHandlerTests()
  {
    _handler = new CreatePatientHandler(
        _repository.Object,
        _unitOfWork.Object,
        new NullCacheService(),
        MapperFactory.Create());
  }

  [Fact]
  public async Task Handle_WithValidCommand_ShouldCreatePatient()
  {
    var command = new CreatePatientCommand(
        TestData.PatientName,
        TestData.ValidEmail,
        TestData.ValidCpf,
        TestData.ValidPhone,
        TestData.ValidBirthDate);

    _repository.Setup(r => r.ExistsByCpfAsync(command.Cpf, It.IsAny<CancellationToken>()))
        .ReturnsAsync(false);

    var result = await _handler.Handle(command, CancellationToken.None);

    result.IsSuccess.Should().BeTrue();
    result.Value.Name.Should().Be(TestData.PatientName);
    result.Value.Email.Should().Be(TestData.ValidEmail);
    _repository.Verify(r => r.AddAsync(It.IsAny<Patient>(), It.IsAny<CancellationToken>()), Times.Once);
    _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
  }

  [Fact]
  public async Task Handle_WhenCpfAlreadyExists_ShouldReturnInvalidCpf()
  {
    var command = new CreatePatientCommand(
        TestData.PatientName,
        TestData.ValidEmail,
        TestData.ValidCpf,
        TestData.ValidPhone,
        TestData.ValidBirthDate);

    _repository.Setup(r => r.ExistsByCpfAsync(command.Cpf, It.IsAny<CancellationToken>()))
        .ReturnsAsync(true);

    var result = await _handler.Handle(command, CancellationToken.None);

    result.IsFailure.Should().BeTrue();
    result.Error.Code.Should().Be("Patient.InvalidCpf");
    _repository.Verify(r => r.AddAsync(It.IsAny<Patient>(), It.IsAny<CancellationToken>()), Times.Never);
  }

  [Fact]
  public async Task Handle_WithInvalidEmail_ShouldReturnDomainError()
  {
    var command = new CreatePatientCommand(
        TestData.PatientName,
        TestData.InvalidEmail,
        TestData.ValidCpf,
        TestData.ValidPhone,
        TestData.ValidBirthDate);

    _repository.Setup(r => r.ExistsByCpfAsync(command.Cpf, It.IsAny<CancellationToken>()))
        .ReturnsAsync(false);

    var result = await _handler.Handle(command, CancellationToken.None);

    result.IsFailure.Should().BeTrue();
    result.Error.Code.Should().Be("Patient.InvalidEmail");
  }
}
