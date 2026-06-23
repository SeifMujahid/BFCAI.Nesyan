using AutoMapper;
using BFCAI.Nesyan.Application.Abstraction.Models._Relations.RelativePatient;
using BFCAI.Nesyan.Application.Abstraction.Models.Appointments;
using BFCAI.Nesyan.Application.Abstraction.Models.Assessments;
using BFCAI.Nesyan.Application.Abstraction.Models.Doctors;
using BFCAI.Nesyan.Application.Abstraction.Models.IoT;
using BFCAI.Nesyan.Application.Abstraction.Models.Patients;
using BFCAI.Nesyan.Application.Abstraction.Models.Relatives;
using BFCAI.Nesyan.Application.Abstraction.Models.Reminders;
using BFCAI.Nesyan.Application.Abstraction.Models.Reminders.Medications;
using BFCAI.Nesyan.Application.Abstraction.Models.Routines;
using BFCAI.Nesyan.Domain.Entities.Assessments;
using BFCAI.Nesyan.Domain.Entities.IoT;
using BFCAI.Nesyan.Domain.Entities.Medications;
using BFCAI.Nesyan.Domain.Entities.Primary.Patients;
using BFCAI.Nesyan.Domain.Entities.Primary.Relatives;
using BFCAI.Nesyan.Domain.Entities.Relations.Primary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Mapping
{
    internal class PatientRelativeMappingProfile:Profile
    {
        public PatientRelativeMappingProfile()
        {
            CreateMap<Patient, DoctorPatientDto>()
               .ForMember(
                   dest => dest.PatientSummary,
                   opt => opt.MapFrom(src => src))
              
               .ForMember(
                   dest => dest.DoctorSummary,
                   opt => opt.MapFrom(src => src.Doctor));
            CreateMap<PatientRelative, RelativePatientToCreateDto>();
            CreateMap<PatientTelemetry,
                TelemetryRequestDto>();

            CreateMap<Assessment,
                AssessmentsToReturnDto>()

                .ForMember(
                    dest => dest.RecognitionOfName,
                    opt => opt.MapFrom(src =>
                        src.RecognitionOfName.ToString()))

                .ForMember(
                    dest => dest.RecognitionOfPlace,
                    opt => opt.MapFrom(src =>
                        src.RecognitionOfPlace.ToString()))

                .ForMember(
                    dest => dest.RecognitionOfTime,
                    opt => opt.MapFrom(src =>
                        src.RecognitionOfTime.ToString()))

                .ForMember(
                    dest => dest.AbilityToConcentrate,
                    opt => opt.MapFrom(src =>
                        src.AbilityToConcentrate.ToString()))

                .ForMember(
                    dest => dest.RecallOfRecentEvents,
                    opt => opt.MapFrom(src =>
                        src.RecallOfRecentEvents.ToString()))

                .ForMember(
                    dest => dest.AnxietyOrStress,
                    opt => opt.MapFrom(src =>
                        src.AnxietyOrStress.ToString()))

                .ForMember(
                    dest => dest.DepressionOrSadness,
                    opt => opt.MapFrom(src =>
                        src.DepressionOrSadness.ToString()))

                .ForMember(
                    dest => dest.Aggression,
                    opt => opt.MapFrom(src =>
                        src.Aggression.ToString()))

                .ForMember(
                    dest => dest.EatingAndDrinking,
                    opt => opt.MapFrom(src =>
                        src.EatingAndDrinking.ToString()))

                .ForMember(
                    dest => dest.Bathing,
                    opt => opt.MapFrom(src =>
                        src.Bathing.ToString()))

                .ForMember(
                    dest => dest.Dressing,
                    opt => opt.MapFrom(src =>
                        src.Dressing.ToString()))

                .ForMember(
                    dest => dest.UsingBathroom,
                    opt => opt.MapFrom(src =>
                        src.UsingBathroom.ToString()))

                .ForMember(
                    dest => dest.Mobility,
                    opt => opt.MapFrom(src =>
                        src.Mobility.ToString()));


            // ================================
            // PatientRelative -> PatientSummary
            // ================================

            CreateMap<PatientRelative,
                PatientSummaryDto>()

                .ForMember(
                    dest => dest.PatientId,
                    opt => opt.MapFrom(src =>
                        src.Patient.Id))

                .ForMember(
                    dest => dest.FullName,
                    opt => opt.MapFrom(src =>
                        src.Patient.FName + " " +
                        src.Patient.LName))

                .ForMember(
                    dest => dest.Age,
                    opt => opt.MapFrom(src =>
                        src.Patient.Age))

                .ForMember(
                    dest => dest.Gender,
                    opt => opt.MapFrom(src =>
                        src.Patient.Gender))

                .ForMember(
                    dest => dest.CurrentStage,
                    opt => opt.MapFrom(src =>
                        src.Patient.CurrentStage))

                .ForMember(
                    dest => dest.CurrentStageName,
                    opt => opt.MapFrom(src =>
                        src.Patient.CurrentStage.ToString()))

                .ForMember(
                    dest => dest.Phone,
                    opt => opt.MapFrom(src =>
                        src.Patient.Phone))

                .ForMember(
                    dest => dest.ImageUrl,
                    opt => opt.MapFrom(src =>
                        src.Patient.ImageUrl))
                .ForMember(
                    dest => dest.DoctorId,
                    opt => opt.MapFrom(src =>
                        src.Patient.DoctorId))
                
                .ForMember(
                    dest => dest.CaregiverId,
                    opt => opt.MapFrom(src =>
                        src.Patient.CaregiverId))
                .ForMember(
                    dest => dest.NearestReminder,
                    opt => opt.MapFrom(src =>
                        GetNearestReminder(src.Patient.Reminders)));


            // ================================
            // Relative -> RelativeSummary
            // ================================

            CreateMap<Relative,
                RelativeSummaryDto>()

                .ForMember(
                    dest => dest.RelativeId,
                    opt => opt.MapFrom(src =>
                        src.Id))

                .ForMember(
                    dest => dest.FullName,
                    opt => opt.MapFrom(src =>
                        $"{src.FName} {src.LName}"));


            // ================================
            // Patient -> PatientSummary
            // ================================

            CreateMap<Patient,
                PatientSummaryDto>()

                .ForMember(
                    dest => dest.PatientId,
                    opt => opt.MapFrom(src =>
                        src.Id))

                .ForMember(
                    dest => dest.FullName,
                    opt => opt.MapFrom(src =>
                        $"{src.FName} {src.LName}"))

                .ForMember(
                    dest => dest.CurrentStage,
                    opt => opt.MapFrom(src =>
                        src.CurrentStage))

                .ForMember(
                    dest => dest.CurrentStageName,
                    opt => opt.MapFrom(src =>
                        src.CurrentStage.ToString()))

                .ForMember(
                    dest => dest.Phone,
                    opt => opt.MapFrom(src =>
                        src.Phone))

                .ForMember(
                    dest => dest.ImageUrl,
                    opt => opt.MapFrom(src =>
                        src.ImageUrl))

                .ForMember(
                    dest => dest.NearestReminder,
                    opt => opt.MapFrom(src =>
                        GetNearestReminder(src.Reminders)));

            // ================================
            // Patient -> PatientHome
            // ================================

            CreateMap<Patient,
                PatientHomeDto>()

                .ForMember(
                    dest => dest.Patient,
                    opt => opt.MapFrom(src => src))

                .ForMember(
                    dest => dest.LatestTelemetry,
                    opt => opt.MapFrom(src =>
                        src.PatientTelemetries
                            .OrderByDescending(t =>
                                t.CreatedOn)
                            .FirstOrDefault()))

                .ForMember(
                    dest => dest.LatestAssessment,
                    opt => opt.MapFrom(src =>
                        src.Assessments
                            .OrderByDescending(a =>
                                a.CreatedOn)
                            .FirstOrDefault()));
                // ================================
                // Reminder -> Medication DTO
                // ================================

                CreateMap<Medication,
                    MedicationToReturnDto>()

                    .ForMember(
                        dest => dest.Frequency,
                        opt => opt.MapFrom(src =>
                            src.Frequency.ToString()));


                // ================================
                // Reminder -> Appointment DTO
                // ================================

                CreateMap<Medication,
                    AppointmentToReturnDto>()

                    .ForMember(
                        dest => dest.Frequency,
                        opt => opt.MapFrom(src =>
                            src.Frequency.ToString()));


                // ================================
                // Reminder -> Routine DTO
                // ================================

                CreateMap<Medication,
                    RoutineToReturnDto>()

                    .ForMember(
                        dest => dest.Frequency,
                        opt => opt.MapFrom(src =>
                            src.Frequency.ToString()));


                // ================================
                // Patient -> PatientMedicationsDto
                // ================================

                CreateMap<Patient,
                    PatientMedicationsDto>()

                    .ForMember(
                        dest => dest.PatientSummary,
                        opt => opt.MapFrom(src => src))

                    .ForMember(
                        dest => dest.PatientMedications,
                        opt => opt.MapFrom(src =>
                            src.Reminders
                                .Where(m =>
                                    m.Type ==
                                    ReminderType.Medication)));


                // ================================
                // Patient -> PatientAppointmentsDto
                // ================================

                CreateMap<Patient,
                    PatientAppointmentsDto>()

                    .ForMember(
                        dest => dest.PatientSummary,
                        opt => opt.MapFrom(src => src))

                    .ForMember(
                        dest => dest.AppointmentToReturn,
                        opt => opt.MapFrom(src =>
                            src.Reminders
                                .Where(m =>
                                    m.Type ==
                                    ReminderType.Appointment)));


                // ================================
                // Patient -> PatientRoutineDto
                // ================================

                CreateMap<Patient,
                    PatientRoutineDto>()

                    .ForMember(
                        dest => dest.PatientSummary,
                        opt => opt.MapFrom(src => src))

                    .ForMember(
                        dest => dest.RoutineToReturn,
                        opt => opt.MapFrom(src =>
                            src.Reminders
                                .Where(m =>
                                    m.Type ==
                                    ReminderType.Routine)));
                CreateMap<ReminderToCreateDto, Medication>();
                CreateMap<ReminderToUpdateDto,Medication>();
        }

        private static PatientReminderSummaryDto? GetNearestReminder(ICollection<Medication>? reminders)
        {
            if (reminders == null || !reminders.Any())
                return null;

            TimeZoneInfo egyptTimeZone;
            try
            {
                egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
            }
            catch (TimeZoneNotFoundException)
            {
                try
                {
                    egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Africa/Cairo");
                }
                catch (TimeZoneNotFoundException)
                {
                    egyptTimeZone = TimeZoneInfo.Local;
                }
            }

            var localNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, egyptTimeZone);
            var todayDate = DateOnly.FromDateTime(localNow);
            var currentTime = TimeOnly.FromDateTime(localNow);

            var todayReminders = reminders
                .Where(r => {
                    if (r.ReminderDate > todayDate)
                        return false;

                    switch (r.Frequency)
                    {
                        case ReminderFrequency.OneTime:
                            return r.ReminderDate == todayDate;
                        case ReminderFrequency.Daily:
                            return true;
                        case ReminderFrequency.Weekly:
                            return r.ReminderDate.DayOfWeek == todayDate.DayOfWeek;
                        case ReminderFrequency.Monthly:
                            return r.ReminderDate.Day == todayDate.Day;
                        default:
                            return false;
                    }
                })
                .ToList();

            if (!todayReminders.Any())
                return null;

            var nearest = todayReminders
                .OrderBy(r => Math.Abs((r.ReminderTime.ToTimeSpan() - currentTime.ToTimeSpan()).Ticks))
                .First();

            return new PatientReminderSummaryDto
            {
                Id = nearest.Id,
                Title = nearest.Title,
                Name = nearest.Name,
                Dosage = nearest.Dosage,
                Type = nearest.Type.ToString(),
                ReminderTime = nearest.ReminderTime,
                ReminderDate = nearest.ReminderDate,
                Frequency = nearest.Frequency.ToString(),
                Notes = nearest.Notes
            };
        }
    }
}
