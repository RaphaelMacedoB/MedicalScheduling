using MediatR;
using MedicalScheduling.Application.Features.Doctors.Commands.CreateDoctor;
using MedicalScheduling.Application.Features.Doctors.Commands.DeactivateDoctor;
using MedicalScheduling.Application.Features.Doctors.DTOs;
using MedicalScheduling.Application.Features.Doctors.Queries.GetDoctorById;
using MedicalScheduling.Application.Features.Doctors.Queries.GetDoctorsBySpeciality;
using MedicalScheduling.Presentation.Constants;
using MedicalScheduling.Presentation.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace MedicalScheduling.Presentation.Controllers;

[ApiController]
[Route(ApiRoutes.Doctors.Base)]
public sealed class DoctorsController : ControllerBase
{
  private readonly IMediator _mediator;

  public DoctorsController(IMediator mediator) => _mediator = mediator;

  [HttpGet(ApiRoutes.Doctors.ById, Name = ApiRoutes.Doctors.GetById)]
  [ProducesResponseType(typeof(DoctorDto), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
  {
    var result = await _mediator.Send(new GetDoctorByIdQuery(id), ct);
    return result.ToActionResult();
  }

  [HttpGet(ApiRoutes.Doctors.BySpeciality)]
  [ProducesResponseType(typeof(IEnumerable<DoctorDto>), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> GetBySpeciality(Guid specialityId, CancellationToken ct)
  {
    var result = await _mediator.Send(new GetDoctorsBySpecialityQuery(specialityId), ct);
    return result.ToActionResult();
  }

  [HttpPost]
  [ProducesResponseType(typeof(DoctorDto), StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
  public async Task<IActionResult> Create(CreateDoctorCommand command, CancellationToken ct)
  {
    var result = await _mediator.Send(command, ct);
    return result.ToCreatedResult(ApiRoutes.Doctors.GetById, new { id = result.IsSuccess ? result.Value.Id : Guid.Empty });
  }

  [HttpDelete(ApiRoutes.Doctors.ById)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> Deactivate(Guid id, CancellationToken ct)
  {
    var result = await _mediator.Send(new DeactivateDoctorCommand(id), ct);
    return result.ToActionResult();
  }
}
