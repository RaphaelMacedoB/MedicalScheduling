using MedicalScheduling.Domain;
using MedicalScheduling.Application.Features.Specialities.Commands.UpdateSpeciality;
using MedicalScheduling.Domain.Repositories;
using MedicalScheduling.UnitTests.Common;
using Moq;

namespace MedicalScheduling.UnitTests.Application.Features.Specialities.Commands.UpdateSpeciality;

public sealed class UpdateSpecialityHandlerTests
{
  private readonly Mock<ISpecialityRepository> _repository = new();
  private readonly Mock<IUnitOfWork> _unitOfWork = UnitOfWorkMockExtensions.CreateUnitOfWorkMock();
  private readonly UpdateSpecialityHandler _handler;

  public UpdateSpecialityHandlerTests()
  {
    _handler = new UpdateSpecialityHandler(
        _repository.Object,
        _unitOfWork.Object,
        new NullCacheService(),
        MapperFactory.Create());
  }

  [Fact]
  public async Task Handle_WhenSpecialityExists_ShouldUpdate()
  {
    var speciality = EntityTestHelper.CreateSpeciality();
    var command = new UpdateSpecialityCommand(speciality.Id, "Neurologia", "Nova descrição");

    _repository.Setup(r => r.GetByIdAsync(speciality.Id, It.IsAny<CancellationToken>()))
        .ReturnsAsync(speciality);
    _repository.Setup(r => r.ExistsByNameAsync(command.Name, It.IsAny<CancellationToken>()))
        .ReturnsAsync(false);

    var result = await _handler.Handle(command, CancellationToken.None);

    result.IsSuccess.Should().BeTrue();
    result.Value.Name.Should().Be("Neurologia");
    speciality.Name.Should().Be("Neurologia");
    _repository.Verify(r => r.Update(speciality), Times.Once);
    _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
  }

  [Fact]
  public async Task Handle_WhenSpecialityNotFound_ShouldReturnNotFound()
  {
    var command = new UpdateSpecialityCommand(Guid.NewGuid(), "Neurologia", null);

    _repository.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
        .ReturnsAsync((Speciality?)null);

    var result = await _handler.Handle(command, CancellationToken.None);

    result.IsFailure.Should().BeTrue();
    result.Error.Code.Should().Be("Speciality.NotFound");
  }

  [Fact]
  public async Task Handle_WhenNameExistsForAnotherSpeciality_ShouldReturnDuplicateName()
  {
    var speciality = EntityTestHelper.CreateSpeciality();
    var command = new UpdateSpecialityCommand(speciality.Id, "Dermatologia", null);

    _repository.Setup(r => r.GetByIdAsync(speciality.Id, It.IsAny<CancellationToken>()))
        .ReturnsAsync(speciality);
    _repository.Setup(r => r.ExistsByNameAsync(command.Name, It.IsAny<CancellationToken>()))
        .ReturnsAsync(true);

    var result = await _handler.Handle(command, CancellationToken.None);

    result.IsFailure.Should().BeTrue();
    result.Error.Code.Should().Be("Speciality.DuplicateName");
  }
}
