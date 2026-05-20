using AutoMapper;
using MedicalScheduling.Application.Features.Appointments.DTOs;
using MedicalScheduling.Application.Features.Doctors.DTOs;
using MedicalScheduling.Application.Features.Patients.DTOs;
using MedicalScheduling.Application.Features.Specialities.DTOs;
using MedicalScheduling.Domain;

namespace MedicalScheduling.Application.Common.Mappings;

public sealed class MappingProfile : Profile
{
  public MappingProfile()
  {
    CreateMap<Patient, PatientDto>()
        .ForCtorParam(nameof(PatientDto.Email), o => o.MapFrom(s => s.Email.Value))
        .ForCtorParam(nameof(PatientDto.Cpf), o => o.MapFrom(s => s.Cpf.Value))
        .ForCtorParam(nameof(PatientDto.Phone), o => o.MapFrom(s => s.Phone.Value));

    CreateMap<Doctor, DoctorDto>()
        .ForCtorParam(nameof(DoctorDto.Email), o => o.MapFrom(s => s.Email.Value))
        .ForCtorParam(nameof(DoctorDto.Phone), o => o.MapFrom(s => s.Phone.Value))
        .ForCtorParam(nameof(DoctorDto.SpecialityName), o => o.MapFrom(s => s.Speciality.Name));

    CreateMap<Appointment, AppointmentDto>()
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

    CreateMap<Speciality, SpecialityDto>()
        .ForCtorParam(nameof(SpecialityDto.Id), o => o.MapFrom(s => s.Id))
        .ForCtorParam(nameof(SpecialityDto.Name), o => o.MapFrom(s => s.Name))
        .ForCtorParam(nameof(SpecialityDto.Description), o => o.MapFrom(s => s.Description))
        .ForCtorParam(nameof(SpecialityDto.CreatedAt), o => o.MapFrom(s => s.CreatedAt));
  }
}
