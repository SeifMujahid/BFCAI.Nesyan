using AutoMapper;
using BFCAI.Nesyan.Application.Abstraction.Models.Doctors;
using BFCAI.Nesyan.Application.Abstraction.Models.Patients;
using BFCAI.Nesyan.Application.Abstraction.Services.Doctors;
using BFCAI.Nesyan.Domain.Contracts;
using BFCAI.Nesyan.Domain.Entities.Primary.Doctor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Services.Doctors
{
    public class DoctorService(IUnitOfWork UnitOfWork,IMapper Mapper) : IDoctorService
    {
        public async Task<IEnumerable<DoctorToReturnDto>> GetDoctorsAsync()
        {
            var doctors =await UnitOfWork.GetRepository<Doctor, int>().GetAllAsync();
            var doctorsToReturn = Mapper.Map<IEnumerable<DoctorToReturnDto>>(doctors);
            return doctorsToReturn;
        }
        public async Task<DoctorToReturnDto> GetDoctorAsync(int id)
        {
            var doctor =await UnitOfWork.GetRepository<Doctor, int>().Get(id);
            var doctorToReturn = Mapper.Map<DoctorToReturnDto>(doctor);
            return doctorToReturn;
        }
        public async Task<DoctorToReturnDto> CreateDoctorAsync(DoctorToCreateDto doctorToCreate)
        {
            var repo = UnitOfWork.GetRepository<Doctor, int>();
            
            // Uniqueness Validations
            var existingDoctors = await repo.GetAllAsync();
            if (existingDoctors.Any(d => d.NationalId == doctorToCreate.NationalId))
                throw new Exception("NationalId is already registered.");
            if (existingDoctors.Any(d => d.Email.Equals(doctorToCreate.Email, StringComparison.OrdinalIgnoreCase)))
                throw new Exception("Email is already registered.");
            if (existingDoctors.Any(d => d.UserName.Equals(doctorToCreate.UserName, StringComparison.OrdinalIgnoreCase)))
                throw new Exception("UserName is already taken.");

            var doctor = Mapper.Map<Doctor>(doctorToCreate);
            doctor.CreatedOn = DateTime.UtcNow;
            doctor.CreatedBy = doctor.UserName; // Or "System"
            doctor.LastModifiedOn = DateTime.UtcNow;
            doctor.LastModifiedBy = doctor.UserName;

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

        //public async Task<IEnumerable<PatientToReturnDto>> GetDoctorPatientsAsync(int doctorId)
        //{
            //var repo = UnitOfWork.GetRepository<Doctor, int>();
            // Since repo now has GetTableNoTracking, we can include relationships using Microsoft.EntityFrameworkCore
            // However, to keep repo patterns clean, we'll try a local query or rely on a specific repo query if Include is unavailable.
            // As DoctorService has no EF Core dependency natively at the top level, we might have to use UnitOfWork context directly or query patient repo.
            
            // To abide by Clean Architecture without adding EntityFrameworkCore to Application Layer:
            //var allDoctors = await repo.GetAllAsync(true);
            //var doctor = allDoctors.FirstOrDefault(d => d.Id == doctorId);
            
            //if (doctor == null) throw new Exception("Doctor not found");
            
            // If lazy loading is enabled, doctor.Patients is populated. If not, this might be null.
             //To be genuinely safe, I could query the TreatmentRequests for Accepted ones where DoctorId matches!
            //var treatmentRepo = UnitOfWork.GetRepository<BFCAI.Nesyan.Domain.Entities.Primary.TreatmentRequests.TreatmentRequest, int>();
            //var allRequests = await treatmentRepo.GetAllAsync(false);
            //var acceptedPatientIds = allRequests.Where(r => r.DoctorId == doctorId && r.Status == BFCAI.Nesyan.Domain.Entities.Primary.TreatmentRequests.RequestStatus.Accepted).Select(r => r.PatientId).Distinct().ToList();

            //var patientRepo = UnitOfWork.GetRepository<BFCAI.Nesyan.Domain.Entities.Primary.Patient.Patient, int>();
            //var allPatients = await patientRepo.GetAllAsync(false);
            
            //var patients = allPatients.Where(p => acceptedPatientIds.Contains(p.Id)).ToList();

            //return Mapper.Map<IEnumerable<PatientToReturnDto>>(patients);
        //}

        public async Task<DoctorStatisticsDto> GetDoctorStatisticsAsync(int doctorId)
        {
            //var trRepo = UnitOfWork.GetRepository<TreatmentRequest, int>();
            //var allTR = await trRepo.GetAllAsync(false);
            
            //var totalPatients = allTR.Count(tr => tr.DoctorId == doctorId && tr.Status == RequestStatus.Accepted);
            //var pendingRequests = allTR.Count(tr => tr.DoctorId == doctorId && tr.Status == RequestStatus.Pending);

            return new DoctorStatisticsDto
            {
                //TotalPatients = totalPatients,
                //PendingRequests = pendingRequests
            };
        }
    }
}
