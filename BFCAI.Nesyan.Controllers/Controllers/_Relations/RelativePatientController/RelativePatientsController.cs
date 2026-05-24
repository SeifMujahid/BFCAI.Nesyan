using BFCAI.Nesyan.Application.Abstraction.Models._Relations.RelativePatient;
using BFCAI.Nesyan.Application.Abstraction.Models.Reminders;
using BFCAI.Nesyan.Application.Abstraction.Services;
using BFCAI.Nesyan.Application.Abstraction.Services._Relations;
using BFCAI.Nesyan.Controllers.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BFCAI.Nesyan.Controllers.Controllers._Relations.RelativePatientController
{
    [Microsoft.AspNetCore.Components.Route("api/relative")]
    public class RelativePatientsController(IServiceManager serviceManager):BaseApiController
    {
        [Microsoft.AspNetCore.Mvc.HttpPost("create-relation")]
        public async Task<IActionResult> CreateRelativePatientRelation(int relativeId,int patientId)
        {
            await serviceManager.RelativePatientService.CreateRelativePatientRelation( relativeId,patientId);
            return Ok("Relative patient relation created successfully");
        }
        [Microsoft.AspNetCore.Mvc.HttpGet("{relativeId}/patients")]
        public async Task<ActionResult<RelativePatientsDto>> GetRelativePatients(int relativeId)
        {
            var result =await serviceManager.RelativePatientService.GetRelativePatients(relativeId);
            return Ok(result);
        }
        [Microsoft.AspNetCore.Mvc.HttpGet("{relativeId}/patients/{patientId}/home")]
        public async Task<ActionResult<RelativePatientsDto>> GetPatientHome(int relativeId,int patientId)
        {
            var result =await serviceManager.RelativePatientService.GetPatientHomeAsync(relativeId,patientId);
            return Ok(result);
        }
        [Microsoft.AspNetCore.Mvc.HttpGet("{relativeId}/patients/{patientId}/reminders")]
        public async Task<IActionResult> GetPatientReminders(int relativeId,int patientId,[FromQuery]int reminderType)
        {
            var result = await serviceManager.RelativePatientService.GetPatientReminders(relativeId,patientId,reminderType);
            return Ok(result);
        }
        [Microsoft.AspNetCore.Mvc.HttpPost("{relativeId}/patients/{patientId}/reminders")]
        public async Task<IActionResult> CreateReminder(int relativeId,int patientId,[FromBody]ReminderToCreateDto dto)
        {
            await serviceManager.RelativePatientService.CreateReminder(relativeId,patientId,dto);
            return Ok("Reminder created Successfuly");
        }
        [Microsoft.AspNetCore.Mvc.HttpPut("{relativeId}/patients/{patientId}/reminders/{reminderId}")]
        public async Task<IActionResult>UpdateReminder(int relativeId,int patientId,int reminderId,[FromBody] ReminderToUpdateDto dto)
        {
            await serviceManager.RelativePatientService.UpdateReminder(relativeId,patientId,reminderId,dto);
            return NoContent();
        }
        [Microsoft.AspNetCore.Mvc.HttpDelete("{relativeId}/patients/{patientId}/reminders/{reminderId}")]
        public async Task<IActionResult>DeleteReminder(int relativeId,int patientId,int reminderId)
        {
            await serviceManager.RelativePatientService.DeleteReminder(relativeId,patientId,reminderId);
            return NoContent();
        }

    }
}
