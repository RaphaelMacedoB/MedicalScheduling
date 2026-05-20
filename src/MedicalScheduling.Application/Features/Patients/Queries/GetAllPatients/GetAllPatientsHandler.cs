using AutoMapper;
using MediatR;
using MedicalScheduling.Application.Features.Patients.DTOs;
using MedicalScheduling.Domain.Primitives;
using MedicalScheduling.Domain.Repositories;

namespace MedicalScheduling.Application.Features.Patients.Queries.GetAllPatients;

public sealed class GetAllPatientsHandler : IRequestHandler<GetAllPatientsQuery, Result<IEnumerable<PatientDto>>>
{
  private readonly IPatientRepository _repository;
  private readonly IMapper _mapper;

  public GetAllPatientsHandler(IPatientRepository repository, IMapper mapper)
  {
    _repository = repository;
    _mapper = mapper;
  }

  public async Task<Result<IEnumerable<PatientDto>>> Handle(GetAllPatientsQuery request, CancellationToken ct)
  {
    var patients = await _repository.GetAllAsync(ct);
    return Result.Success(_mapper.Map<IEnumerable<PatientDto>>(patients));
  }
}
