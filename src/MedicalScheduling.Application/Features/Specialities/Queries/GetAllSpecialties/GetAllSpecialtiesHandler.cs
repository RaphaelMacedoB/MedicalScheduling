namespace MedicalScheduling.Application.Features.Specialities.Queries.GetAllSpecialties;

using AutoMapper;
using MediatR;
using MedicalScheduling.Application.Features.Specialities.DTOs;
using MedicalScheduling.Domain.Primitives;
using MedicalScheduling.Domain.Repositories;

public sealed class GetAllSpecialtiesHandler
    : IRequestHandler<GetAllSpecialitiesQuery, Result<IEnumerable<SpecialityDto>>>
{
  private readonly ISpecialityRepository _repository;
  private readonly IMapper _mapper;

  public GetAllSpecialtiesHandler(ISpecialityRepository repository, IMapper mapper)
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