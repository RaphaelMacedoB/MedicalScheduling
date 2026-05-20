namespace MedicalScheduling.Application.Features.Specialities.Commands.UpdateSpeciality;

using AutoMapper;
using MediatR;
using MedicalScheduling.Application.Features.Specialities.DTOs;
using MedicalScheduling.Domain.Primitives;
using MedicalScheduling.Domain.Repositories;

public sealed class UpdateSpecialityHandler : IRequestHandler<UpdateSpecialityCommand, Result<SpecialityDto>>
{
  private readonly ISpecialityRepository _repository;
  private readonly IUnitOfWork _uow;
  private readonly IMapper _mapper;

  public UpdateSpecialityHandler(
      ISpecialityRepository repository,
      IUnitOfWork uow,
      IMapper mapper)
  {
    _repository = repository;
    _uow = uow;
    _mapper = mapper;
  }

  public async Task<Result<SpecialityDto>> Handle(UpdateSpecialityCommand request, CancellationToken ct)
  {
    var speciality = await _repository.GetByIdAsync(request.Id, ct);
    if (speciality is null)
      return Result.Failure<SpecialityDto>(DomainErrors.Speciality.NotFound);

    var nameExists = await _repository.ExistsByNameAsync(request.Name, ct);
    if (nameExists && speciality.Name != request.Name)
      return Result.Failure<SpecialityDto>(DomainErrors.Speciality.DuplicateName);

    speciality.Update(request.Name, request.Description);
    _repository.Update(speciality);
    await _uow.SaveChangesAsync(ct);

    return Result.Success(_mapper.Map<SpecialityDto>(speciality));
  }
}
