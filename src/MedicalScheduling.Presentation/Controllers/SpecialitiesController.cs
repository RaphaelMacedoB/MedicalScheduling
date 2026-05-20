using MediatR;
using MedicalScheduling.Application.Features.Specialities.Commands.CreateSpeciality;
using MedicalScheduling.Application.Features.Specialities.Commands.UpdateSpeciality;
using MedicalScheduling.Application.Features.Specialities.DTOs;
using MedicalScheduling.Application.Features.Specialities.Queries.GetAllSpecialities;
using MedicalScheduling.Presentation.Constants;
using MedicalScheduling.Presentation.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace MedicalScheduling.Presentation.Controllers;

[ApiController]
[Route(ApiRoutes.Specialities.Base)]
public sealed class SpecialitiesController : ControllerBase
{
  private readonly IMediator _mediator;

  public SpecialitiesController(IMediator mediator) => _mediator = mediator;

  [HttpGet]
  [ProducesResponseType(typeof(IEnumerable<SpecialityDto>), StatusCodes.Status200OK)]
  public async Task<IActionResult> GetAll(CancellationToken ct)
  {
    var result = await _mediator.Send(new GetAllSpecialitiesQuery(), ct);
    return result.ToActionResult();
  }

  [HttpPost]
  [ProducesResponseType(typeof(SpecialityDto), StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
  public async Task<IActionResult> Create(CreateSpecialityCommand command, CancellationToken ct)
  {
    var result = await _mediator.Send(command, ct);
    return result.ToCreatedResult(ApiRoutes.Specialities.GetById, new { id = result.IsSuccess ? result.Value.Id : Guid.Empty });
  }

  [HttpPut(ApiRoutes.Specialities.ById, Name = ApiRoutes.Specialities.GetById)]
  [ProducesResponseType(typeof(SpecialityDto), StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status404NotFound)]
  public async Task<IActionResult> Update(Guid id, UpdateSpecialityCommand command, CancellationToken ct)
  {
    var result = await _mediator.Send(command with { Id = id }, ct);
    return result.ToActionResult();
  }
}
