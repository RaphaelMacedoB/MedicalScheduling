using MedicalScheduling.Domain;
using MedicalScheduling.Application.Features.Specialities.Commands.CreateSpeciality;
using MedicalScheduling.Domain.Repositories;
using MedicalScheduling.UnitTests.Common;
using Moq;

namespace MedicalScheduling.UnitTests.Application.Features.Specialities.Commands.CreateSpeciality;

public sealed class CreateSpecialityHandlerTests
{
  private readonly Mock<ISpecialityRepository> _repository = new();
  private readonly Mock<IUnitOfWork> _unitOfWork = UnitOfWorkMockExtensions.CreateUnitOfWorkMock();
  private readonly CreateSpecialityHandler _handler;

  public CreateSpecialityHandlerTests()
  {
    _handler = new CreateSpecialityHandler(
        _repository.Object,
        _unitOfWork.Object,
        MapperFactory.Create());
  }

  [Fact]
  public async Task Handle_WithValidCommand_ShouldCreateSpeciality()
  {
    var command = new CreateSpecialityCommand(TestData.SpecialityName, "Descrição");

    _repository.Setup(r => r.ExistsByNameAsync(command.Name, It.IsAny<CancellationToken>()))
        .ReturnsAsync(false);

    var result = await _handler.Handle(command, CancellationToken.None);

    result.IsSuccess.Should().BeTrue();
    result.Value.Name.Should().Be(TestData.SpecialityName);
    _repository.Verify(r => r.AddAsync(It.IsAny<Speciality>(), It.IsAny<CancellationToken>()), Times.Once);
    _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
  }

  [Fact]
  public async Task Handle_WhenNameAlreadyExists_ShouldReturnDuplicateName()
  {
    var command = new CreateSpecialityCommand(TestData.SpecialityName, null);

    _repository.Setup(r => r.ExistsByNameAsync(command.Name, It.IsAny<CancellationToken>()))
        .ReturnsAsync(true);

    var result = await _handler.Handle(command, CancellationToken.None);

    result.IsFailure.Should().BeTrue();
    result.Error.Code.Should().Be("Speciality.DuplicateName");
  }
}
