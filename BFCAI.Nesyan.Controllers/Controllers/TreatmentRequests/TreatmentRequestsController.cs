using BFCAI.Nesyan.Application.Abstraction.Models.TreatmentRequests;
using BFCAI.Nesyan.Application.Abstraction.Services;
using BFCAI.Nesyan.Application.Abstraction.Services.TreatmentRequests;
using BFCAI.Nesyan.Controllers.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace BFCAI.Nesyan.Controllers.Controllers.TreatmentRequests
{
    public class TreatmentRequestsController(IServiceManager ServiceManager) : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> CreateRequest(TreatmentRequestToCreateDto dto)
        {
            await ServiceManager.TreatmentRequestService.RealtiveCreateRequestAsync(dto);
            return Ok("Request created successfuly");
        }

        [HttpGet("doctor/{doctorId}/doctor-requests")]
        public async Task<ActionResult<IEnumerable<TreatmentRequestToReturnDto>>> GetDoctorPendingRequests(int doctorId, [FromQuery] int orderType = 0)
        {
            var requests = await ServiceManager.TreatmentRequestService.GetDoctorPendingRequestsAsync(doctorId,orderType);
            return Ok(requests);
        }
        [HttpGet("doctor/{relativeId}/relative-requests")]
        public async Task<ActionResult<IEnumerable<TreatmentRequestToReturnDto>>> GetRelativePendingRequests(int relativeId, [FromQuery] int orderType = 0)
        {
            var requests = await ServiceManager.TreatmentRequestService.GetRelativePendingRequestsAsync(relativeId, orderType);
            return Ok(requests);
        }
        [HttpPatch("{requestId}/doctor-accept")]
        public async Task<ActionResult> DoctorAcceptRequest(int requestId, [FromQuery] int doctorId)
        {
            await ServiceManager.TreatmentRequestService.DoctorAcceptRequestAsync(requestId, doctorId);
            return Ok("Treatment request accepted successfully");
        }
        [HttpPatch("{requestId}/relative-select")]
        public async Task<ActionResult> RelativeSelectDoctor(int requestId, [FromQuery] int relativeId)
        {
            await ServiceManager.TreatmentRequestService.RelativeSelectDoctorAsync(requestId, relativeId);
            return Ok("Treatment request accepted successfully");
        }
        [HttpPatch("{requestId}/doctor-reject")]
        public async Task<ActionResult> DoctorRejectRequest(int requestId, [FromQuery] int doctorId)
        {
            await ServiceManager.TreatmentRequestService.DoctorRejectRequestAsync(requestId,doctorId);
            return Ok("Treatment request rejected successfully");
        }
        [HttpPatch("{requestId}/relative-reject")]
        public async Task<ActionResult> RelativeRejectRequest(int requestId, [FromQuery] int relativeId)
        {
            await ServiceManager.TreatmentRequestService.RelativeRejectRequestAsync(requestId,relativeId);
            return Ok("Treatment request rejected successfully");
        }
    }
}
