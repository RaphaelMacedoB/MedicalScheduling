using MediatR;
using MedicalScheduling.Application.Features.Appointments.CancelAppointment;
using MedicalScheduling.Application.Features.Appointments.CompleteAppointment;
using MedicalScheduling.Application.Features.Appointments.ConfirmAppointment;
using MedicalScheduling.Application.Features.Appointments.GetAppointmentById;
using MedicalScheduling.Application.Features.Appointments.GetAppointmentsByDoctor;
using MedicalScheduling.Application.Features.Appointments.GetAppointmentsByPatient;
using MedicalScheduling.Application.Features.Appointments.ScheduleAppointment;
using MedicalScheduling.Presentation.Constants;
using MedicalScheduling.Presentation.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace MedicalScheduling.Presentation.Controllers;

[ApiController]
[Route(ApiRoutes.Appointments.Base)]
public sealed class AppointmentsController : ControllerBase
{
  private readonly IMediator _mediator;

  public AppointmentsController(IMediator mediator) => _mediator = mediator;

  [HttpGet(ApiRoutes.Appointments.ById, Name = ApiRoutes.Appointments.GetById)]
  [ProducesResponseType(typeof(AppointmentDto), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
  {
    var result = await _mediator.Send(new GetAppointmentByIdQuery(id), ct);
    return result.ToActionResult();
  }

  [HttpGet(ApiRoutes.Appointments.ByDoctor)]
  [ProducesResponseType(typeof(IEnumerable<AppointmentDto>), StatusCodes.Status200OK)]
  public async Task<IActionResult> GetByDoctor(Guid doctorId, CancellationToken ct)
  {
    var result = await _mediator.Send(new GetAppointmentsByDoctorQuery(doctorId), ct);
    return result.ToActionResult();
  }

  [HttpGet(ApiRoutes.Appointments.ByPatient)]
  [ProducesResponseType(typeof(IEnumerable<AppointmentDto>), StatusCodes.Status200OK)]
  public async Task<IActionResult> GetByPatient(Guid patientId, CancellationToken ct)
  {
    var result = await _mediator.Send(new GetAppointmentsByPatientQuery(patientId), ct);
    return result.ToActionResult();
  }

  [HttpPost]
  [ProducesResponseType(typeof(AppointmentDto), StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
  public async Task<IActionResult> Schedule(ScheduleAppointmentCommand command, CancellationToken ct)
  {
    var result = await _mediator.Send(command, ct);
    return result.ToCreatedResult(ApiRoutes.Appointments.GetById, new { id = result.IsSuccess ? result.Value.Id : Guid.Empty });
  }

  [HttpPatch(ApiRoutes.Appointments.Confirm)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> Confirm(Guid id, CancellationToken ct)
  {
    var result = await _mediator.Send(new ConfirmAppointmentCommand(id), ct);
    return result.ToActionResult();
  }

  [HttpPatch(ApiRoutes.Appointments.Complete)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> Complete(Guid id, [FromBody] string? notes, CancellationToken ct)
  {
    var result = await _mediator.Send(new CompleteAppointmentCommand(id, notes), ct);
    return result.ToActionResult();
  }

  [HttpPatch(ApiRoutes.Appointments.Cancel)]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> Cancel(Guid id, CancellationToken ct)
  {
    var result = await _mediator.Send(new CancelAppointmentCommand(id), ct);
    return result.ToActionResult();
  }
}
