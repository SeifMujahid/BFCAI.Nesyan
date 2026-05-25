using AutoMapper;
using BFCAI.Nesyan.Application.Abstraction.Models.TreatmentRequests;
using BFCAI.Nesyan.Application.Abstraction.Services.TreatmentRequests;
using BFCAI.Nesyan.Application.Common.Exceptions;
using BFCAI.Nesyan.Domain.Contracts;
using BFCAI.Nesyan.Domain.Entities.Primary.Doctors;
using BFCAI.Nesyan.Domain.Entities.Primary.Patients;
using BFCAI.Nesyan.Domain.Entities.Primary.Relatives;
using BFCAI.Nesyan.Domain.Entities.Relations.Primary;
using BFCAI.Nesyan.Domain.Specifications.PatientRelatives;
using BFCAI.Nesyan.Domain.Specifications.RequestTreatment;
using Microsoft.AspNetCore.Http.HttpResults;

namespace BFCAI.Nesyan.Application.Services.TreatmentRequests
{
    public class TreatmentRequestService(IUnitOfWork UnitOfWork, IMapper Mapper) : ITreatmentRequestService
    {
        public async Task RealtiveCreateRequestAsync(TreatmentRequestToCreateDto dto)
        {
            var repo = UnitOfWork.GetRepository<RelativeDoctorRequest, int>();
            var checkPatintStatusSpec = new CheckPatintStatusSpecifications(dto.PatientId);
            var checkPatintStatus = await repo.GetAllWithSpecAsync(checkPatintStatusSpec);
            if (checkPatintStatus.Any())
                throw new BadRequestException("This Patient Already Have An Doctor");
            var specs = new RelativePatientCheckSpecifications(dto.RelativeId, dto.PatientId);
            var relativePatient = await UnitOfWork.GetRepository<PatientRelative, int>().GetWithSpecAsync(specs);
            if (relativePatient == null)
                throw new NotFoundException(nameof(relativePatient), new { dto.RelativeId, dto.PatientId });
            var doctor = await UnitOfWork.GetRepository<Doctor, int>().Get(dto.DoctorId);
            if (doctor == null)
                throw new NotFoundException(nameof(doctor), dto.DoctorId);
            var request = Mapper.Map<RelativeDoctorRequest>(dto);
            await repo.AddAsync(request);
            await UnitOfWork.CompleteAsync();
        }
        public async Task<IEnumerable<TreatmentRequestToReturnDto>> GetDoctorPendingRequestsAsync(int doctorId,int orderType=0)
        {
            var specs = new DoctorTreatmentRequestsSpecifications(doctorId, orderType);
            var requests =await UnitOfWork.GetRepository<RelativeDoctorRequest, int>().GetAllWithSpecAsync(specs);
            if (requests == null)
                throw new NotFoundException(nameof(requests), doctorId);
            var requestToReturn = Mapper.Map<IEnumerable<TreatmentRequestToReturnDto>>(requests);
            return requestToReturn;
        }
        public async Task<IEnumerable<TreatmentRequestToReturnDto>> GetRelativePendingRequestsAsync(int relativeId,int orderType=0)
        {
            var specs = new RelativeTreatmentRequestSpecifications(relativeId,orderType);
            var requests = await UnitOfWork.GetRepository<RelativeDoctorRequest, int>().GetAllWithSpecAsync(specs);
            if (requests == null)
                throw new NotFoundException(nameof(requests), relativeId);
            var requestToReturn = Mapper.Map<IEnumerable<TreatmentRequestToReturnDto>>(requests);
            return requestToReturn;
        }

        public async Task DoctorAcceptRequestAsync(int requestId,int doctorId)
        {
            var repo = UnitOfWork.GetRepository<RelativeDoctorRequest, int>();
            var request = await repo.Get(requestId);
            if (request == null)
                throw new NotFoundException(nameof(request), requestId);
            if (request.DoctorId != doctorId)
                throw new UnAuthourizedException("This request does not belong to this Doctor");
            if (request.Status == RequestStatus.Rejected)
                throw new BadRequestException("This request is already rejected");
            if (request.Status == RequestStatus.Pending)
            {
                request.Status = RequestStatus.Accepted;
                repo.Update(request);
                await UnitOfWork.CompleteAsync();
            }
            else
                throw new BadRequestException("This request is Selected or already accepted.");
        }

        public async Task RelativeSelectDoctorAsync(int requestId, int relativeId)
        {
            var repo = UnitOfWork.GetRepository<RelativeDoctorRequest, int>();
            var request = await repo.Get(requestId);
            
            if (request == null)
                throw new NotFoundException(nameof(request), requestId);
            
            if (request.RelativeId != relativeId)
                throw new UnAuthourizedException("This request does not belong to this Relative");
           
            var checkPatintStatusSpec = new CheckPatintStatusSpecifications(request.PatientId);
            var checkPatintStatus = await repo.GetAllWithSpecAsync(checkPatintStatusSpec);

            if (checkPatintStatus.Any())
                throw new BadRequestException("This Patient Already Have An Doctor");

            if (request.Status != RequestStatus.Accepted)
                throw new BadRequestException("Doctor Should Accept Request First");

            var patientRepo =UnitOfWork.GetRepository<Patient, int>();
            var patient=await patientRepo.Get(request.PatientId);

            if (patient == null)
                throw new NotFoundException(nameof(patient), request.PatientId);

            patient.DoctorId = request.DoctorId;
            patientRepo.Update(patient);
            request.Status = RequestStatus.Selected;
            repo.Update(request);
            await UnitOfWork.CompleteAsync();
        }
        public async Task DoctorRejectRequestAsync(int requestId,int doctorId)
        {
            var repo = UnitOfWork.GetRepository<RelativeDoctorRequest, int>();
            var request = await repo.Get(requestId);
            if (request == null)
                throw new NotFoundException(nameof(request), requestId);
            if (request.DoctorId != doctorId)
                throw new UnAuthourizedException("This request does not belong to this Doctor");
            if (request.Status == RequestStatus.Selected)
                {
                request.Status =RequestStatus.DoctorRemovalPending;
                request.RequestDate =DateTime.UtcNow;
                repo.Update(request);
                await UnitOfWork.CompleteAsync();
                return;
                }

            request.Status = RequestStatus.Rejected;
            repo.Update(request);
            await UnitOfWork.CompleteAsync();
        }
        public async Task RelativeRejectRequestAsync(int requestId, int relativeId)
        {
            var repo = UnitOfWork.GetRepository<RelativeDoctorRequest, int>();
            var request = await repo.Get(requestId);
            if (request == null)
                throw new NotFoundException(nameof(request), requestId);
            if (request.RelativeId != relativeId)
                throw new UnAuthourizedException("This request does not belong to this Relative");
            if (request.Status == RequestStatus.Selected|| request.Status == RequestStatus.DoctorRemovalPending)
            {
                var patientRepo = UnitOfWork.GetRepository<Patient, int>();
                var patient = await patientRepo.Get(request.PatientId);

                if (patient == null)
                    throw new NotFoundException(nameof(patient), request.PatientId);
                patient.DoctorId = null;
                patientRepo.Update(patient);
            }
            request.Status = RequestStatus.Rejected;
            repo.Update(request);
            await UnitOfWork.CompleteAsync();
        }

    }
}
