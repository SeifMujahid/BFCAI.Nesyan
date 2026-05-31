
using BFCAI.Nesyan.Application.Abstraction.Models.Reminders;
using BFCAI.Nesyan.Application.Abstraction.Models.Routines;
using BFCAI.Nesyan.Application.Abstraction.Services.Medications;
using BFCAI.Nesyan.Controllers.Controllers.Base;
using BFCAI.Nesyan.Controllers.Errors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Controllers.Controllers.Medications
{
    public class MedicationsController(IMedicationService MedicationService) : BaseApiController
    {
        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<IEnumerable<RoutineToReturnDto>>> GetPatientMedications(int patientId)
        {
            try
            {
                var meds = await MedicationService.GetPatientMedicationsAsync(patientId);
                return Ok(meds);
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponse(404, ex.InnerException?.Message ?? ex.Message));
            }
        }

        [HttpPost]
        public async Task<ActionResult<RoutineToReturnDto>> AddMedication([FromBody] ReminderToCreateDto dto)
        {
            try
            {
                var med = await MedicationService.AddMedicationAsync(dto);
                return CreatedAtAction(nameof(GetPatientMedications), new { patientId = med.Id }, med);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, ex.InnerException?.Message ?? ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMedication(int id)
        {
            try
            {
                await MedicationService.DeleteMedicationAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponse(404, ex.InnerException?.Message ?? ex.Message));
            }
        }
    }
}
