namespace MedicalScheduling.Application.Features.Specialities.Commands.CreateSpeciality;

using AutoMapper;
using MediatR;
using MedicalScheduling.Application.Abstractions.Caching;
using MedicalScheduling.Application.Features.Specialities.DTOs;
using MedicalScheduling.Domain;
using MedicalScheduling.Domain.Primitives;
using MedicalScheduling.Domain.Repositories;
public sealed class CreateSpecialityHandler : IRequestHandler<CreateSpecialityCommand, Result<SpecialityDto>>
{
  private readonly ISpecialityRepository _repository;
  private readonly IUnitOfWork _uow;
  private readonly ICacheService _cache;
  private readonly IMapper _mapper;

  public CreateSpecialityHandler(
      ISpecialityRepository repository,
      IUnitOfWork uow,
      ICacheService cache,
      IMapper mapper)
  {
    _repository = repository;
    _uow = uow;
    _cache = cache;
    _mapper = mapper;
  }

  public async Task<Result<SpecialityDto>> Handle(CreateSpecialityCommand request, CancellationToken ct)
  {
    var exists = await _repository.ExistsByNameAsync(request.Name, ct);
    if (exists)
      return Result.Failure<SpecialityDto>(DomainErrors.Speciality.DuplicateName);

    var result = Speciality.Create(request.Name, request.Description);
    if (result.IsFailure)
      return Result.Failure<SpecialityDto>(result.Error);

    await _repository.AddAsync(result.Value, ct);
    await _uow.SaveChangesAsync(ct);
    await _cache.RemoveAsync(CacheKeys.Specialities.All, ct);

    return Result.Success(_mapper.Map<SpecialityDto>(result.Value));
  }
}
