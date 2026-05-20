using MedicalScheduling.Domain.Primitives;

public static class DomainErrors
{
  public static class Patient
  {
    public static readonly Error NotFound = new("Patient.NotFound", "Paciente não encontrado");
    public static readonly Error InvalidCpf = new("Patient.InvalidCpf", "CPF inválido");
    public static readonly Error InvalidEmail = new("Patient.InvalidEmail", "E-mail inválido");
    public static readonly Error InvalidPhone = new("Patient.InvalidPhone", "Telefone inválido");
  }

  public static class Doctor
  {
    public static readonly Error NotFound = new("Doctor.NotFound", "Médico não encontrado");
    public static readonly Error InvalidCrm = new("Doctor.InvalidCrm", "CRM inválido");
    public static readonly Error Unavailable = new("Doctor.Unavailable", "Médico indisponível neste horário");
  }

  public static class Appointment
  {
    public static readonly Error NotFound = new("Appointment.NotFound", "Consulta não encontrada");
    public static readonly Error SlotUnavailable = new("Appointment.SlotUnavailable", "Horário indisponível");
    public static readonly Error AlreadyCancelled = new("Appointment.AlreadyCancelled", "Consulta já cancelada");
    public static readonly Error AlreadyCompleted = new("Appointment.AlreadyCompleted", "Consulta já finalizada");
    public static readonly Error InvalidTimeSlot = new("Appointment.InvalidTimeSlot", "Horário inválido");
    public static readonly Error PastDate = new("Appointment.PastDate", "Não é possível agendar em data passada");
  }

  public static class Speciality
  {
    public static readonly Error NotFound = new("Speciality.NotFound", "Especialidade não encontrada");
    public static readonly Error DuplicateName = new("Speciality.DuplicateName", "Especialidade já cadastrada");
  }
}