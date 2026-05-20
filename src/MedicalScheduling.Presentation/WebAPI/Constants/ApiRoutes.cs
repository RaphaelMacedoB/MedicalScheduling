namespace MedicalScheduling.Presentation.WebAPI.Constants;

public static class ApiRoutes
{
  public const string Prefix = "api/v1";

  public static class Patients
  {
    public const string Base = Prefix + "/patients";
    public const string ById = "{id:guid}";
    public const string GetById = "GetPatientById";
  }

  public static class Doctors
  {
    public const string Base = Prefix + "/doctors";
    public const string ById = "{id:guid}";
    public const string BySpeciality = "speciality/{specialityId:guid}";
    public const string GetById = "GetDoctorById";
  }

  public static class Appointments
  {
    public const string Base = Prefix + "/appointments";
    public const string ById = "{id:guid}";
    public const string ByDoctor = "doctor/{doctorId:guid}";
    public const string ByPatient = "patient/{patientId:guid}";
    public const string Confirm = "{id:guid}/confirm";
    public const string Complete = "{id:guid}/complete";
    public const string Cancel = "{id:guid}/cancel";
    public const string GetById = "GetAppointmentById";
  }

  public static class Specialities
  {
    public const string Base = Prefix + "/specialities";
    public const string ById = "{id:guid}";
    public const string GetById = "GetSpecialityById";
  }
}
