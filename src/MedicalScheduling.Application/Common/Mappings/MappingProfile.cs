using AutoMapper;
using MedicalScheduling.Application.Features.Doctors.DTOs;
using MedicalScheduling.Application.Features.Patients.DTOs;
using MedicalScheduling.Application.Features.Specialities.DTOs;
using MedicalScheduling.Domain;

public sealed class MappingProfile : Profile
{
  public MappingProfile()
  {
    CreateMap<Patient, PatientDto>()
        .ForMember(d => d.Email, o => o.MapFrom(s => s.Email.Value))
        .ForMember(d => d.Cpf, o => o.MapFrom(s => s.Cpf.Value))
        .ForMember(d => d.Phone, o => o.MapFrom(s => s.Phone.Value));

    CreateMap<Doctor, DoctorDto>()
        .ForMember(d => d.Email, o => o.MapFrom(s => s.Email.Value))
        .ForMember(d => d.Phone, o => o.MapFrom(s => s.Phone.Value))
        .ForMember(d => d.SpecialityName, o => o.MapFrom(s => s.Speciality.Name));

    CreateMap<Appointment, AppointmentDto>()
        .ForMember(d => d.Start, o => o.MapFrom(s => s.TimeSlot.Start))
        .ForMember(d => d.End, o => o.MapFrom(s => s.TimeSlot.End))
        .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
        .ForMember(d => d.PatientName, o => o.MapFrom(s => s.Patient.Name))
        .ForMember(d => d.DoctorName, o => o.MapFrom(s => s.Doctor.Name))
        .ForMember(d => d.SpecialityName, o => o.MapFrom(s => s.Doctor.Speciality.Name));

    CreateMap<Speciality, SpecialityDto>();
  }
}