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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Services.Doctors
{
    public class DoctorService(IUnitOfWork UnitOfWork, IMapper Mapper) : IDoctorService
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


            var treatmentRepo = UnitOfWork.GetRepository<RelativeDoctorRequest, int>();
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
            var trRepo = UnitOfWork.GetRepository<RelativeDoctorRequest, int>();
            var allTR = await trRepo.GetAllAsync();

            var totalPatients = allTR.Count(tr => tr.DoctorId == doctorId && tr.Status == RequestStatus.Accepted);
            var pendingRequests = allTR.Count(tr => tr.DoctorId == doctorId && tr.Status == RequestStatus.Pending);

            return new DoctorStatisticsDto
            {
                TotalPatients = totalPatients,
                PendingRequests = pendingRequests
            };
        }

    }
}
