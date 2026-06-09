using BFCAI.Nesyan.Application.Abstraction.Models._Relations.CaregiverPatient;
using BFCAI.Nesyan.Application.Abstraction.Models.Patients;
using BFCAI.Nesyan.Application.Abstraction.Services;
using BFCAI.Nesyan.Controllers.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Controllers.Controllers._Relations.CaregiverPatientController
{
    [Route("api/CaregiverPatient")]
    public class CaregiverPatientController(IServiceManager serviceManager) : BaseApiController
    {
        [HttpPost("add-patient")]
        public async Task<IActionResult> AddPatientToCaregiver([FromBody] CaregiverPatientAddDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await serviceManager.CaregiverPatientService.AddPatientToCaregiverAsync(dto);
            return Ok("Patient added to caregiver successfully");
        }

        [HttpGet("caregiver/{caregiverId}/patients")]
        public async Task<ActionResult<IEnumerable<PatientSummaryDto>>> GetCaregiverPatients(int caregiverId)
        {
            var result = await serviceManager.CaregiverPatientService.GetCaregiverPatientsAsync(caregiverId);
            return Ok(result);
        }

        [HttpGet("patient/{patientId}/caregiver")]
        public async Task<ActionResult<PatientCaregiverDto>> GetPatientCaregiver(int patientId)
        {
            var result = await serviceManager.CaregiverPatientService.GetPatientCaregiverAsync(patientId);
            return Ok(result);
        }

        [HttpPut("patient/{patientId}/caregiver/{caregiverId}")]
        public async Task<IActionResult> UpdatePatientCaregiver(int patientId, int caregiverId)
        {
            await serviceManager.CaregiverPatientService.UpdatePatientCaregiverAsync(patientId, caregiverId);
            return NoContent();
        }

        [HttpDelete("caregiver/{caregiverId}/patient/{patientId}")]
        public async Task<IActionResult> RemovePatientFromCaregiver(int caregiverId, int patientId)
        {
            await serviceManager.CaregiverPatientService.RemovePatientFromCaregiverAsync(caregiverId, patientId);
            return NoContent();
        }
    }
}
