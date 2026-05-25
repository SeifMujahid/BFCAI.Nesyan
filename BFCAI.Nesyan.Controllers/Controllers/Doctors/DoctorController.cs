using BFCAI.Nesyan.Application.Abstraction.Models.Doctors;
using BFCAI.Nesyan.Application.Abstraction.Models.Patients;
using BFCAI.Nesyan.Application.Abstraction.Models.Reminders;
using BFCAI.Nesyan.Application.Abstraction.Services;
using BFCAI.Nesyan.Application.Abstraction.Services.Doctors;
using BFCAI.Nesyan.Controllers.Controllers.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;


namespace BFCAI.Nesyan.Controllers.Controllers.Doctors
{
    public class DoctorController(IServiceManager serviceManager) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoctorToReturnDto>>> GetDoctors()
        {
            var doctors = await serviceManager.DoctorService.GetDoctorsAsync();
            return Ok(doctors);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorToReturnDto>> GetDoctor(int id)
        {
            var doctor = await serviceManager.DoctorService.GetDoctorAsync(id);
            return Ok(doctor);
        }
        [HttpPost]
        public async Task<ActionResult<DoctorToReturnDto>> CreateDoctor([FromForm] DoctorToCreateDto request)
        {
            var doctor = await serviceManager.DoctorService.CreateDoctorAsync(request);
            return Ok(doctor);

        }
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateDoctor(DoctorToReturnDto dto)
        {
            await serviceManager.DoctorService.UpdateDoctorAsync(dto);
            return NoContent(); 
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDoctor(int id)
        {
            await serviceManager.DoctorService.DeleteDoctorAsync(id);
            return NoContent(); 
        }

        [HttpGet("{id}/patients")]
        public async Task<ActionResult<IEnumerable<PatientToReturnDto>>> GetDoctorPatients(int id)
        {
            var patients = await serviceManager.DoctorService.GetDoctorWithSpecAsync(id);
            return Ok(patients);
        }
        [HttpGet("{doctorId}/patient")]
        public async Task<ActionResult<IEnumerable<PatientToReturnDto>>> GetDoctorPatient(int doctorId,[FromQuery]int patientId)
        {
            var patients = await serviceManager.DoctorService.GetDoctorPatientWithSpecAsync(doctorId, patientId);
            return Ok(patients);
        }
        [HttpPatch("{doctorId}/patients/{patientId}/stage")]
        public async Task<ActionResult> DoctorUpdatePatientStage(int doctorId, int patientId, [FromQuery] int stageNumber)
        {
            await serviceManager.DoctorService.DoctorUpdatePatientStage(doctorId, patientId, stageNumber);
            return Ok("Patient stage updated successfully");
        }
        [HttpGet("{doctorId}/patients/{patientId}/medications")]
        public async Task<ActionResult<DoctorPatientMedicationsDto>> GetPatientMedications(int doctorId, int patientId)
        {
            var result = await serviceManager.DoctorService.GetPatientMedications(doctorId, patientId);
            return Ok(result);
        }
        [HttpPost("{doctorId}/patients/{patientId}/reminders")]
        public async Task<ActionResult> CreateReminderByDoctor(int doctorId, int patientId, ReminderToCreateDto dto)
        {
            await serviceManager.DoctorService.CreateReminderByDoctor(doctorId, patientId, dto);
            return Ok("Reminder created successfully");
        }
        [HttpPut("{doctorId}/patients/{patientId}/reminders/{reminderId}")]
        public async Task<ActionResult> UpdateReminderByDoctor(int doctorId, int patientId, int reminderId, ReminderToUpdateDto dto)
        {
            await serviceManager.DoctorService.UpdateReminderByDoctor(doctorId, patientId, reminderId, dto);
            return Ok("Reminder updated successfully");
        }
        [HttpGet("{id}/statistics")]
        public async Task<ActionResult<DoctorStatisticsDto>> GetDoctorStatistics(int id)
        {
            var stats = await serviceManager.DoctorService.GetDoctorStatisticsAsync(id);
            return Ok(stats);
        }
    }
}
