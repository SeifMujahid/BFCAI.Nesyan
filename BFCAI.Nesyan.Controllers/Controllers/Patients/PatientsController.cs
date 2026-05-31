using BFCAI.Nesyan.Application.Abstraction.Services;
using BFCAI.Nesyan.Application.Abstraction.Services.Patients;
using BFCAI.Nesyan.Application.Abstraction.Models.Patients;
using BFCAI.Nesyan.Application.Abstraction.Models.Reminders;
using BFCAI.Nesyan.Controllers.Controllers.Base;
using BFCAI.Nesyan.Controllers.Errors;
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
                return NotFound(new ApiResponse(404, ex.Message));
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
                return NotFound(new ApiResponse(404, ex.Message));
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

        [HttpPost("{patientId}/reminders")]
        public async Task<IActionResult> CreateReminder(int patientId, [FromBody] ReminderToCreateDto dto)
        {
            try
            {
                await serviceManager.PatientService.CreateReminderAsync(patientId, dto);
                return Ok("Reminder created Successfuly");
            }
            catch (System.Exception ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [HttpPut("{patientId}/reminders/{reminderId}")]
        public async Task<IActionResult> UpdateReminder(int patientId, int reminderId, [FromBody] ReminderToUpdateDto dto)
        {
            try
            {
                await serviceManager.PatientService.UpdateReminderAsync(patientId, reminderId, dto);
                return NoContent();
            }
            catch (System.Exception ex)
            {
                return NotFound(new ApiResponse(404, ex.Message));
            }
        }

        [HttpDelete("{patientId}/reminders/{reminderId}")]
        public async Task<IActionResult> DeleteReminder(int patientId, int reminderId)
        {
            try
            {
                await serviceManager.PatientService.DeleteReminderAsync(patientId, reminderId);
                return NoContent();
            }
            catch (System.Exception ex)
            {
                return NotFound(new ApiResponse(404, ex.Message));
            }
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
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatePatient([FromForm] PatientToUpdateDto dto)
        {
            try
            {
                await serviceManager.PatientService.UpdatePatientAsync(dto);
                return NoContent(); // 204
            }
            catch (System.Exception ex)
            {
                return NotFound(new ApiResponse(404, ex.Message));
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
                return NotFound(new ApiResponse(404, ex.Message));
            }
        }
    }
}
