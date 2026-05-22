using AutoMapper;
using BFCAI.Nesyan.Application.Abstraction.Models._Relations.RelativePatient;
using BFCAI.Nesyan.Application.Abstraction.Models.Caregivers;
using BFCAI.Nesyan.Application.Abstraction.Models.Doctors;
using BFCAI.Nesyan.Application.Abstraction.Models.MindGames;
using BFCAI.Nesyan.Application.Abstraction.Models.Patients;
using BFCAI.Nesyan.Application.Abstraction.Models.Relatives;
using BFCAI.Nesyan.Application.Abstraction.Models.TreatmentRequests;
using BFCAI.Nesyan.Domain.Entities.Medications;
using BFCAI.Nesyan.Domain.Entities.MindGames;
using BFCAI.Nesyan.Domain.Entities.Primary;
using BFCAI.Nesyan.Domain.Entities.Primary.Caregivers;
using BFCAI.Nesyan.Domain.Entities.Primary.Doctors;
using BFCAI.Nesyan.Domain.Entities.Primary.Patients;
using BFCAI.Nesyan.Domain.Entities.Primary.Relatives;
using BFCAI.Nesyan.Domain.Entities.Relations.MindGames;
using BFCAI.Nesyan.Domain.Entities.Relations.Primary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Mapping
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Doctor, DoctorToReturnDto>();
            CreateMap<DoctorToCreateDto, Doctor>()
                    .ForMember(dest => dest.Gender,
                    opt => opt.MapFrom(src => Enum.Parse<Gender>(src.Gender, true)));

            CreateMap<RelativeDoctorRequest, TreatmentRequestToReturnDto>()
                .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

            CreateMap<TreatmentRequestToCreateDto, RelativeDoctorRequest>()
                .ForMember(d => d.RequestDate, o => o.MapFrom(s => s.RequestDate ?? DateTime.UtcNow));

            CreateMap<Patient, PatientToReturnDto>()
                .ForMember(d => d.Gender, o => o.MapFrom(s => s.Gender.ToString()))
                .ForMember(d => d.BloodType, o => o.MapFrom(s => s.BloodType.ToString()))
                .ForMember(d => d.CurrentStageName, o => o.MapFrom(s => s.CurrentStage.ToString()))
                .ForMember(d => d.Diseases, o => o.MapFrom(s => !string.IsNullOrEmpty(s.ChronicDisease)
                    ? s.ChronicDisease.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(d => d.Trim()).ToList()
                    : new List<string>()));

            CreateMap<PatientToCreateDto, Patient>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => Enum.Parse<Gender>(src.Gender, true)))
                .ForMember(dest => dest.BloodType, opt => opt.MapFrom(src => Enum.Parse<BloodType>(src.BloodType, true)))
                .ForMember(dest => dest.CurrentStage, opt => opt.MapFrom(src => (AlzheimerStage)src.CurrentStage))
                .ForMember(dest => dest.ChronicDisease, opt => opt.MapFrom(src => src.Diseases != null && src.Diseases.Count > 0 ? string.Join(",", src.Diseases) : string.Empty));

            CreateMap<PatientToReturnDto, Patient>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => Enum.Parse<Gender>(src.Gender, true)))
                .ForMember(dest => dest.BloodType, opt => opt.MapFrom(src => Enum.Parse<BloodType>(src.BloodType, true)))
                .ForMember(dest => dest.CurrentStage, opt => opt.MapFrom(src => (AlzheimerStage)src.CurrentStage))
                .ForMember(dest => dest.ChronicDisease, opt => opt.MapFrom(src => src.Diseases != null && src.Diseases.Count > 0 ? string.Join(",", src.Diseases) : string.Empty));

            CreateMap<Patient, PatientFullProfileDto>()
                .ForMember(d => d.CurrentStage, o => o.MapFrom(s => (int)s.CurrentStage))
                .ForMember(d => d.BloodType, o => o.MapFrom(s => s.BloodType.ToString()))
                .ForMember(d => d.CurrentStageName, o => o.MapFrom(s => s.CurrentStage.ToString()))
                .ForMember(d => d.Gender, o => o.MapFrom(s => s.Gender.ToString()))
                .ForMember(d => d.Diseases, o => o.MapFrom(s => !string.IsNullOrEmpty(s.ChronicDisease)
                    ? s.ChronicDisease.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(d => d.Trim()).ToList()
                    : new List<string>()));




            // Mind Games
            CreateMap<MindGame, MindGameDto>();
            CreateMap<MindGameSession, PatientMindGameDto>();

            // Relatives
            CreateMap<RelativeToCreateDto, Relative>()
                    .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => Enum.Parse<Gender>(src.Gender, true)));
            CreateMap<Relative, RelativeToReturnDto>()
                    .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()));
            CreateMap<RelativeToReturnDto, Relative>()
                    .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => Enum.Parse<Gender>(src.Gender, true)));

            // Caregivers
            CreateMap<CaregiverToCreateDto, Caregiver>()
                    .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => Enum.Parse<Gender>(src.Gender, true)));
            CreateMap<Caregiver, CaregiverToReturnDto>()
                    .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()));

            CreateMap<Caregiver, CaregiverSummaryDto>()
                    .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                    .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FName + " " + src.LName));

            CreateMap<CaregiverToReturnDto, Caregiver>()
                    .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => Enum.Parse<Gender>(src.Gender, true)));

        }
    }
}
