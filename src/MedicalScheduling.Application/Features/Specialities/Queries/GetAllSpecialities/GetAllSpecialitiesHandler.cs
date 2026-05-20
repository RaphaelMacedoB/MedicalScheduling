namespace MedicalScheduling.Application.Features.Specialities.Queries.GetAllSpecialities;

using AutoMapper;
using MediatR;
using MedicalScheduling.Application.Features.Specialities.DTOs;
using MedicalScheduling.Domain.Primitives;
using MedicalScheduling.Domain.Repositories;

public sealed class GetAllSpecialitiesHandler
    : IRequestHandler<GetAllSpecialitiesQuery, Result<IEnumerable<SpecialityDto>>>
{
  private readonly ISpecialityRepository _repository;
  private readonly IMapper _mapper;

  public GetAllSpecialitiesHandler(ISpecialityRepository repository, IMapper mapper)
  {
    _repository = repository;
    _mapper = mapper;
  }

  public async Task<Result<IEnumerable<SpecialityDto>>> Handle(GetAllSpecialitiesQuery request, CancellationToken ct)
  {
    var specialities = await _repository.GetAllAsync(ct);
    return Result.Success(_mapper.Map<IEnumerable<SpecialityDto>>(specialities));
  }
}
