//using AutoMapper;
//using BFCAI.Nesyan.Application.Abstraction.Models.Medications;
//using BFCAI.Nesyan.Application.Abstraction.Services.Medications;
//using BFCAI.Nesyan.Domain.Contracts;
//using BFCAI.Nesyan.Domain.Entities.Primary.Medications;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace BFCAI.Nesyan.Application.Services.Medications
//{
//    public class MedicationService(IUnitOfWork UnitOfWork, IMapper Mapper) : IMedicationService
//    {
//        public async Task<IEnumerable<MedicationToReturnDto>> GetPatientMedicationsAsync(int patientId)
//        {
//            var repo = UnitOfWork.GetRepository<Medication, int>();
//            var allMeds = await repo.GetAllAsync(false);
//            var patientMeds = allMeds.Where(m => m.PatientId == patientId).ToList();
//            return Mapper.Map<IEnumerable<MedicationToReturnDto>>(patientMeds);
//        }

//        public async Task<MedicationToReturnDto> AddMedicationAsync(MedicationToCreateDto dto)
//        {
//            var med = Mapper.Map<Medication>(dto);
//            med.CreatedOn = DateTime.UtcNow;
//            med.CreatedBy = "System";
//            med.LastModifiedOn = DateTime.UtcNow;
//            med.LastModifiedBy = "System";

//            var repo = UnitOfWork.GetRepository<Medication, int>();
//            await repo.AddAsync(med);
//            await UnitOfWork.CompleteAsync();

//            return Mapper.Map<MedicationToReturnDto>(med);
//        }

//        public async Task DeleteMedicationAsync(int id)
//        {
//            var repo = UnitOfWork.GetRepository<Medication, int>();
//            var med = await repo.Get(id);
//            if (med == null) throw new Exception("Medication not found");

//            repo.Delete(med);
//            await UnitOfWork.CompleteAsync();
//        }
//    }
//}
