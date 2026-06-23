using AutoMapper;
using BFCAI.Nesyan.Application.Abstraction.Models.Caregivers;
using BFCAI.Nesyan.Application.Abstraction.Models.Doctors;
using BFCAI.Nesyan.Application.Abstraction.Models.TreatmentRequests;
using BFCAI.Nesyan.Application.Abstraction.Services.TreatmentRequests;
using BFCAI.Nesyan.Application.Common.Exceptions;
using BFCAI.Nesyan.Domain.Contracts;
using BFCAI.Nesyan.Domain.Entities.Primary.Caregivers;
using BFCAI.Nesyan.Domain.Entities.Primary.Doctors;
using BFCAI.Nesyan.Domain.Entities.Primary.Patients;
using BFCAI.Nesyan.Domain.Entities.Primary.Relatives;
using BFCAI.Nesyan.Domain.Entities.Relations.Primary;
using BFCAI.Nesyan.Domain.Specifications.Caregivers;
using BFCAI.Nesyan.Domain.Specifications.Doctors;
using BFCAI.Nesyan.Domain.Specifications.PatientRelatives;
using BFCAI.Nesyan.Domain.Specifications.RequestTreatment;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BFCAI.Nesyan.Application.Services.TreatmentRequests
{
    public class TreatmentRequestService(IUnitOfWork UnitOfWork, IMapper Mapper) : ITreatmentRequestService
    {
        public async Task<IEnumerable<DoctorSummaryDto>> SearchDoctors(string name)
        {
            var spec = new DoctorSearchByNameSpecs(name);
            var doctors = await UnitOfWork.GetRepository<Doctor, int>().GetAllWithSpecAsync(spec);
            return Mapper.Map<IEnumerable<DoctorSummaryDto>>(doctors);
        }
        public async Task<IEnumerable<CaregiverSummaryDto>> SearchCaregiver(string name)
        {
            var spec = new CaregiverSearchByNameSpecs(name);
            var doctors = await UnitOfWork.GetRepository<Caregiver, int>().GetAllWithSpecAsync(spec);
            return Mapper.Map<IEnumerable<CaregiverSummaryDto>>(doctors);
        }
        public async Task RealtiveCreateTreatmentRequestAsync(TreatmentRequestToCreateDto dto,int actorType)
        {
            if (dto.CaregiverId == null && dto.DoctorId == null)
                throw new BadRequestException("DoctorId or CaregiverId is required");
            if (dto.CaregiverId!=null&&dto.DoctorId!=null)
                throw new BadRequestException("You Cannot add Doctor and Caregiver at the same request");
            if(actorType==1 && dto.DoctorId == null)
                throw new BadRequestException("DoctorId is required");
            if (actorType != 1 && dto.CaregiverId == null)
                throw new BadRequestException("CaregiverId is required");
            var repo = UnitOfWork.GetRepository<TreatmentRequest, int>();
            var specs = new RelativePatientCheckSpecifications(dto.RelativeId, dto.PatientId);
            var relativePatient = await UnitOfWork.GetRepository<PatientRelative, int>().GetWithSpecAsync(specs);
            if (relativePatient == null)
                throw new NotFoundException(nameof(relativePatient), new { dto.RelativeId, dto.PatientId });
            if (actorType == 1)
            {
                var doctor = await UnitOfWork.GetRepository<Doctor, int>().Get(dto.DoctorId ?? 0);
                if (doctor == null)
                    throw new NotFoundException(nameof(Doctor), dto.DoctorId??0);
            }
            else
            {
                var caregiver = await UnitOfWork.GetRepository<Caregiver, int>().Get(dto.CaregiverId ?? 0);
                if (caregiver == null)
                    throw new NotFoundException(nameof(caregiver), dto.CaregiverId??0);
            }
            var request = Mapper.Map<TreatmentRequest>(dto);
            await repo.AddAsync(request);
            await UnitOfWork.CompleteAsync();
        }
        public async Task<IEnumerable<TreatmentRequestToReturnDto>> GetDoctorOrCaregiverRequestsAsync(int actorId,int actorType, int orderType = 0)
        {
            var specs = new DoctorOrCaregiverTreatmentRequestsSpecifications(actorId, orderType, actorType);
            var requests =await UnitOfWork.GetRepository<TreatmentRequest, int>().GetAllWithSpecAsync(specs);
            if (requests == null)
                throw new NotFoundException(nameof(requests), actorId);
            var requestToReturn = Mapper.Map<IEnumerable<TreatmentRequestToReturnDto>>(requests);
            return requestToReturn;
        }
        public async Task<IEnumerable<TreatmentRequestToReturnDto>> GetRelativeRequestsAsync(int relativeId, int actorType,int orderType=0)
        {
            var specs = new RelativeTreatmentRequestSpecifications(relativeId, actorType,orderType);
            var requests = await UnitOfWork.GetRepository<TreatmentRequest, int>().GetAllWithSpecAsync(specs);
            if (requests == null)
                throw new NotFoundException(nameof(requests), relativeId);
            var requestToReturn = Mapper.Map<IEnumerable<TreatmentRequestToReturnDto>>(requests);
            return requestToReturn;
        }
        public async Task<IEnumerable<TreatmentRequestToReturnDto>> GetPatientRequestsAsync(int relativeId, int actorType, int orderType = 0)
        {
            var specs = new PatientTreatmentRequestSpecifications(relativeId, actorType, orderType);
            var requests = await UnitOfWork.GetRepository<TreatmentRequest, int>().GetAllWithSpecAsync(specs);
            if (requests == null)
                throw new NotFoundException(nameof(requests), relativeId);
            var requestToReturn = Mapper.Map<IEnumerable<TreatmentRequestToReturnDto>>(requests);
            return requestToReturn;
        }
        public async Task DoctorOrCaregiverAcceptRequestAsync(int requestId, int actorType, int actorId)
        {
            var repo = UnitOfWork.GetRepository<TreatmentRequest, int>();
            var request = await repo.Get(requestId);
            if (request == null)
                throw new NotFoundException(nameof(request), requestId);
            if (actorType == 1 ?request.DoctorId != actorId: request.CaregiverId != actorId)
                throw new UnAuthourizedException("This request does not belong to this Actor");
            if (request.Status == RequestStatus.Pending)
            {
                request.Status = RequestStatus.Accepted;
                repo.Update(request);
                await UnitOfWork.CompleteAsync();
            }
            else
                throw new BadRequestException($"Request cannot be accepted because its current status is {request.Status}");
        }
        public async Task RelativeSelectDoctorOrCaregiverAsync(int requestId, int relativeId,int actorType)
        {
            var repo = UnitOfWork.GetRepository<TreatmentRequest, int>();
            var request = await repo.Get(requestId);
            
            if (request == null)
                throw new NotFoundException(nameof(request), requestId);
            
            if (request.RelativeId != relativeId)
                throw new UnAuthourizedException("This request does not belong to this Relative");
           
            var checkPatintStatusSpec = new CheckPatintStatusSpecifications(request.PatientId, actorType);
            var checkPatintStatus = await repo.GetAllWithSpecAsync(checkPatintStatusSpec);

            if (checkPatintStatus.Any())
                throw new BadRequestException($"This Patient Already Has A {(actorType == 1 ? nameof(Doctor) : nameof(Caregiver))}");

            if (request.Status != RequestStatus.Accepted)
                throw new BadRequestException($"{(actorType == 1 ? nameof(Doctor) : nameof(Caregiver))} Should Accept Request First");

            var patientRepo =UnitOfWork.GetRepository<Patient, int>();
            var patient=await patientRepo.Get(request.PatientId);

            if (patient == null)
                throw new NotFoundException(nameof(patient), request.PatientId);

            if (actorType == 1)
                patient.DoctorId = request.DoctorId;
            else
                patient.CaregiverId = request.CaregiverId;
            patientRepo.Update(patient);
            request.Status = RequestStatus.Selected;
            repo.Update(request);
            await UnitOfWork.CompleteAsync();
        }
        public async Task DoctorRejectRequestAsync(int requestId,int actorId, int actorType)
        {
            var repo = UnitOfWork.GetRepository<TreatmentRequest, int>();
            var request = await repo.Get(requestId);
            if (request == null)
                throw new NotFoundException(nameof(request), requestId);
            if ((actorType == 1 && request.DoctorId != actorId) ||
                   (actorType == 2 && request.CaregiverId != actorId))
                throw new UnAuthourizedException($"This request does not belong to this {(actorType == 1 ? nameof(Doctor) : nameof(Caregiver))}");
            if (request.Status == RequestStatus.RemovalPending)
                throw new BadRequestException("A removal request is already pending. It will be automatically processed if no response is received within 7 days of submitting.");
            if (request.Status == RequestStatus.Selected)
                {
                request.Status =RequestStatus.RemovalPending;
                request.RequestDate =DateTime.UtcNow;
                repo.Update(request);
                await UnitOfWork.CompleteAsync();
                return;
                }

            request.Status = RequestStatus.Rejected;
            repo.Update(request);
            await UnitOfWork.CompleteAsync();
        }
        public async Task RelativeRejectRequestAsync(int requestId, int relativeId, int actorType)
        {
            var repo = UnitOfWork.GetRepository<TreatmentRequest, int>();
            var request = await repo.Get(requestId);
            if (request == null)
                throw new NotFoundException(nameof(request), requestId);
            if (request.RelativeId != relativeId)
                throw new UnAuthourizedException("This request does not belong to this Relative");
            if (request.Status == RequestStatus.Selected|| request.Status == RequestStatus.RemovalPending)
            {
                var patientRepo = UnitOfWork.GetRepository<Patient, int>();
                var patient = await patientRepo.Get(request.PatientId);

                if (patient == null)
                    throw new NotFoundException(nameof(patient), request.PatientId);
                if(actorType==1)
                    patient.DoctorId = null;
                else
                    patient.CaregiverId = null;
                patientRepo.Update(patient);
            }
            request.Status = RequestStatus.Rejected;
            repo.Update(request);
            await UnitOfWork.CompleteAsync();
        }

    }
}
