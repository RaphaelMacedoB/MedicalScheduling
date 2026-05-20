using MedicalScheduling.Domain;
using AutoMapper;
using MedicalScheduling.Application.Features.Doctors.Commands.CreateDoctor;
using MedicalScheduling.Application.Features.Doctors.DTOs;
using MedicalScheduling.Domain.Repositories;
using MedicalScheduling.UnitTests.Common;
using Moq;

namespace MedicalScheduling.UnitTests.Application.Features.Doctors.Commands.CreateDoctor;

public sealed class CreateDoctorHandlerTests
{
    private readonly Mock<IDoctorRepository> _doctorRepository = new();
    private readonly Mock<ISpecialityRepository> _specialityRepository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = UnitOfWorkMockExtensions.CreateUnitOfWorkMock();
    private readonly Mock<IMapper> _mapper = new();

    private CreateDoctorHandler CreateHandler() => new(
        _doctorRepository.Object,
        _specialityRepository.Object,
        _unitOfWork.Object,
      _mapper.Object);

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreateDoctor()
    {
        var speciality = EntityTestHelper.CreateSpeciality();
        var command = new CreateDoctorCommand(
            TestData.DoctorName,
            TestData.Crm,
            TestData.ValidEmail,
            TestData.ValidPhone,
            speciality.Id);

        _specialityRepository.Setup(r => r.GetByIdAsync(speciality.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(speciality);
        _doctorRepository.Setup(r => r.ExistsByCrmAsync(command.Crm, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _mapper.Setup(m => m.Map<DoctorDto>(It.IsAny<Doctor>()))
            .Returns((Doctor d) => new DoctorDto(
                d.Id, d.Name, d.Crm, d.Email.Value, d.Phone.Value,
                d.SpecialityId, speciality.Name, true, d.CreatedAt));

        var result = await CreateHandler().Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be(TestData.DoctorName);
        _doctorRepository.Verify(r => r.AddAsync(It.IsAny<Doctor>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenSpecialityNotFound_ShouldReturnNotFound()
    {
        var command = new CreateDoctorCommand(
            TestData.DoctorName,
            TestData.Crm,
            TestData.ValidEmail,
            TestData.ValidPhone,
            Guid.NewGuid());

        _specialityRepository.Setup(r => r.GetByIdAsync(command.SpecialityId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Speciality?)null);

        var result = await CreateHandler().Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Speciality.NotFound");
    }

    [Fact]
    public async Task Handle_WhenCrmAlreadyExists_ShouldReturnInvalidCrm()
    {
        var speciality = EntityTestHelper.CreateSpeciality();
        var command = new CreateDoctorCommand(
            TestData.DoctorName,
            TestData.Crm,
            TestData.ValidEmail,
            TestData.ValidPhone,
            speciality.Id);

        _specialityRepository.Setup(r => r.GetByIdAsync(speciality.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(speciality);
        _doctorRepository.Setup(r => r.ExistsByCrmAsync(command.Crm, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await CreateHandler().Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Doctor.InvalidCrm");
    }
}
