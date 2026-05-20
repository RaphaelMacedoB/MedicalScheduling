namespace MedicalScheduling.Application.Common.Pagination.Filters;

public sealed record AppointmentFilter : BaseFilter
{
  public Guid? PatientId { get; init; }
  public Guid? DoctorId { get; init; }
  public Guid? SpecialityId { get; init; }
  public string? Status { get; init; }
  public DateTime? StartFrom { get; init; }
  public DateTime? StartTo { get; init; }
}
