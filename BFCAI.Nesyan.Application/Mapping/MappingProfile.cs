using AutoMapper;
using BFCAI.Nesyan.Application.Abstraction.Models.Doctors;
using BFCAI.Nesyan.Domain.Entities.Primary;
using BFCAI.Nesyan.Domain.Entities.Primary.Doctor;
using BFCAI.Nesyan.Application.Abstraction.Models.TreatmentRequests;
using BFCAI.Nesyan.Application.Abstraction.Models.Patients;
using BFCAI.Nesyan.Domain.Entities.Primary.Patient;
using BFCAI.Nesyan.Application.Abstraction.Models.Medications;

using BFCAI.Nesyan.Application.Abstraction.Models.MindGames;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BFCAI.Nesyan.Domain.Entities.Relations.Primary;
using BFCAI.Nesyan.Domain.Entities.MindGames;
using BFCAI.Nesyan.Domain.Entities.Relations.MindGames;

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

            CreateMap<TreatmentRequestToCreateDto,RelativeDoctorRequest>();

            CreateMap<Patient, PatientToReturnDto>()
                .ForMember(d => d.CurrentStageName, o => o.MapFrom(s => s.CurrentStage.ToString()));

            CreateMap<Patient, PatientFullProfileDto>()
                .ForMember(d => d.CurrentStageName, o => o.MapFrom(s => s.CurrentStage.ToString()))
                .ForMember(d => d.Gender, o => o.MapFrom(s => s.Gender.ToString()));


            // Medications
            //CreateMap<MedicationToCreateDto, Medication>();
            //CreateMap<Medication, MedicationToReturnDto>();

            // Mind Games
            CreateMap<MindGame, MindGameDto>();
            CreateMap<MindGameSession, PatientMindGameDto>();
        }
    }
}
