//using AutoMapper;
//using BFCAI.Nesyan.Application.Abstraction.Models.TreatmentRequests;
//using BFCAI.Nesyan.Application.Abstraction.Services.TreatmentRequests;
//using BFCAI.Nesyan.Domain.Contracts;
//using BFCAI.Nesyan.Domain.Entities.Primary.TreatmentRequests;
//using BFCAI.Nesyan.Domain.Entities.Primary.Doctor;
//using BFCAI.Nesyan.Domain.Entities.Primary.Patient;

//namespace BFCAI.Nesyan.Application.Services.TreatmentRequests
//{
//    public class TreatmentRequestService(IUnitOfWork UnitOfWork, IMapper Mapper) : ITreatmentRequestService
//    {
//        public async Task<TreatmentRequestToReturnDto> CreateRequestAsync(TreatmentRequestToCreateDto dto)
//        {
//            var request = Mapper.Map<TreatmentRequest>(dto);
//            request.Status = RequestStatus.Pending;
//            request.CreatedOn = DateTime.UtcNow;
//            request.CreatedBy = "System"; 
//            request.LastModifiedOn = DateTime.UtcNow;
//            request.LastModifiedBy = "System";

//            var repo = UnitOfWork.GetRepository<TreatmentRequest, int>();
//            await repo.AddAsync(request);
//            await UnitOfWork.CompleteAsync();

//            return await GetMappedRequestDto(request.Id);
//        }

//        public async Task<IEnumerable<TreatmentRequestToReturnDto>> GetDoctorPendingRequestsAsync(int doctorId)
//        {
//            var repo = UnitOfWork.GetRepository<TreatmentRequest, int>();
//            // Since there's no generic Include out of the box in the snippet we saw, we might just map directly or fetch related explicitly if needed.
//            // For now, assuming GetAll returns standard query or using straightforward mapping
//            var requests = await repo.GetAllAsync();
//            var pendingRequests = requests.Where(r => r.DoctorId == doctorId && r.Status == RequestStatus.Pending).ToList();

//            var result = new List<TreatmentRequestToReturnDto>();
//            foreach (var req in pendingRequests)
//            {
//                result.Add(await GetMappedRequestDto(req.Id));
//            }
//            return result;
//        }

//        public async Task AcceptRequestAsync(int requestId)
//        {
//            var repo = UnitOfWork.GetRepository<TreatmentRequest, int>();
//            var request = await repo.Get(requestId);
//            if (request == null) throw new Exception("Request not found");

//            request.Status = RequestStatus.Accepted;
//            request.LastModifiedOn = DateTime.UtcNow;
//            request.LastModifiedBy = "System";
//            repo.Update(request);

//            // Fetch Doctor and Patient to bind them
//            var doctorRepo = UnitOfWork.GetRepository<Doctor, int>();
//            var patientRepo = UnitOfWork.GetRepository<Patient, int>();

//            var doctor = await doctorRepo.Get(request.DoctorId);
//            var patient = await patientRepo.Get(request.PatientId);

//            if (doctor != null && patient != null)
//            {
//                // This assumes EF tracks the collection. If lazy loading/includes are missing, 
//                // we might need to manually ensure collection is initialized.
//                if (doctor.Patients == null) doctor.Patients = new List<Patient>();
//                doctor.Patients.Add(patient);
//            }

//            await UnitOfWork.CompleteAsync();
//        }

//        public async Task RejectRequestAsync(int requestId)
//        {
//            var repo = UnitOfWork.GetRepository<TreatmentRequest, int>();
//            var request = await repo.Get(requestId);
//            if (request == null) throw new Exception("Request not found");

//            request.Status = RequestStatus.Rejected;
//            request.LastModifiedOn = DateTime.UtcNow;
//            request.LastModifiedBy = "System";
//            repo.Update(request);
//            await UnitOfWork.CompleteAsync();
//        }

//        private async Task<TreatmentRequestToReturnDto> GetMappedRequestDto(int id)
//        {
//            var request = await UnitOfWork.GetRepository<TreatmentRequest, int>().Get(id);
//            var patient = await UnitOfWork.GetRepository<Patient, int>().Get(request.PatientId);
            
//            var dto = Mapper.Map<TreatmentRequestToReturnDto>(request);
//            if (patient != null)
//            {
//                dto.PatientName = $"{patient.FName} {patient.LName}";
//                dto.PatientAge = patient.Age;
//            }
//            return dto;
//        }
//    }
//}
