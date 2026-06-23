using AutoMapper;
using BFCAI.Nesyan.Application.Abstraction.Models.Caregivers;
using BFCAI.Nesyan.Application.Abstraction.Models.Patients;
using BFCAI.Nesyan.Application.Abstraction.Services.Caregivers;
using BFCAI.Nesyan.Application.Common.Exceptions;
using BFCAI.Nesyan.Domain.Contracts;
using BFCAI.Nesyan.Domain.Entities.Primary.Caregivers;
using BFCAI.Nesyan.Domain.Entities.Primary.Patients;
using BFCAI.Nesyan.Domain.Specifications.Caregivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace BFCAI.Nesyan.Application.Services.Caregivers
{
    public class CaregiverService(IUnitOfWork UnitOfWork, IMapper Mapper) : ICaregiverService
    {
        public async Task<IEnumerable<CaregiverSummaryDto>> GetCaregiversAsync()
        {
            var caregivers = await UnitOfWork.GetRepository<Caregiver, int>().GetAllAsync();
            return Mapper.Map<IEnumerable<CaregiverSummaryDto>>(caregivers);
        }

        public async Task<CaregiverPatientsDto> GetCaregiverAsync(int id)
        {
            var specs = new CaregiverGetPatientsSpecifications(id);
            var caregiver = await UnitOfWork.GetRepository<Caregiver, int>().GetWithSpecAsync(specs);

            if (caregiver is null) 
                throw new NotFoundException(nameof(caregiver),id);
            var caregivePatients = new CaregiverPatientsDto
            {
                CaregiverSummary = Mapper.Map<CaregiverSummaryDto>(caregiver),
                Patients = Mapper.Map<IEnumerable<PatientSummaryDto>>(caregiver.Patients)
            };
            return caregivePatients;
        }
        public async Task <CaregiverPatientHomeDto>GetPatientHome(int caregiverId,int patientId)
        {
            var specs =new CaregiverMainSpecs( caregiverId,  patientId);
            var caregiverpatient=await UnitOfWork.GetRepository<Patient,int>().GetWithSpecAsync(specs);
            if (caregiverpatient is null)
                throw new NotFoundException(nameof(caregiverpatient), new{ caregiverId, patientId });
            var caregiverPatientHome = new CaregiverPatientHomeDto
            {
                CaregiverSummary = Mapper.Map<CaregiverSummaryDto>(caregiverpatient.Caregiver),
                Patient = Mapper.Map<PatientHomeDto>(caregiverpatient)
            };
            return caregiverPatientHome;

        }
        public async Task<CaregiverPatientRemindersDto>GetPatientReminders(int caregiverId, int patientId,int reminderType)
        {
            var specs =new CaregiverMainSpecs(caregiverId,patientId, reminderType);

            var patient =await UnitOfWork.GetRepository<Patient, int>().GetWithSpecAsync(specs);

            if (patient is null)
                throw new NotFoundException(nameof(Patient),new{caregiverId,patientId});


            CaregiverPatientRemindersDto caregiverPatientReminders;

            switch (reminderType)
            {

                case 1:
                    caregiverPatientReminders =new CaregiverPatientRemindersDto
                        {
                            PatientMedications =
                                Mapper.Map<PatientMedicationsDto>(patient),

                            PatientAppointments =null,

                            PatientRoutines =null
                        };

                    break;


                case 2:

                    caregiverPatientReminders =new CaregiverPatientRemindersDto
                        {
                            PatientMedications =null,

                            PatientAppointments =
                                Mapper.Map<PatientAppointmentsDto>(patient),

                            PatientRoutines =null
                        };

                    break;


                case 3:

                    caregiverPatientReminders =new CaregiverPatientRemindersDto
                        {
                            PatientMedications =null,

                            PatientAppointments =null,

                            PatientRoutines =
                            Mapper.Map<PatientRoutineDto>(patient)
                        };

                    break;

                default:
                    throw new Exception("Invalid reminder type");
            }

            return caregiverPatientReminders;
        }
        public async Task<CaregiverToReturnDto> CreateCaregiverAsync(CaregiverToCreateDto caregiverToCreate)
        {
            var repo = UnitOfWork.GetRepository<Caregiver, int>();

            var existingCaregivers = await repo.GetAllAsync();
            if (existingCaregivers.Any(d => d.NationalId == caregiverToCreate.NationalId))
                throw new Exception("NationalId is already registered.");
            if (existingCaregivers.Any(d => d.Email.Equals(caregiverToCreate.Email, StringComparison.OrdinalIgnoreCase)))
                throw new Exception("Email is already registered.");
            if (existingCaregivers.Any(d => d.UserName.Equals(caregiverToCreate.UserName, StringComparison.OrdinalIgnoreCase)))
                throw new Exception("UserName is already taken.");

            var caregiver = Mapper.Map<Caregiver>(caregiverToCreate);
            caregiver.Password = BCrypt.Net.BCrypt.HashPassword(caregiver.Password);
            caregiver.CreatedOn = DateTime.UtcNow;
            caregiver.CreatedBy = caregiver.UserName;
            caregiver.LastModifiedOn = DateTime.UtcNow;
            caregiver.LastModifiedBy = caregiver.UserName;

            await repo.AddAsync(caregiver);
            await UnitOfWork.CompleteAsync();

            return Mapper.Map<CaregiverToReturnDto>(caregiver);
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

        public async Task UpdateCaregiverAsync(CaregiverToReturnDto caregiverToUpdate)
        {
            var repo = UnitOfWork.GetRepository<Caregiver, int>();
            var caregiver = await repo.Get(caregiverToUpdate.Id);
            if (caregiver is null) throw new Exception("Caregiver not found");

            Mapper.Map(caregiverToUpdate, caregiver);

            if (caregiverToUpdate.Image != null)
            {
                caregiver.ImageUrl = SaveFile(caregiverToUpdate.Image, "caregivers/avatars");
            }

            caregiver.LastModifiedOn = DateTime.UtcNow;
            caregiver.LastModifiedBy = caregiver.UserName;

            repo.Update(caregiver);
            await UnitOfWork.CompleteAsync();
        }

        public async Task DeleteCaregiverAsync(int id)
        {
            var repo = UnitOfWork.GetRepository<Caregiver, int>();
            var caregiver = await repo.Get(id);
            if (caregiver is null) throw new Exception("Caregiver not found");

            repo.Delete(caregiver);
            await UnitOfWork.CompleteAsync();
        }

        public async Task<CaregiverProfileDto> GetCaregiverProfileAsync(int id)
        {
            var specs = new CaregiverGetPatientsSpecifications(id);
            var caregiver = await UnitOfWork.GetRepository<Caregiver, int>().GetWithSpecAsync(specs);
            if (caregiver is null)
                throw new NotFoundException(nameof(caregiver), id);
            return Mapper.Map<CaregiverProfileDto>(caregiver);
        }
    }
}
