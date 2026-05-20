using MediatR;
using MedicalScheduling.Application.Features.Patients.Commands.CreatePatient;
using MedicalScheduling.Application.Features.Patients.Commands.DeactivatePatient;
using MedicalScheduling.Application.Features.Patients.DTOs;
using MedicalScheduling.Application.Features.Patients.Queries.GetAllPatients;
using MedicalScheduling.Application.Features.Patients.Queries.GetPatientById;
using MedicalScheduling.Presentation.Constants;
using MedicalScheduling.Presentation.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace MedicalScheduling.Presentation.Controllers;

[ApiController]
[Route(ApiRoutes.Patients.Base)]
public sealed class PatientsController : ControllerBase
{
  private readonly IMediator _mediator;

  public PatientsController(IMediator mediator) => _mediator = mediator;

  [HttpGet]
  [ProducesResponseType(typeof(IEnumerable<PatientDto>), StatusCodes.Status200OK)]
  public async Task<IActionResult> GetAll(CancellationToken ct)
  {
    var result = await _mediator.Send(new GetAllPatientsQuery(), ct);
    return result.ToActionResult();
  }

  [HttpGet(ApiRoutes.Patients.ById, Name = ApiRoutes.Patients.GetById)]
  [ProducesResponseType(typeof(PatientDto), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
  {
    var result = await _mediator.Send(new GetPatientByIdQuery(id), ct);
    return result.ToActionResult();
  }

  [HttpPost]
  [ProducesResponseType(typeof(PatientDto), StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
  public async Task<IActionResult> Create(CreatePatientCommand command, CancellationToken ct)
  {
    var result = await _mediator.Send(command, ct);
    return result.ToCreatedResult(ApiRoutes.Patients.GetById, new { id = result.IsSuccess ? result.Value.Id : Guid.Empty });
  }

  [HttpDelete(ApiRoutes.Patients.ById)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> Deactivate(Guid id, CancellationToken ct)
  {
    var result = await _mediator.Send(new DeactivatePatientCommand(id), ct);
    return result.ToActionResult();
  }
}
