using AutoMapper;
using MediatR;
using MedicalScheduling.Application.Features.Doctors.DTOs;
using MedicalScheduling.Domain;
using MedicalScheduling.Domain.Primitives;
using MedicalScheduling.Domain.Repositories;
using MedicalScheduling.Infrastructure;

namespace MedicalScheduling.Application.Features.Doctors.Commands.CreateDoctor;

public sealed class CreateDoctorHandler : IRequestHandler<CreateDoctorCommand, Result<DoctorDto>>
{
  private readonly IDoctorRepository _doctorRepository;
  private readonly ISpecialityRepository _specialityRepository;
  private readonly IUnitOfWork _uow;
  private readonly IMapper _mapper;

  public CreateDoctorHandler(
      IDoctorRepository doctorRepository,
      ISpecialityRepository specialityRepository,
      IUnitOfWork uow,
      IMapper mapper)
  {
    _doctorRepository = doctorRepository;
    _specialityRepository = specialityRepository;
    _uow = uow;
    _mapper = mapper;
  }

  public async Task<Result<DoctorDto>> Handle(CreateDoctorCommand request, CancellationToken ct)
  {
    var specialtyExists = await _specialityRepository.GetByIdAsync(request.SpecialityId, ct);
    if (specialtyExists is null)
      return Result.Failure<DoctorDto>(DomainErrors.Speciality.NotFound);

    var crmExists = await _doctorRepository.ExistsByCrmAsync(request.Crm, ct);
    if (crmExists)
      return Result.Failure<DoctorDto>(DomainErrors.Doctor.InvalidCrm);

    var result = Doctor.Create(
        request.Name,
        request.Crm,
        request.Email,
        request.Phone,
        request.SpecialityId);

    if (result.IsFailure)
      return Result.Failure<DoctorDto>(result.Error);

    await _doctorRepository.AddAsync(result.Value, ct);
    await _uow.SaveChangesAsync(ct);

    return Result.Success(_mapper.Map<DoctorDto>(result.Value));
  }
}