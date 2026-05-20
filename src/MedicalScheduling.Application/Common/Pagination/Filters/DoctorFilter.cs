namespace MedicalScheduling.Application.Common.Pagination.Filters;

public sealed record DoctorFilter : BaseFilter
{
  public Guid? SpecialityId { get; init; }
  public bool? IsActive { get; init; }
}
