using BFCAI.Nesyan.Application.Abstraction.Models.Caregivers;
using BFCAI.Nesyan.Application.Abstraction.Models.Doctors;
using BFCAI.Nesyan.Application.Abstraction.Models.TreatmentRequests;
using BFCAI.Nesyan.Application.Abstraction.Services;
using BFCAI.Nesyan.Application.Abstraction.Services.TreatmentRequests;
using BFCAI.Nesyan.Controllers.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace BFCAI.Nesyan.Controllers.Controllers.TreatmentRequests
{
    public class TreatmentRequestsController(IServiceManager ServiceManager) : BaseApiController
    {
        [HttpGet("search-doctors")]
        public async Task<ActionResult<IEnumerable<DoctorSummaryDto>>> SearchDoctors([FromQuery] string name)
        {
            var doctors = await ServiceManager.TreatmentRequestService.SearchDoctors(name);

            return Ok(doctors);
        }
        [HttpGet("search-caregivers")]
        public async Task<ActionResult<IEnumerable<CaregiverSummaryDto>>> SearchCaregivers([FromQuery] string name)
        {
            var caregivers = await ServiceManager.TreatmentRequestService.SearchCaregiver(name);

            return Ok(caregivers);
        }
        [HttpPost]
        public async Task<IActionResult> CreateRequest(TreatmentRequestToCreateDto dto,[FromQuery] int actorType)
        {
            await ServiceManager.TreatmentRequestService.RealtiveCreateTreatmentRequestAsync(dto, actorType);
            return Ok("Request created successfuly");
        }

        [HttpGet("doctor/{actorId}/doctor-caregiver-requests")]
        public async Task<ActionResult<IEnumerable<TreatmentRequestToReturnDto>>> GetDoctorPendingRequests(int actorId,int actorType, [FromQuery] int orderType = 0)
        {
            var requests = await ServiceManager.TreatmentRequestService.GetDoctorOrCaregiverRequestsAsync(actorId, actorType, orderType);
            return Ok(requests);
        }
        [HttpGet("doctor/{relativeId}/relative-requests")]
        public async Task<ActionResult<IEnumerable<TreatmentRequestToReturnDto>>> GetRelativeRequests(int relativeId, [FromQuery] int actorType, [FromQuery] int orderType = 0)
        {
            var requests = await ServiceManager.TreatmentRequestService.GetRelativeRequestsAsync(relativeId,actorType, orderType);
            return Ok(requests);
        }
        [HttpGet("doctor/{patientId}/patient-requests")]
        public async Task<ActionResult<IEnumerable<TreatmentRequestToReturnDto>>> GetPatientRequests(int patientId, [FromQuery] int actorType, [FromQuery] int orderType = 0)
        {
            var requests = await ServiceManager.TreatmentRequestService.GetPatientRequestsAsync(patientId, actorType, orderType);
            return Ok(requests);
        }
        [HttpPatch("{requestId}/doctor-caregiver-accept")]
        public async Task<ActionResult> DoctorAcceptRequest(int requestId, int actorType, [FromQuery] int actorId)
        {
            await ServiceManager.TreatmentRequestService.DoctorOrCaregiverAcceptRequestAsync(requestId, actorType, actorId);
            return Ok("Treatment request accepted successfully");
        }
        [HttpPatch("{requestId}/relative-select")]
        public async Task<ActionResult> RelativeSelectDoctor(int requestId, [FromQuery] int relativeId, [FromQuery] int actorType)
        {
            await ServiceManager.TreatmentRequestService.RelativeSelectDoctorOrCaregiverAsync(requestId, relativeId, actorType);
            return Ok("Treatment request accepted successfully");
        }
        [HttpPatch("{requestId}/doctor-caregiver-reject")]
        public async Task<ActionResult> DoctorRejectRequest(int requestId, [FromQuery] int actorId, int actorType)
        {
            await ServiceManager.TreatmentRequestService.DoctorRejectRequestAsync(requestId,actorId,actorType);
            return Ok("Your request has been sent to the patient's relative. If no response is received within 7 days, it will be automatically approved.");
        }
        [HttpPatch("{requestId}/relative-reject")]
        public async Task<ActionResult> RelativeRejectRequest(int requestId, [FromQuery] int relativeId, int actorType)
        {
            await ServiceManager.TreatmentRequestService.RelativeRejectRequestAsync(requestId,relativeId,actorType);
            return Ok("Treatment request rejected successfully");
        }
    }
}
