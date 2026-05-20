namespace MedicalScheduling.Application.Common.Pagination.Filters;

public sealed record PatientFilter : BaseFilter
{
  public bool? IsActive { get; init; }
  public DateOnly? BirthDateFrom { get; init; }
  public DateOnly? BirthDateTo { get; init; }
}
