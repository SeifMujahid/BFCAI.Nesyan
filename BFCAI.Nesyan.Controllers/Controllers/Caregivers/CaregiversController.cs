using BFCAI.Nesyan.Application.Abstraction.Models.Caregivers;
using BFCAI.Nesyan.Application.Abstraction.Services;
using BFCAI.Nesyan.Application.Abstraction.Services.Caregivers;
using BFCAI.Nesyan.Controllers.Controllers.Base;
using BFCAI.Nesyan.Controllers.Errors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Controllers.Controllers.Caregivers
{
    public class CaregiversController(IServiceManager serviceManager) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CaregiverToReturnDto>>> GetCaregivers()
        {
            var caregivers = await serviceManager.CaregiverService.GetCaregiversAsync();
            return Ok(caregivers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CaregiverToReturnDto>> GetCaregiver(int id)
        {

                var caregiver = await serviceManager.CaregiverService.GetCaregiverAsync(id);
                return Ok(caregiver);
           
        }

        [HttpGet("{id}/profile")]
        public async Task<ActionResult<CaregiverProfileDto>> GetCaregiverProfile(int id)
        {
            try
            {
                var profile = await serviceManager.CaregiverService.GetCaregiverProfileAsync(id);
                return Ok(profile);
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponse(404, ex.Message));
            }
        }

        [HttpGet("{caregiverId}/patients/{patientId}/home")]
        public async Task<IActionResult> GetPatientHome(int caregiverId,int patientId)
        {
            var result =await serviceManager.CaregiverService.GetPatientHome(caregiverId,patientId);

            return Ok(result);
        }
        [HttpGet("{caregiverId}/patients/{patientId}/reminders")]
        public async Task<IActionResult>GetPatientReminders(int caregiverId,int patientId,[FromQuery]int reminderType)
        {
            var result = await serviceManager.CaregiverService.GetPatientReminders(caregiverId,patientId,reminderType);

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<CaregiverToReturnDto>> CreateCaregiver(CaregiverToCreateDto request)
        {
            try
            {
                var caregiver = await serviceManager.CaregiverService.CreateCaregiverAsync(request);
                return Ok(caregiver);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCaregiver([FromForm] CaregiverToReturnDto dto)
        {
            try
            {
                await serviceManager.CaregiverService.UpdateCaregiverAsync(dto);
                return NoContent(); // 204
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponse(404));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCaregiver(int id)
        {
            try
            {
                await serviceManager.CaregiverService.DeleteCaregiverAsync(id);
                return NoContent(); // 204
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponse(404));
            }
        }
    }
}
