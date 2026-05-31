using AutoMapper;
using BFCAI.Nesyan.Application.Abstraction.Models._Relations.RelativePatient;
using BFCAI.Nesyan.Application.Abstraction.Models.Appointments;
using BFCAI.Nesyan.Application.Abstraction.Models.Patients;
using BFCAI.Nesyan.Application.Abstraction.Models.Relatives;
using BFCAI.Nesyan.Application.Abstraction.Models.Reminders;
using BFCAI.Nesyan.Application.Abstraction.Models.Reminders.Medications;
using BFCAI.Nesyan.Application.Abstraction.Models.Routines;
using BFCAI.Nesyan.Application.Abstraction.Services._Relations;
using BFCAI.Nesyan.Application.Abstraction.Services;
using BFCAI.Nesyan.Application.Abstraction.Models.Auth;
using BFCAI.Nesyan.Application.Common.Exceptions;
using BFCAI.Nesyan.Domain.Contracts;
using BFCAI.Nesyan.Domain.Entities.Medications;
using BFCAI.Nesyan.Domain.Entities.Primary.Patients;
using BFCAI.Nesyan.Domain.Entities.Primary.Relatives;
using BFCAI.Nesyan.Domain.Entities.Primary.Doctors;
using BFCAI.Nesyan.Domain.Entities.Primary.Caregivers;
using BFCAI.Nesyan.Domain.Entities.Relations.Primary;
using BFCAI.Nesyan.Domain.Specifications.PatientRelatives;
using BFCAI.Nesyan.Domain.Specifications.Patients;
using BFCAI.Nesyan.Domain.Specifications.Relatives;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Services._Reltaions.RelativePatient
{
    public class RelativePatientService(IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService) : IRelativePatientService
    {
        public async Task<PatientSummaryV2Dto> RelativeSearchByUserName(string userName)
        {
            var specs =new PatientSerachSpecifications(userName);
            var patient =await unitOfWork.GetRepository<Patient, int>().GetWithSpecAsync(specs);
            if(patient == null)
                throw new NotFoundException(nameof(patient),userName);
            return mapper.Map<PatientSummaryV2Dto>(patient);
        }
        public async Task AddExistingPatient(int relativeId, VerifyPatientDto dto)
        {
            var specs = new PatientSerachSpecifications(dto.NationalId, dto.Email);
            var patient = await unitOfWork.GetRepository<Patient, int>().GetWithSpecAsync(specs);
            if (patient == null)
                throw new BadRequestException("Data Incorrect");
            var repo = unitOfWork.GetRepository<PatientRelative, int>();
            await repo.AddAsync(new PatientRelative { RelativeId = relativeId, PatientId = patient.Id });
            await unitOfWork.CompleteAsync();
        }

        public async Task CreateRelativePatientRelation(int relativeId,int patientId)
        {
            var repo =unitOfWork.GetRepository<PatientRelative,int>();
            await repo.AddAsync(new PatientRelative { RelativeId = relativeId, PatientId = patientId });
            await unitOfWork.CompleteAsync();
        }
        public async Task<RelativePatientsDto> GetRelativePatients(int relativeId)
        {
            var spec = new GetRelativePatientsSpecification(relativeId);
            var relative = await unitOfWork.GetRepository<Relative, int>().GetWithSpecAsync(spec);
            var relativePatientsDto = new RelativePatientsDto
            {
                RelativeSummary =
                    mapper.Map<RelativeSummaryDto>(relative),

                PatientsSummary =
                    mapper.Map<IEnumerable<PatientSummaryDto>>(
                    relative?.Patients)
            };
            return relativePatientsDto;
        }
        public async Task<RelativePatientHomeDto> GetPatientHomeAsync(int relativeId, int patientId)
        {
            var sepcs = new PatientRelativeHomeSpecifications(relativeId, patientId);
            var relativePatient = await unitOfWork.GetRepository<PatientRelative, int>().GetWithSpecAsync(sepcs);
            var relativePatientsDto = new RelativePatientHomeDto
            {
                RelativeSummary =
                  mapper.Map<RelativeSummaryDto>(relativePatient?.Relative),

                Patient =
                  mapper.Map<PatientHomeDto>(relativePatient?.Patient)

            };
            return relativePatientsDto;
        }
        public async Task<RelativePatientRemindersDto> GetPatientReminders(int relativeId, int patientId, int reminderType)
        {
            var specs = new RelativePatientRemindersSpecifications(relativeId, patientId);
            var relativePatient = await unitOfWork.GetRepository<PatientRelative, int>().GetWithSpecAsync(specs);
            if (relativePatient is null)
                throw new NotFoundException(nameof(relativePatient), new { rId = relativeId, pId = patientId });
            RelativePatientRemindersDto patientsRemindesDto;

            switch (reminderType)
            {
                case 1:
                    patientsRemindesDto = new RelativePatientRemindersDto
                    {

                        RelativeSummary =
                         mapper.Map<RelativeSummaryDto>(relativePatient?.Relative),

                        PatientMedications =
                         mapper.Map<PatientMedicationsDto>(relativePatient?.Patient),

                        PatientAppointments = null,
                        PatientRoutines = null

                    }; ;


                    break;


                case 2:

                    patientsRemindesDto = new RelativePatientRemindersDto
                    {

                        RelativeSummary =
                         mapper.Map<RelativeSummaryDto>(relativePatient?.Relative),

                        PatientMedications = null,

                        PatientAppointments =
                         mapper.Map<PatientAppointmentsDto>(relativePatient?.Patient),

                        PatientRoutines = null

                    }; ;

                    break;
                case 3:
                    patientsRemindesDto = new RelativePatientRemindersDto
                    {

                        RelativeSummary =
                         mapper.Map<RelativeSummaryDto>(relativePatient?.Relative),

                        PatientMedications = null,

                        PatientAppointments = null,

                        PatientRoutines = mapper.Map<PatientRoutineDto>(relativePatient?.Patient)

                    }; ;

                    break;


                default:
                    throw new Exception("Invalid reminder type");
            }
            return patientsRemindesDto;
        }
        public async Task CreateReminder(int relativeId, int patientId, ReminderToCreateDto dto)
        {
            var spec = new RelativePatientCheckSpecifications(relativeId, patientId);
            var relation = await unitOfWork.GetRepository<PatientRelative, int>().GetWithSpecAsync(spec);

            if (relation is null)
                throw new NotFoundException(nameof(PatientRelative), new { relativeId, patientId });

            var reminder =
                 mapper.Map<Medication>(dto);

            reminder.PatientId = patientId;

            await unitOfWork.GetRepository<Medication, int>().AddAsync(reminder);

            await unitOfWork.CompleteAsync();
        }
        public async Task UpdateReminder(int relativeId,int patientId,int reminderId,ReminderToUpdateDto dto)
        {
            var relationSpec = new RelativePatientCheckSpecifications(relativeId,patientId);

            var relation =await unitOfWork.GetRepository<PatientRelative, int>().GetWithSpecAsync(relationSpec);

            if (relation is null)
                throw new NotFoundException( nameof(PatientRelative),new{relativeId,patientId});

            var reminderRepo =unitOfWork.GetRepository<Medication, int>();

            var reminder =await reminderRepo.Get(reminderId);

            if (reminder?.PatientId != patientId)
                throw new NotFoundException(nameof(Medication), new { reminderId, patientId });

            mapper.Map(dto, reminder);

            reminderRepo.Update(reminder);

            await unitOfWork.CompleteAsync();
        }
        public async Task DeleteReminder(int relativeId,int patientId,int reminderId)
        {
            var relationSpec =new RelativePatientCheckSpecifications(relativeId,patientId);

            var relation =await unitOfWork.GetRepository<PatientRelative, int>().GetWithSpecAsync(relationSpec);

            if (relation is null)
                  throw new NotFoundException( nameof(PatientRelative),new { relativeId, patientId });

            var reminderRepo = unitOfWork.GetRepository<Medication, int>();

            var reminder =await reminderRepo.Get(reminderId);

            if (reminder?.PatientId != patientId)
                throw new NotFoundException(nameof(Medication), new { reminderId, patientId });


            reminderRepo.Delete(reminder);

            await unitOfWork.CompleteAsync();
        }

        public async Task DeletePatientFromRelative(int relativeId, int patientId)
        {
            var relationSpec = new RelativePatientCheckSpecifications(relativeId, patientId);

            var relation = await unitOfWork.GetRepository<PatientRelative, int>().GetWithSpecAsync(relationSpec);

            if (relation is null)
                throw new NotFoundException(nameof(PatientRelative), new { relativeId, patientId });

            unitOfWork.GetRepository<PatientRelative, int>().Delete(relation);

            await unitOfWork.CompleteAsync();
        }

        private async Task<bool> IsEmailOrNationalIdOrUsernameExistsAsync(string email, string nationalId, string userName)
        {
            var patients = await unitOfWork.GetRepository<Patient, int>().GetTableNoTracking().AnyAsync(x => x.Email == email || x.NationalId == nationalId || x.UserName == userName);
            var doctors = await unitOfWork.GetRepository<Doctor, int>().GetTableNoTracking().AnyAsync(x => x.Email == email || x.NationalId == nationalId || x.UserName == userName);
            var relatives = await unitOfWork.GetRepository<Relative, int>().GetTableNoTracking().AnyAsync(x => x.Email == email || x.NationalId == nationalId || x.UserName == userName);
            var caregivers = await unitOfWork.GetRepository<Caregiver, int>().GetTableNoTracking().AnyAsync(x => x.Email == email || x.NationalId == nationalId || x.UserName == userName);
            
            return patients || doctors || relatives || caregivers;
        }

        private string SaveFile(IFormFile? file, string folderName)
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

        public async Task<AuthResponseDto> RegisterAndAddPatientAsync(int relativeId, RegisterPatientDto dto)
        {
            var relative = await unitOfWork.GetRepository<Relative, int>().Get(relativeId);
            if (relative == null)
                throw new NotFoundException(nameof(Relative), relativeId);

            if (await IsEmailOrNationalIdOrUsernameExistsAsync(dto.Email, dto.NationalId, dto.UserName))
                return new AuthResponseDto { IsSuccess = false, Message = "Email, National ID, or Username is already registered." };

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            var imageUrl = dto.Image != null ? SaveFile(dto.Image, "patients/avatars") : null;

            var patient = new Patient
            {
                Email = dto.Email,
                UserName = dto.UserName,
                NationalId = dto.NationalId,
                Password = hashedPassword,
                FName = dto.FName,
                LName = dto.LName,
                Phone = dto.Phone,
                Gender = dto.Gender,
                MaritalStatus = dto.MaritalStatus,
                ImageUrl = imageUrl,
                Country = dto.Country,
                City = dto.City,
                Age = dto.Age,
                CurrentStage = dto.CurrentStage,
                Height = dto.Height,
                Weight = dto.Weight,
                BloodType = dto.BloodType,
                ChronicDisease = dto.Diseases != null && dto.Diseases.Count > 0 
                    ? string.Join(",", dto.Diseases) 
                    : string.Empty,
                CreatedBy = "System",
                CreatedOn = DateTime.UtcNow,
                LastModifiedBy = "System",
                LastModifiedOn = DateTime.UtcNow,
                IsVerified = true,
                VerificationCode = null,
                VerificationCodeExpires = null
            };

            await unitOfWork.GetRepository<Patient, int>().AddAsync(patient);
            await unitOfWork.CompleteAsync();

            var patientRelativeRepo = unitOfWork.GetRepository<PatientRelative, int>();
            await patientRelativeRepo.AddAsync(new PatientRelative 
            { 
                RelativeId = relativeId, 
                PatientId = patient.Id,
                EnrollmentDate = DateTime.UtcNow,
                CreatedBy = "System",
                CreatedOn = DateTime.UtcNow,
                LastModifiedBy = "System",
                LastModifiedOn = DateTime.UtcNow
            });
            await unitOfWork.CompleteAsync();

            var relativeEmailBody = $"Dear {relative.FName} {relative.LName},\n\nWe are pleased to inform you that your patient, {patient.FName} {patient.LName}, has been successfully registered and linked to your account on the Nesyan application.\n\nYou can now manage their medications, appointments, routines, and monitor their safety through the application.\n\nBest regards,\nThe Nesyan Team";
            var patientEmailBody = $"Dear {patient.FName} {patient.LName},\n\nWelcome to Nesyan! Your relative, {relative.FName} {relative.LName}, has successfully registered you on the Nesyan application.\n\nOur platform is designed to assist and support you and your family in managing daily tasks, medications, and maintaining safe routines.\n\nYour account details:\nUsername: {patient.UserName}\nEmail: {patient.Email}\n\nWarm regards,\nThe Nesyan Team";

            try
            {
                await emailService.SendEmailAsync(relative.Email, "Patient Registered Successfully - Nesyan", relativeEmailBody);
                await emailService.SendEmailAsync(patient.Email, "Welcome to Nesyan!", patientEmailBody);
            }
            catch
            {
                // Silence email sending failures so standard API still succeeds
            }

            return new AuthResponseDto 
            { 
                IsSuccess = true, 
                Message = "Patient registered and linked successfully.",
                Token = string.Empty,
                UserId = patient.Id,
                Email = patient.Email,
                Role = "Patient",
                Stage = patient.CurrentStage.ToString()
            };
        }
    }

}

