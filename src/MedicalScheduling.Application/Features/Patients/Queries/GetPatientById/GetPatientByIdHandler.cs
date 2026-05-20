using AutoMapper;
using MediatR;
using MedicalScheduling.Application.Features.Patients.DTOs;
using MedicalScheduling.Domain.Primitives;
using MedicalScheduling.Domain.Repositories;

namespace MedicalScheduling.Application.Features.Patients.Queries.GetPatientById;

public sealed class GetPatientByIdHandler : IRequestHandler<GetPatientByIdQuery, Result<PatientDto>>
{
  private readonly IPatientRepository _repository;
  private readonly IMapper _mapper;

  public GetPatientByIdHandler(IPatientRepository repository, IMapper mapper)
  {
    _repository = repository;
    _mapper = mapper;
  }

  public async Task<Result<PatientDto>> Handle(GetPatientByIdQuery request, CancellationToken ct)
  {
    var patient = await _repository.GetByIdAsync(request.Id, ct);
    if (patient is null)
      return Result.Failure<PatientDto>(DomainErrors.Patient.NotFound);

    return Result.Success(_mapper.Map<PatientDto>(patient));
  }
}