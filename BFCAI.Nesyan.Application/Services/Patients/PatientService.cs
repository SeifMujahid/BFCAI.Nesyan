using AutoMapper;
using BFCAI.Nesyan.Application.Abstraction.Models.Caregivers;
using BFCAI.Nesyan.Application.Abstraction.Models.IoT;
using BFCAI.Nesyan.Application.Abstraction.Models.MindGames;
using BFCAI.Nesyan.Application.Abstraction.Models.Patients;
using BFCAI.Nesyan.Application.Abstraction.Models.Reminders;
using BFCAI.Nesyan.Application.Abstraction.Services.Patients;
using BFCAI.Nesyan.Application.Common.Exceptions;
using BFCAI.Nesyan.Domain.Contracts;
using BFCAI.Nesyan.Domain.Entities.Medications;
using BFCAI.Nesyan.Domain.Entities.MindGames;
using BFCAI.Nesyan.Domain.Entities.Primary.Patients;
using BFCAI.Nesyan.Domain.Entities.Relations.MindGames;
using BFCAI.Nesyan.Domain.Specifications.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace BFCAI.Nesyan.Application.Services.Patients
{
    public class PatientService(IUnitOfWork UnitOfWork, IMapper Mapper) : IPatientService
    {
        public async Task UpdatePatientStageAsync(int patientId, int newStage)
        {
            var repo = UnitOfWork.GetRepository<Patient, int>();
            var patient = await repo.Get(patientId);

            if (patient == null)
                throw new Exception("Patient not found");

            patient.CurrentStage = (AlzheimerStage)newStage;
            patient.LastModifiedOn = DateTime.UtcNow;

            repo.Update(patient);
            await UnitOfWork.CompleteAsync();
        }

        public async Task<PatientHomeDto> GetPatientHome(int patientId)
        {
            var specs = new PatientSpecifications(patientId);
            var patient=await UnitOfWork.GetRepository<Patient,int>().GetWithSpecAsync(specs);
            if (patient is null)
                throw new NotFoundException(nameof(patient), patientId);
            var patientHomeDto = Mapper.Map<PatientHomeDto>(patient);
            return patientHomeDto;
        }

        public async Task<PatientFullProfileDto> GetPatientProfileAsync(int patientId)
        {
            var specs = new PatientFullProfileSpecifications(patientId);
            var patient = await UnitOfWork.GetRepository<Patient, int>().GetWithSpecAsync(specs);
            if (patient is null)
                throw new NotFoundException(nameof(patient), patientId);

            var profileDto = Mapper.Map<PatientFullProfileDto>(patient);

            // Fetch and map assigned games manually
            var sessionRepo = UnitOfWork.GetRepository<MindGameSession, int>();
            var gameRepo = UnitOfWork.GetRepository<MindGame, int>();

            var allSessions = await sessionRepo.GetAllAsync(false);
            var patientSessions = allSessions.Where(s => s.PatientId == patientId).ToList();

            var assignedGames = Mapper.Map<List<PatientMindGameDto>>(patientSessions);
            var allGames = await gameRepo.GetAllAsync(false);

            foreach (var dto in assignedGames)
            {
                var game = allGames.FirstOrDefault(g => g.Id == dto.MindGameId);
                if (game != null)
                {
                    dto.MindGame = Mapper.Map<MindGameDto>(game);
                }
            }

            profileDto.AssignedGames = assignedGames;
            return profileDto;
        }

        public async Task<IEnumerable<PatientSummaryDto>> GetPatientsAsync()
        {
            var repo = UnitOfWork.GetRepository<Patient, int>();
            var patients = await repo.GetAllAsync();
            return Mapper.Map<IEnumerable<PatientSummaryDto>>(patients);
        }

        public async Task<PatientRemindersDto> GetPatientReminder(int patientId, int reminderType)
        {
            var specs = new PatientSpecifications(patientId, reminderType);
            var patient = await UnitOfWork.GetRepository<Patient, int>().GetWithSpecAsync(specs);
            if (patient is null)
                throw new NotFoundException(nameof(patient), new { patientId, reminderType });
            PatientRemindersDto patientRemindersDto;
            switch (reminderType)
            {

                case 1:
                    patientRemindersDto = new PatientRemindersDto
                    {
                        PatientMedications =
                                Mapper.Map<PatientMedicationsDto>(patient),

                        AppointmentToReturn = null,

                        RoutineToReturn = null
                    };

                    break;


                case 2:

                    patientRemindersDto = new PatientRemindersDto
                    {
                        PatientMedications = null,

                        AppointmentToReturn =
                                Mapper.Map<PatientAppointmentsDto>(patient),

                        RoutineToReturn = null
                    };

                    break;


                case 3:

                    patientRemindersDto = new PatientRemindersDto
                    {
                        PatientMedications = null,

                        AppointmentToReturn = null,

                        RoutineToReturn =
                            Mapper.Map<PatientRoutineDto>(patient)
                    };

                    break;

                default:
                    throw new Exception("Invalid reminder type");
            }
            return patientRemindersDto;

        }

        public async Task<PatientToReturnDto> CreatePatientAsync(PatientToCreateDto patientToCreate)
        {
            var repo = UnitOfWork.GetRepository<Patient, int>();

            var existingPatients = await repo.GetAllAsync();
            if (existingPatients.Any(p => p.NationalId == patientToCreate.NationalId))
                throw new Exception("NationalId is already registered.");
            if (existingPatients.Any(p => p.Email.Equals(patientToCreate.Email, StringComparison.OrdinalIgnoreCase)))
                throw new Exception("Email is already registered.");
            if (existingPatients.Any(p => p.UserName.Equals(patientToCreate.UserName, StringComparison.OrdinalIgnoreCase)))
                throw new Exception("UserName is already taken.");

            var patient = Mapper.Map<Patient>(patientToCreate);
            patient.Password = BCrypt.Net.BCrypt.HashPassword(patient.Password);
            patient.CreatedOn = DateTime.UtcNow;
            patient.CreatedBy = patient.UserName;
            patient.LastModifiedOn = DateTime.UtcNow;
            patient.LastModifiedBy = patient.UserName;

            await repo.AddAsync(patient);
            await UnitOfWork.CompleteAsync();

            return Mapper.Map<PatientToReturnDto>(patient);
        }

        private string SaveFile(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0) return string.Empty;
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", folderName);
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            return $"/uploads/{folderName}/{uniqueFileName}";
        }

        public async Task UpdatePatientAsync(PatientToUpdateDto patientToUpdate)
        {
            var repo = UnitOfWork.GetRepository<Patient, int>();
            var patient = await repo.Get(patientToUpdate.Id);
            if (patient is null) throw new Exception("Patient not found");

            Mapper.Map(patientToUpdate, patient);

            if (patientToUpdate.Image != null)
            {
                patient.ImageUrl = SaveFile(patientToUpdate.Image, "patients/avatars");
            }

            patient.LastModifiedOn = DateTime.UtcNow;
            patient.LastModifiedBy = patient.UserName ?? "System";

            repo.Update(patient);
            await UnitOfWork.CompleteAsync();
        }

        public async Task DeletePatientAsync(int id)
        {
            var repo = UnitOfWork.GetRepository<Patient, int>();
            var patient = await repo.Get(id);
            if (patient is null) throw new Exception("Patient not found");

            repo.Delete(patient);
            await UnitOfWork.CompleteAsync();
        }

        public async Task CreateReminderAsync(int patientId, ReminderToCreateDto dto)
        {
            var patientRepo = UnitOfWork.GetRepository<Patient, int>();
            var patient = await patientRepo.Get(patientId);
            if (patient is null)
                throw new NotFoundException(nameof(Patient), patientId);

            var reminder = Mapper.Map<Medication>(dto);
            reminder.PatientId = patientId;
            reminder.CreatedBy = patient.UserName ?? "Patient";

            await UnitOfWork.GetRepository<Medication, int>().AddAsync(reminder);
            await UnitOfWork.CompleteAsync();
        }

        public async Task UpdateReminderAsync(int patientId, int reminderId, ReminderToUpdateDto dto)
        {
            var patientRepo = UnitOfWork.GetRepository<Patient, int>();
            var patient = await patientRepo.Get(patientId);
            if (patient is null)
                throw new NotFoundException(nameof(Patient), patientId);

            var reminderRepo = UnitOfWork.GetRepository<Medication, int>();
            var reminder = await reminderRepo.Get(reminderId);
            if (reminder is null || reminder.PatientId != patientId)
                throw new NotFoundException(nameof(Medication), new { reminderId, patientId });

            Mapper.Map(dto, reminder);
            reminder.LastModifiedBy = patient.UserName ?? "Patient";
            reminder.LastModifiedOn = DateTime.UtcNow;

            reminderRepo.Update(reminder);
            await UnitOfWork.CompleteAsync();
        }

        public async Task DeleteReminderAsync(int patientId, int reminderId)
        {
            var patientRepo = UnitOfWork.GetRepository<Patient, int>();
            var patient = await patientRepo.Get(patientId);
            if (patient is null)
                throw new NotFoundException(nameof(Patient), patientId);

            var reminderRepo = UnitOfWork.GetRepository<Medication, int>();
            var reminder = await reminderRepo.Get(reminderId);
            if (reminder is null || reminder.PatientId != patientId)
                throw new NotFoundException(nameof(Medication), new { reminderId, patientId });

            reminderRepo.Delete(reminder);
            await UnitOfWork.CompleteAsync();
        }
    }
}
