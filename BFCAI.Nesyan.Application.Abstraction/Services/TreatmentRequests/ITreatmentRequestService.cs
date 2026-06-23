using BFCAI.Nesyan.Application.Abstraction.Models.Caregivers;
using BFCAI.Nesyan.Application.Abstraction.Models.Doctors;
using BFCAI.Nesyan.Application.Abstraction.Models.TreatmentRequests;

namespace BFCAI.Nesyan.Application.Abstraction.Services.TreatmentRequests
{
    public interface ITreatmentRequestService
    {
        Task<IEnumerable<DoctorSummaryDto>> SearchDoctors(string name);
        Task<IEnumerable<CaregiverSummaryDto>> SearchCaregiver(string name);
        Task RealtiveCreateTreatmentRequestAsync(TreatmentRequestToCreateDto dto, int actorType);
        Task<IEnumerable<TreatmentRequestToReturnDto>> GetDoctorOrCaregiverRequestsAsync(int doctorId, int actorType, int orderType = 0);
        Task<IEnumerable<TreatmentRequestToReturnDto>> GetRelativeRequestsAsync(int relativeId, int actorType, int orderType = 0);
        public Task<IEnumerable<TreatmentRequestToReturnDto>> GetPatientRequestsAsync(int relativeId, int actorType, int orderType = 0);
        Task RelativeSelectDoctorOrCaregiverAsync(int requestId, int relativeId, int actorType);
        public Task DoctorOrCaregiverAcceptRequestAsync(int requestId, int actorType, int actorId);
        Task DoctorRejectRequestAsync(int requestId,int actorId, int actorType);
        Task RelativeRejectRequestAsync(int requestId,int relativeId, int actorType);
    }
}
