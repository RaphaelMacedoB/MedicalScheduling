using AutoMapper;
using MedicalScheduling.Application.Features.Doctors.DTOs;
using MedicalScheduling.Application.Features.Patients.DTOs;
using MedicalScheduling.Application.Features.Specialities.DTOs;
using MedicalScheduling.Domain;

namespace MedicalScheduling.UnitTests.Common;

public static class MapperFactory
{
  private static readonly Lazy<IMapper> Mapper = new(() =>
  {
    var config = new MapperConfiguration(cfg =>
    {
      cfg.CreateMap<Patient, PatientDto>()
          .ForCtorParam(nameof(PatientDto.Email), o => o.MapFrom(s => s.Email.Value))
          .ForCtorParam(nameof(PatientDto.Cpf), o => o.MapFrom(s => s.Cpf.Value))
          .ForCtorParam(nameof(PatientDto.Phone), o => o.MapFrom(s => s.Phone.Value));

      cfg.CreateMap<Doctor, DoctorDto>()
          .ForCtorParam(nameof(DoctorDto.Email), o => o.MapFrom(s => s.Email.Value))
          .ForCtorParam(nameof(DoctorDto.Phone), o => o.MapFrom(s => s.Phone.Value))
          .ForCtorParam(nameof(DoctorDto.SpecialityName), o => o.MapFrom(s => s.Speciality.Name));

      cfg.CreateMap<Appointment, AppointmentDto>()
          .ConstructUsing(s => new AppointmentDto(
              s.Id,
              s.PatientId,
              s.Patient.Name,
              s.DoctorId,
              s.Doctor.Name,
              s.Doctor.Speciality.Name,
              s.TimeSlot.Start,
              s.TimeSlot.End,
              s.Status.ToString(),
              s.Notes,
              s.CreatedAt));

      cfg.CreateMap<Speciality, SpecialityDto>();
    });

    return config.CreateMapper();
  });

  public static IMapper Create() => Mapper.Value;
}
