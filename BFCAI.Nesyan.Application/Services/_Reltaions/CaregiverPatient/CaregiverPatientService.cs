using AutoMapper;
using BFCAI.Nesyan.Application.Abstraction.Models._Relations.CaregiverPatient;
using BFCAI.Nesyan.Application.Abstraction.Models.Patients;
using BFCAI.Nesyan.Application.Abstraction.Services._Relations;
using BFCAI.Nesyan.Application.Common.Exceptions;
using BFCAI.Nesyan.Domain.Contracts;
using BFCAI.Nesyan.Domain.Entities.Primary.Caregivers;
using BFCAI.Nesyan.Domain.Entities.Primary.Patients;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Services._Reltaions.CaregiverPatient
{
    public class CaregiverPatientService(IUnitOfWork unitOfWork, IMapper mapper) : ICaregiverPatientService
    {
        public async Task AddPatientToCaregiverAsync(CaregiverPatientAddDto dto)
        {
            var caregiver = await unitOfWork.GetRepository<Caregiver, int>()
                .GetTableNoTracking()
                .FirstOrDefaultAsync(c => c.NationalId == dto.NationalIdcaregavier && c.Email == dto.Emailcaregavier);

            if (caregiver == null)
                throw new BadRequestException("Caregiver data incorrect");

            var patientRepo = unitOfWork.GetRepository<Patient, int>();
            var patient = await patientRepo.Get(dto.PatientId);

            if (patient == null)
                throw new NotFoundException(nameof(Patient), dto.PatientId);

            patient.CaregiverId = caregiver.Id;
            patientRepo.Update(patient);
            await unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<PatientSummaryDto>> GetCaregiverPatientsAsync(int caregiverId)
        {
            var caregiver = await unitOfWork.GetRepository<Caregiver, int>().Get(caregiverId);
            if (caregiver == null)
                throw new NotFoundException(nameof(Caregiver), caregiverId);

            var patients = await unitOfWork.GetRepository<Patient, int>()
                .GetTableNoTracking()
                .Where(p => p.CaregiverId == caregiverId)
                .ToListAsync();

            return mapper.Map<IEnumerable<PatientSummaryDto>>(patients);
        }

        public async Task<PatientCaregiverDto> GetPatientCaregiverAsync(int patientId)
        {
            var patient = await unitOfWork.GetRepository<Patient, int>()
                .GetTableNoTracking()
                .Include(p => p.Caregiver)
                .FirstOrDefaultAsync(p => p.Id == patientId);

            if (patient == null)
                throw new NotFoundException(nameof(Patient), patientId);

            if (patient.Caregiver == null)
                throw new NotFoundException("Caregiver for patient", patientId);

            return mapper.Map<PatientCaregiverDto>(patient.Caregiver);
        }

        public async Task UpdatePatientCaregiverAsync(int patientId, int caregiverId)
        {
            var caregiver = await unitOfWork.GetRepository<Caregiver, int>().Get(caregiverId);
            if (caregiver == null)
                throw new NotFoundException(nameof(Caregiver), caregiverId);

            var patientRepo = unitOfWork.GetRepository<Patient, int>();
            var patient = await patientRepo.Get(patientId);

            if (patient == null)
                throw new NotFoundException(nameof(Patient), patientId);

            patient.CaregiverId = caregiverId;
            patientRepo.Update(patient);
            await unitOfWork.CompleteAsync();
        }

        public async Task RemovePatientFromCaregiverAsync(int caregiverId, int patientId)
        {
            var patientRepo = unitOfWork.GetRepository<Patient, int>();
            var patient = await patientRepo.Get(patientId);

            if (patient == null)
                throw new NotFoundException(nameof(Patient), patientId);

            if (patient.CaregiverId != caregiverId)
                throw new BadRequestException("Patient is not assigned to this caregiver");

            patient.CaregiverId = null;
            patientRepo.Update(patient);
            await unitOfWork.CompleteAsync();
        }
    }
}
