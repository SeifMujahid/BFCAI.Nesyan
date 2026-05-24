using BFCAI.Nesyan.Application.Abstraction.Services;
using BFCAI.Nesyan.Application.Abstraction.Services.Patients;
using BFCAI.Nesyan.Application.Abstraction.Models.Patients;
using BFCAI.Nesyan.Controllers.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace BFCAI.Nesyan.Controllers.Controllers.Patients
{
    public class PatientsController(IServiceManager serviceManager) : BaseApiController
    {
        [HttpPatch("{id}/stage")]
        public async Task<ActionResult> UpdatePatientStage(int id, [FromBody] int newStage)
        {
            try
            {
                await serviceManager.PatientService.UpdatePatientStageAsync(id, newStage);
                return NoContent(); // 204
            }
            catch (System.Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}/profile")]
        public async Task<ActionResult<PatientFullProfileDto>> GetPatientProfile(int id)
        {
            try
            {
                var profile = await serviceManager.PatientService.GetPatientProfileAsync(id);
                return Ok(profile);
            }
            catch (System.Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientToReturnDto>>> GetPatients()
        {
            var patients = await serviceManager.PatientService.GetPatientsAsync();
            return Ok(patients);
        }

        [HttpGet("{patientId}/reminders")]
        public async Task<IActionResult>GetPatientReminder(int patientId,[FromQuery]int reminderType)
        {
            var result = await serviceManager.PatientService.GetPatientReminder(patientId,reminderType);

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<PatientToReturnDto>> CreatePatient(PatientToCreateDto request)
        {
            try
            {
                var patient = await serviceManager.PatientService.CreatePatientAsync(request);
                return Ok(patient);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePatient(PatientToReturnDto dto)
        {
            try
            {
                await serviceManager.PatientService.UpdatePatientAsync(dto);
                return NoContent(); // 204
            }
            catch (System.Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePatient(int id)
        {
            try
            {
                await serviceManager.PatientService.DeletePatientAsync(id);
                return NoContent(); // 204
            }
            catch (System.Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
