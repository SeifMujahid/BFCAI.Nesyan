using BFCAI.Nesyan.Application.Abstraction.Models._Relations.DoctorPatient;
using BFCAI.Nesyan.Application.Abstraction.Models.Patients;
using BFCAI.Nesyan.Application.Abstraction.Services;
using BFCAI.Nesyan.Controllers.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Controllers.Controllers._Relations.DoctorPatientController
{
    [Route("api/DoctorPatient")]
    public class DoctorPatientController(IServiceManager serviceManager) : BaseApiController
    {
        [HttpPost("add-patient")]
        public async Task<IActionResult> AddPatientToDoctor([FromBody] DoctorPatientAddDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await serviceManager.DoctorPatientService.AddPatientToDoctorAsync(dto);
            return Ok("Patient added to doctor successfully");
        }

        [HttpGet("doctor/{doctorId}/patients")]
        public async Task<ActionResult<IEnumerable<PatientSummaryDto>>> GetDoctorPatients(int doctorId)
        {
            var result = await serviceManager.DoctorPatientService.GetDoctorPatientsAsync(doctorId);
            return Ok(result);
        }

        [HttpGet("patient/{patientId}/doctor")]
        public async Task<ActionResult<PatientDoctorDto>> GetPatientDoctor(int patientId)
        {
            var result = await serviceManager.DoctorPatientService.GetPatientDoctorAsync(patientId);
            return Ok(result);
        }

        [HttpPut("patient/{patientId}/doctor/{doctorId}")]
        public async Task<IActionResult> UpdatePatientDoctor(int patientId, int doctorId)
        {
            await serviceManager.DoctorPatientService.UpdatePatientDoctorAsync(patientId, doctorId);
            return NoContent();
        }

        [HttpDelete("doctor/{doctorId}/patient/{patientId}")]
        public async Task<IActionResult> RemovePatientFromDoctor(int doctorId, int patientId)
        {
            await serviceManager.DoctorPatientService.RemovePatientFromDoctorAsync(doctorId, patientId);
            return NoContent();
        }
    }
}
