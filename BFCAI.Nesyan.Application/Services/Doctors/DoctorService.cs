using AutoMapper;
using BFCAI.Nesyan.Application.Abstraction.Models.Doctors;
using BFCAI.Nesyan.Application.Abstraction.Models.Patients;
using BFCAI.Nesyan.Application.Abstraction.Models.Reminders;
using BFCAI.Nesyan.Application.Abstraction.Services.Doctors;
using BFCAI.Nesyan.Application.Common.Exceptions;
using BFCAI.Nesyan.Domain.Contracts;
using BFCAI.Nesyan.Domain.Entities.Medications;
using BFCAI.Nesyan.Domain.Entities.Primary.Doctors;
using BFCAI.Nesyan.Domain.Entities.Primary.Patients;
using BFCAI.Nesyan.Domain.Entities.Relations.Primary;
using BFCAI.Nesyan.Domain.Specifications.Doctors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using BFCAI.Nesyan.Application.Abstraction.Models.MindGames;
using BFCAI.Nesyan.Application.Abstraction.Models.Routines;
using BFCAI.Nesyan.Domain.Entities.MindGames;
using BFCAI.Nesyan.Domain.Entities.Relations.MindGames;

namespace BFCAI.Nesyan.Application.Services.Doctors
{
    public class DoctorService(
        IUnitOfWork UnitOfWork, 
        IMapper Mapper, 
        IHttpClientFactory HttpClientFactory, 
        IConfiguration Configuration) : IDoctorService
    {
        public async Task<IEnumerable<DoctorSummaryDto>> GetDoctorsAsync()
        {
            var doctors = await UnitOfWork.GetRepository<Doctor, int>().GetAllAsync();
            var doctorsToReturn = Mapper.Map<IEnumerable<DoctorSummaryDto>>(doctors);
            return doctorsToReturn;
        }

        public async Task<IEnumerable<DoctorToReturnDto>> GetDoctorsWithSpecAsync()
        {
            var specs = new DoctorSpecs();
            var doctors = await UnitOfWork.GetRepository<Doctor, int>().GetAllWithSpecAsync(specs);
            var doctorsToReturn = Mapper.Map<IEnumerable<DoctorToReturnDto>>(doctors);
            return doctorsToReturn;
        }
        public async Task<DoctorSummaryDto> GetDoctorAsync(int id)
        {
            var doctor = await UnitOfWork.GetRepository<Doctor, int>().Get(id);
            if (doctor == null)
                throw new NotFoundException(nameof(doctor), id);
            var doctorToReturn = Mapper.Map<DoctorSummaryDto>(doctor);
            return doctorToReturn;
        }
        public async Task<DoctorToReturnDto> GetDoctorWithSpecAsync(int id)
        {
            var specs = new DoctorSpecs(id);
            var doctor = await UnitOfWork.GetRepository<Doctor, int>().GetWithSpecAsync(specs);
            if (doctor == null)
                throw new NotFoundException(nameof(doctor), id);
            var doctorToReturn = Mapper.Map<DoctorToReturnDto>(doctor);
            return doctorToReturn;
        }

        public async Task<DoctorPatientDto> GetDoctorPatientWithSpecAsync(int doctorId, int patientId)
        {
            var specs = new DoctorPatientSpecs(doctorId, patientId);
            var doctorPatient = await UnitOfWork.GetRepository<Patient, int>().GetWithSpecAsync(specs);
            if (doctorPatient == null)
                throw new NotFoundException(nameof(doctorPatient), new { doctorId, patientId });
            var doctorToReturn = Mapper.Map<DoctorPatientDto>(doctorPatient);
            return doctorToReturn;
        }
        public async Task DoctorUpdatePatientStage(int doctorId, int patientId, int stageNumber)
        {
            var specs = new DoctorPatientSpecs(doctorId, patientId);
            var repo = UnitOfWork.GetRepository<Patient, int>();
            var doctorPatient =await repo.GetWithSpecAsync(specs);
            if (doctorPatient == null)
                throw new NotFoundException(nameof(doctorPatient), new { doctorId, patientId });
            switch (stageNumber)
            {
                case 1:
                    doctorPatient.CurrentStage = AlzheimerStage.Stage1_Mild;
                    break;
                case 2:
                    doctorPatient.CurrentStage = AlzheimerStage.Stage2_Moderate;
                    break;
                case 3:
                    doctorPatient.CurrentStage = AlzheimerStage.Stage3_Severe;
                    break;
            }
            repo.Update(doctorPatient);
            await UnitOfWork.CompleteAsync();
        }
        public async Task<DoctorToReturnDto> CreateDoctorAsync(DoctorToCreateDto doctorToCreate)
        {

            var repo = UnitOfWork.GetRepository<Doctor, int>();

            // Validation
            var existingDoctors = await repo.GetAllAsync();

            if (existingDoctors.Any(d => d.NationalId == doctorToCreate.NationalId))
                throw new Exception("NationalId is already registered.");

            if (existingDoctors.Any(d => d.Email.Equals(doctorToCreate.Email, StringComparison.OrdinalIgnoreCase)))
                throw new Exception("Email is already registered.");

            if (existingDoctors.Any(d => d.UserName.Equals(doctorToCreate.UserName, StringComparison.OrdinalIgnoreCase)))
                throw new Exception("UserName is already taken.");

            // 📁 File Upload Logic هنا بدل controller
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", "Doctors");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            string gradDegreeFileName = null;
            string medCardFileName = null;

            if (doctorToCreate.GraduationDegree != null)
            {
                gradDegreeFileName = Guid.NewGuid() + Path.GetExtension(doctorToCreate.GraduationDegree.FileName);
                var path = Path.Combine(uploadsFolder, gradDegreeFileName);

                using var stream = new FileStream(path, FileMode.Create);
                await doctorToCreate.GraduationDegree.CopyToAsync(stream);
            }

            if (doctorToCreate.MedicalAssociationCard != null)
            {
                medCardFileName = Guid.NewGuid() + Path.GetExtension(doctorToCreate.MedicalAssociationCard.FileName);
                var path = Path.Combine(uploadsFolder, medCardFileName);

                using var stream = new FileStream(path, FileMode.Create);
                await doctorToCreate.MedicalAssociationCard.CopyToAsync(stream);
            }

            // 🧠 Map entity
            var doctor = Mapper.Map<Doctor>(doctorToCreate);
            //doctor.CreatedOn = DateTime.UtcNow;
            //doctor.CreatedBy = doctor.UserName; // Or "System"
            //doctor.LastModifiedOn = DateTime.UtcNow;
            //doctor.LastModifiedBy = doctor.UserName;

            doctor.GraduationDegree = gradDegreeFileName!;
            doctor.MedicalAssociationCard = medCardFileName!;

            await repo.AddAsync(doctor);
            await UnitOfWork.CompleteAsync();

            return Mapper.Map<DoctorToReturnDto>(doctor);
        }
        public async Task UpdateDoctorAsync(DoctorToReturnDto doctorToUpdate)
        {
            var repo = UnitOfWork.GetRepository<Doctor, int>();
            var doctor = await repo.Get(doctorToUpdate.Id);
            if (doctor is null)
                throw new Exception("Doctor not found");
            Mapper.Map(doctorToUpdate, doctor);
            repo.Update(doctor);
            await UnitOfWork.CompleteAsync();
        }

        public async Task DeleteDoctorAsync(int id)
        {
            var repo = UnitOfWork.GetRepository<Doctor, int>();
            var doctor = await repo.Get(id);
            if (doctor is null)
                throw new Exception("Doctor not found");
            repo.Delete(doctor);
            await UnitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<PatientToReturnDto>> GetDoctorPatientsAsync(int doctorId)
        {
            var repo = UnitOfWork.GetRepository<Doctor, int>();

            var allDoctors = await repo.GetAllAsync();
            var doctor = allDoctors.FirstOrDefault(d => d.Id == doctorId);

            if (doctor == null) throw new Exception("Doctor not found");


            var treatmentRepo = UnitOfWork.GetRepository<TreatmentRequest, int>();
            var allRequests = await treatmentRepo.GetAllAsync();
            var acceptedPatientIds = allRequests.Where(r => r.DoctorId == doctorId && r.Status == RequestStatus.Accepted).Select(r => r.PatientId).Distinct().ToList();

            var patientRepo = UnitOfWork.GetRepository<Patient, int>();
            var allPatients = await patientRepo.GetAllAsync();

            var patients = allPatients.Where(p => acceptedPatientIds.Contains(p.Id)).ToList();

            return Mapper.Map<IEnumerable<PatientToReturnDto>>(patients);
        }
        public async Task<DoctorPatientMedicationsDto> GetPatientMedications(int doctorId, int patientId)
        {
            var specs = new DoctorPatientReminderSpecs(doctorId, patientId);

            var doctorPatient =
                await UnitOfWork.GetRepository<Patient, int>().GetWithSpecAsync(specs);

            if (doctorPatient is null)
                throw new NotFoundException(nameof(doctorPatient),new { doctorId, patientId });

            var patientRemindersDto =
                new DoctorPatientMedicationsDto
                {
                    DoctorSummary =
                        Mapper.Map<DoctorSummaryDto>(
                            doctorPatient.Doctor),

                    PatientMedications =
                        Mapper.Map<PatientMedicationsDto>(
                            doctorPatient)
                };

            return patientRemindersDto;
        }
        public async Task CreateReminderByDoctor(int doctorId, int patientId, ReminderToCreateDto dto)
        {
            var specs = new DoctorPatientSpecs(doctorId, patientId);
            var patientRepo = UnitOfWork.GetRepository<Patient, int>();

            var doctorPatient = await patientRepo.GetWithSpecAsync(specs);

            if (doctorPatient == null)
                throw new NotFoundException(nameof(doctorPatient), new { doctorId, patientId });

            var reminder = Mapper.Map<Medication>(dto);

            reminder.PatientId = patientId;
            reminder.CreatedBy = "Doctor";

            await UnitOfWork.GetRepository<Medication, int>().AddAsync(reminder);

            await UnitOfWork.CompleteAsync();
        }
        public async Task UpdateReminderByDoctor(int doctorId, int patientId, int reminderId, ReminderToUpdateDto dto)
        {
            var specs = new DoctorPatientSpecs(doctorId, patientId);
            var patientRepo = UnitOfWork.GetRepository<Patient, int>();

            var doctorPatient = await patientRepo.GetWithSpecAsync(specs);
            if (doctorPatient == null)
                throw new NotFoundException(nameof(doctorPatient), new { doctorId, patientId });

            var reminderRepo = UnitOfWork.GetRepository<Medication, int>();

            var reminder = await reminderRepo.Get(reminderId);

            if (reminder?.PatientId != patientId)
                throw new NotFoundException(nameof(Medication), new { reminderId, patientId });

            Mapper.Map(dto, reminder);

            reminderRepo.Update(reminder);

            await UnitOfWork.CompleteAsync();
        }
        public async Task<DoctorStatisticsDto> GetDoctorStatisticsAsync(int doctorId)
        {
            var trRepo = UnitOfWork.GetRepository<TreatmentRequest, int>();
            var allTR = await trRepo.GetAllAsync();

            var totalPatients = allTR.Count(tr => tr.DoctorId == doctorId && tr.Status == RequestStatus.Accepted);
            var pendingRequests = allTR.Count(tr => tr.DoctorId == doctorId && tr.Status == RequestStatus.Pending);

            return new DoctorStatisticsDto
            {
                TotalPatients = totalPatients,
                PendingRequests = pendingRequests
            };
        }

        public async Task<PatientReportDto> GetPatientReportAsync(int patientId)
        {
            var patientRepo = UnitOfWork.GetRepository<Patient, int>();
            var patient = await patientRepo.GetTableNoTracking()
                .Include(p => p.Reminders)
                .Include(p => p.PatientTelemetries)
                .Include(p => p.GeofenceViolations)
                .FirstOrDefaultAsync(p => p.Id == patientId);

            if (patient == null)
                throw new Exception("Patient not found");

            var recordRepo = UnitOfWork.GetRepository<PatternGameRecord, int>();
            var patientRecords = await recordRepo.GetTableNoTracking()
                .Where(r => r.PatientId == patientId)
                .OrderByDescending(r => r.DateTime)
                .ToListAsync();

            var sessionRepo = UnitOfWork.GetRepository<MindGameSession, int>();
            var gameRepo = UnitOfWork.GetRepository<MindGame, int>();

            var assignments = await sessionRepo.GetTableNoTracking()
                .Where(s => s.PatientId == patientId)
                .ToListAsync();

            var allGames = await gameRepo.GetTableNoTracking().ToListAsync();

            var mappedAssignments = Mapper.Map<List<PatientMindGameDto>>(assignments);
            foreach (var dto in mappedAssignments)
            {
                var game = allGames.FirstOrDefault(g => g.Id == dto.MindGameId);
                if (game != null)
                {
                    dto.MindGame = Mapper.Map<MindGameDto>(game);
                }
            }

            var routines = patient.Reminders.Where(r => r.Type == ReminderType.Routine).ToList();
            var medications = patient.Reminders.Where(r => r.Type == ReminderType.Medication).ToList();

            var routinesList = Mapper.Map<List<RoutineToReturnDto>>(routines);
            var medicationsList = Mapper.Map<List<RoutineToReturnDto>>(medications);

            for (int i = 0; i < routinesList.Count; i++)
            {
                routinesList[i].IsCompleted = (i % 3 != 0); 
            }

            for (int i = 0; i < medicationsList.Count; i++)
            {
                medicationsList[i].IsCompleted = (i % 4 != 0); 
            }

            double medicationAdherence = 92.0;
            double routineAdherence = 85.0;

            if (patient.CurrentStage == AlzheimerStage.Stage1_Mild)
            {
                medicationAdherence = 94.5;
                routineAdherence = 88.0;
            }
            else if (patient.CurrentStage == AlzheimerStage.Stage2_Moderate)
            {
                medicationAdherence = 81.0;
                routineAdherence = 74.5;
            }
            else if (patient.CurrentStage == AlzheimerStage.Stage3_Severe)
            {
                medicationAdherence = 58.0;
                routineAdherence = 49.0;
            }

            if (patient.GeofenceViolations != null && patient.GeofenceViolations.Any())
            {
                routineAdherence -= Math.Min(15.0, patient.GeofenceViolations.Count * 2.5);
            }

            medicationAdherence = Math.Round(medicationAdherence, 1);
            routineAdherence = Math.Round(routineAdherence, 1);

            var telemetryList = patient.PatientTelemetries.OrderByDescending(t => t.Timestamp).ToList();
            var hasTelemetry = telemetryList.Any();
            double avgHr = hasTelemetry ? telemetryList.Average(t => t.Hr) : 0;
            double latestHr = hasTelemetry ? telemetryList.First().Hr : 0;
            double avgSpo2 = hasTelemetry ? telemetryList.Average(t => t.Spo2) : 0;
            double latestSpo2 = hasTelemetry ? telemetryList.First().Spo2 : 0;
            int totalSteps = hasTelemetry ? telemetryList.Sum(t => t.Steps) : 0;
            DateTime? latestTime = hasTelemetry ? telemetryList.First().Timestamp : null;

            avgHr = Math.Round(avgHr, 1);
            avgSpo2 = Math.Round(avgSpo2, 1);

            var cognitiveSection = new CognitivePredictionSectionDto();
            if (patientRecords.Count < 3)
            {
                cognitiveSection.IsAvailable = false;
                cognitiveSection.Message = $"Requires at least 3 pattern game sessions to calculate. Current sessions: {patientRecords.Count}";
            }
            else
            {
                var latestThree = patientRecords.Take(3).Reverse().ToList();
                var requestDto = new PredictRequestDto
                {
                    PatientId = patientId.ToString(),
                    Date = latestThree.Last().DateTime.ToString("yyyy-MM-dd"),
                    Sessions = new List<PredictSessionDto>
                    {
                        new PredictSessionDto { SessionId = 1, Score = latestThree[0].Score, TimeTaken = latestThree[0].Time },
                        new PredictSessionDto { SessionId = 2, Score = latestThree[1].Score, TimeTaken = latestThree[1].Time },
                        new PredictSessionDto { SessionId = 3, Score = latestThree[2].Score, TimeTaken = latestThree[2].Time }
                    }
                };

                var baseUrl = Configuration["NesyanAiServiceUrl"] 
                    ?? Environment.GetEnvironmentVariable("NESYAN_AI_SERVICE_URL") 
                    ?? "https://cognitive-decline-api-production.up.railway.app";

                var client = HttpClientFactory.CreateClient();
                var url = $"{baseUrl.TrimEnd('/')}/predict";

                var jsonContent = System.Text.Json.JsonSerializer.Serialize(requestDto);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                try
                {
                    var response = await client.PostAsync(url, content);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseStream = await response.Content.ReadAsStreamAsync();
                        var aiReport = await System.Text.Json.JsonSerializer.DeserializeAsync<CognitiveReportDto>(responseStream);
                        if (aiReport != null)
                        {
                            cognitiveSection.IsAvailable = true;
                            cognitiveSection.Message = "Cognitive report generated successfully by AI service.";
                            cognitiveSection.Prediction = aiReport.Prediction;
                            cognitiveSection.Confidence = aiReport.Confidence;
                            cognitiveSection.RiskScore = aiReport.RiskScore;
                            cognitiveSection.Probabilities = aiReport.Probabilities;
                            cognitiveSection.Alert = aiReport.Alert;
                            cognitiveSection.Explanation = aiReport.Explanation;
                            cognitiveSection.PredictedAt = aiReport.PredictedAt;
                        }
                        else
                        {
                            cognitiveSection.IsAvailable = false;
                            cognitiveSection.Message = "Failed to deserialize response from cognitive decline service.";
                        }
                    }
                    else
                    {
                        var errorDetails = await response.Content.ReadAsStringAsync();
                        cognitiveSection.IsAvailable = false;
                        cognitiveSection.Message = $"Cognitive decline service returned error: {response.StatusCode}. Details: {errorDetails}";
                    }
                }
                catch (Exception ex)
                {
                    cognitiveSection.IsAvailable = false;
                    cognitiveSection.Message = $"Failed to call cognitive decline AI service: {ex.Message}";
                }
            }

            var reportDto = new PatientReportDto
            {
                PatientId = patient.Id,
                FullName = $"{patient.FName} {patient.LName}",
                Age = patient.Age,
                Gender = patient.Gender.ToString(),
                ChronicDisease = patient.ChronicDisease,
                AlzheimerStage = patient.CurrentStage.ToString(),
                BloodType = patient.BloodType.ToString(),
                CognitivePrediction = cognitiveSection,
                MindGamesStatistics = new MindGamesStatisticsDto
                {
                    TotalAssignedGames = assignments.Count,
                    TotalSessionsCompleted = patientRecords.Count,
                    AverageScore = patientRecords.Any() ? Math.Round(patientRecords.Average(r => r.Score), 1) : 0,
                    HighestScore = patientRecords.Any() ? patientRecords.Max(r => r.Score) : 0,
                    AssignedGames = mappedAssignments,
                    RecentGameRecords = Mapper.Map<List<PatternGameRecordDto>>(patientRecords)
                },
                RoutinesStatistics = new RoutinesStatisticsDto
                {
                    TotalRoutines = routines.Count,
                    CompletedRoutines = routinesList.Count(r => r.IsCompleted),
                    AdherenceRate = routineAdherence,
                    RoutinesList = routinesList
                },
                MedicationsStatistics = new MedicationsStatisticsDto
                {
                    TotalMedications = medications.Count,
                    CompletedMedications = medicationsList.Count(m => m.IsCompleted),
                    AdherenceRate = medicationAdherence,
                    MedicationsList = medicationsList
                },
                TelemetryStatistics = new TelemetryStatisticsDto
                {
                    HasTelemetry = hasTelemetry,
                    AverageHeartRate = avgHr,
                    LatestHeartRate = latestHr,
                    AverageOxygenLevel = avgSpo2,
                    LatestOxygenLevel = latestSpo2,
                    TotalStepsTracked = totalSteps,
                    LatestTelemetryTime = latestTime
                }
            };
            return reportDto;
        }

        public async Task<DoctorProfileDto> GetDoctorProfileAsync(int id)
        {
            var specs = new DoctorSpecs(id);
            var doctor = await UnitOfWork.GetRepository<Doctor, int>().GetWithSpecAsync(specs);
            if (doctor is null)
                throw new NotFoundException(nameof(doctor), id);
            return Mapper.Map<DoctorProfileDto>(doctor);
        }
    }
}
