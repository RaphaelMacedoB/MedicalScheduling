using System.Reflection;
using MedicalScheduling.Domain;

namespace MedicalScheduling.UnitTests.Common;

public static class EntityTestHelper
{
  public static Patient CreatePatient(
      string? name = null,
      string? email = null,
      string? cpf = null,
      string? phone = null,
      DateOnly? birthDate = null)
  {
    return Patient.Create(
        name ?? TestData.PatientName,
        email ?? TestData.ValidEmail,
        cpf ?? TestData.ValidCpf,
        phone ?? TestData.ValidPhone,
        birthDate ?? TestData.ValidBirthDate).Value;
  }

  public static Speciality CreateSpeciality(
      string? name = null,
      string? description = null)
  {
    return Speciality.Create(
        name ?? TestData.SpecialityName,
        description ?? "Descrição da especialidade").Value;
  }

  public static Doctor CreateDoctor(
      Speciality speciality,
      string? name = null,
      string? crm = null,
      string? email = null,
      string? phone = null)
  {
    var doctor = Doctor.Create(
        name ?? TestData.DoctorName,
        crm ?? TestData.Crm,
        email ?? TestData.ValidEmail,
        phone ?? TestData.ValidPhone,
        speciality.Id).Value;

    SetProperty(doctor, nameof(Doctor.Speciality), speciality);
    return doctor;
  }

  public static Appointment CreateAppointment(
      Patient patient,
      Doctor doctor,
      DateTime? start = null,
      DateTime? end = null)
  {
    var slotStart = start ?? TestData.FutureStart;
    var slotEnd = end ?? TestData.FutureEnd;

    var appointment = Appointment.Schedule(
        patient.Id,
        doctor.Id,
        slotStart,
        slotEnd).Value;

    SetProperty(appointment, nameof(Appointment.Patient), patient);
    SetProperty(appointment, nameof(Appointment.Doctor), doctor);
    return appointment;
  }

  private static void SetProperty<T>(object target, string propertyName, T value)
  {
    var property = target.GetType().GetProperty(
        propertyName,
        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

    property!.SetValue(target, value);
  }
}
