namespace MedicalScheduling.Application.Features.Specialities.Commands.UpdateSpeciality;

using AutoMapper;
using MediatR;
using MedicalScheduling.Application.Features.Specialities.DTOs;
using MedicalScheduling.Domain.Primitives;
using MedicalScheduling.Domain.Repositories;
using MedicalScheduling.Infrastructure;

public sealed class UpdateSpecialtyHandler : IRequestHandler<UpdateSpecialtyCommand, Result<SpecialityDto>>
{
  private readonly ISpecialityRepository _repository;
  private readonly IUnitOfWork _uow;
  private readonly IMapper _mapper;

  public UpdateSpecialtyHandler(ISpecialityRepository repository, IUnitOfWork uow, IMapper mapper)
  {
    _repository = repository;
    _uow = uow;
    _mapper = mapper;
  }

  public async Task<Result<SpecialityDto>> Handle(UpdateSpecialtyCommand request, CancellationToken ct)
  {
    var specialty = await _repository.GetByIdAsync(request.Id, ct);
    if (specialty is null)
      return Result.Failure<SpecialityDto>(DomainErrors.Speciality.NotFound);

    var nameExists = await _repository.ExistsByNameAsync(request.Name, ct);
    if (nameExists && specialty.Name != request.Name)
      return Result.Failure<SpecialityDto>(DomainErrors.Speciality.DuplicateName);

    specialty.Update(request.Name, request.Description);
    _repository.Update(specialty);
    await _uow.SaveChangesAsync(ct);

    return Result.Success(_mapper.Map<SpecialityDto>(specialty));
  }
}