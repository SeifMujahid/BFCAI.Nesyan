using BFCAI.Nesyan.Application.Abstraction.Models.TreatmentRequests;

namespace BFCAI.Nesyan.Application.Abstraction.Services.TreatmentRequests
{
    public interface ITreatmentRequestService
    {
        Task RealtiveCreateRequestAsync(TreatmentRequestToCreateDto dto);
        Task<IEnumerable<TreatmentRequestToReturnDto>> GetDoctorPendingRequestsAsync(int doctorId, int orderType = 0);
        Task<IEnumerable<TreatmentRequestToReturnDto>> GetRelativePendingRequestsAsync(int relativeId, int orderType = 0);
        Task RelativeSelectDoctorAsync(int requestId, int relativeId);
        Task DoctorAcceptRequestAsync(int requestId, int doctorId);
        Task DoctorRejectRequestAsync(int requestId,int doctorId);
        Task RelativeRejectRequestAsync(int requestId,int relativeId);
    }
}
