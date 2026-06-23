using AutoMapper;
using BFCAI.Nesyan.Application.Abstraction.Models._Relations.DoctorPatient;
using BFCAI.Nesyan.Application.Abstraction.Models.Patients;
using BFCAI.Nesyan.Application.Abstraction.Services._Relations;
using BFCAI.Nesyan.Application.Common.Exceptions;
using BFCAI.Nesyan.Domain.Contracts;
using BFCAI.Nesyan.Domain.Entities.Primary.Doctors;
using BFCAI.Nesyan.Domain.Entities.Primary.Patients;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Services._Reltaions.DoctorPatient
{
    public class DoctorPatientService(IUnitOfWork unitOfWork, IMapper mapper) : IDoctorPatientService
    {
        public async Task AddPatientToDoctorAsync(DoctorPatientAddDto dto)
        {
            var doctor = await unitOfWork.GetRepository<Doctor, int>()
                .GetTableNoTracking()
                .FirstOrDefaultAsync(d => d.NationalId == dto.NationalIdDoctor && d.Email == dto.EmailDoctor);

            if (doctor == null)
                throw new BadRequestException("Doctor data incorrect");

            var patientRepo = unitOfWork.GetRepository<Patient, int>();
            var patient = await patientRepo.Get(dto.PatientId);

            if (patient == null)
                throw new NotFoundException(nameof(Patient), dto.PatientId);

            patient.DoctorId = doctor.Id;
            patientRepo.Update(patient);
            await unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<PatientSummaryDto>> GetDoctorPatientsAsync(int doctorId)
        {
            var doctor = await unitOfWork.GetRepository<Doctor, int>().Get(doctorId);
            if (doctor == null)
                throw new NotFoundException(nameof(Doctor), doctorId);

            var patients = await unitOfWork.GetRepository<Patient, int>()
                .GetTableNoTracking()
                .Where(p => p.DoctorId == doctorId)
                .ToListAsync();

            return mapper.Map<IEnumerable<PatientSummaryDto>>(patients);
        }

        public async Task<PatientDoctorDto> GetPatientDoctorAsync(int patientId)
        {
            var patient = await unitOfWork.GetRepository<Patient, int>()
                .GetTableNoTracking()
                .Include(p => p.Doctor)
                .FirstOrDefaultAsync(p => p.Id == patientId);

            if (patient == null)
                throw new NotFoundException(nameof(Patient), patientId);

            if (patient.Doctor == null)
                throw new NotFoundException("Doctor for patient", patientId);

            return mapper.Map<PatientDoctorDto>(patient.Doctor);
        }

        public async Task UpdatePatientDoctorAsync(int patientId, int doctorId)
        {
            var doctor = await unitOfWork.GetRepository<Doctor, int>().Get(doctorId);
            if (doctor == null)
                throw new NotFoundException(nameof(Doctor), doctorId);

            var patientRepo = unitOfWork.GetRepository<Patient, int>();
            var patient = await patientRepo.Get(patientId);

            if (patient == null)
                throw new NotFoundException(nameof(Patient), patientId);

            patient.DoctorId = doctorId;
            patientRepo.Update(patient);
            await unitOfWork.CompleteAsync();
        }

        public async Task RemovePatientFromDoctorAsync(int doctorId, int patientId)
        {
            var patientRepo = unitOfWork.GetRepository<Patient, int>();
            var patient = await patientRepo.Get(patientId);

            if (patient == null)
                throw new NotFoundException(nameof(Patient), patientId);

            if (patient.DoctorId != doctorId)
                throw new BadRequestException("Patient is not assigned to this doctor");

            patient.DoctorId = null;
            patientRepo.Update(patient);
            await unitOfWork.CompleteAsync();
        }
    }
}
