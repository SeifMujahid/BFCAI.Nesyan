
using BFCAI.Nesyan.Application.Abstraction.Services.Medications;
using BFCAI.Nesyan.Domain.Contracts;
using BFCAI.Nesyan.Domain.Entities.Medications;
using BFCAI.Nesyan.Domain.Entities.Primary.Doctors;
using BFCAI.Nesyan.Domain.Entities.Primary.Patients;
using AutoMapper;
using BFCAI.Nesyan.Application.Abstraction.Models.Routines;
using BFCAI.Nesyan.Application.Abstraction.Models.Reminders;

namespace BFCAI.Nesyan.Application.Services.Medications
{
    public class MedicationService(IUnitOfWork UnitOfWork, IMapper Mapper) : IMedicationService
    {
        public async Task<IEnumerable<RoutineToReturnDto>> GetPatientMedicationsAsync(int patientId)
        {
            var repo = UnitOfWork.GetRepository<Medication, int>();
            var all = await repo.GetAllAsync(false);
            var patientMeds = all.Where(x => x.PatientId == patientId).ToList();
            return Mapper.Map<IEnumerable<RoutineToReturnDto>>(patientMeds);
        }

        public async Task<RoutineToReturnDto> AddMedicationAsync(ReminderToCreateDto dto)
        {
            var patientRepo = UnitOfWork.GetRepository<Patient, int>();
            var doctorRepo = UnitOfWork.GetRepository<Doctor, int>();

            if (await patientRepo.Get(dto.PatientId) is null)
                throw new Exception("Patient not found");


            var med = Mapper.Map<Medication>(dto);
            med.CreatedOn = DateTime.UtcNow;
            med.CreatedBy = "System";
            med.LastModifiedOn = DateTime.UtcNow;
            med.LastModifiedBy = "System";

            var repo = UnitOfWork.GetRepository<Medication, int>();
            await repo.AddAsync(med);
            await UnitOfWork.CompleteAsync();

            return Mapper.Map<RoutineToReturnDto>(med);
        }

        public async Task DeleteMedicationAsync(int id)
        {
            var repo = UnitOfWork.GetRepository<Medication, int>();
            var med = await repo.Get(id);
            if (med is null) throw new Exception("Medication not found");

            repo.Delete(med);
            await UnitOfWork.CompleteAsync();
        }
    }
}
