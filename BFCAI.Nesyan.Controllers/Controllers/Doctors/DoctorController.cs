using BFCAI.Nesyan.Application.Abstraction.Models.Doctors;
using BFCAI.Nesyan.Application.Abstraction.Services.Doctors;
using BFCAI.Nesyan.Controllers.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using BFCAI.Nesyan.Application.Abstraction.Models.Patients;
using BFCAI.Nesyan.Application.Abstraction.Services;


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

            if (doctor is null)
                return NotFound($"Doctor with id {id} not found");

            return Ok(doctor);
        }
        [HttpPost]
        public async Task<ActionResult<DoctorToReturnDto>> CreateDoctor([FromForm] DoctorToCreateDto request)
        {
            try
            {
                var doctor = await serviceManager.DoctorService.CreateDoctorAsync(request);
                return Ok(doctor);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateDoctor(DoctorToReturnDto dto)
        {
            try
            {
                await serviceManager.DoctorService.UpdateDoctorAsync(dto);
                return NoContent(); // 204
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDoctor(int id)
        {
            try
            {
                await serviceManager.DoctorService.DeleteDoctorAsync(id);
                return NoContent(); // 204
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}/patients")]
        public async Task<ActionResult<IEnumerable<PatientToReturnDto>>> GetDoctorPatients(int id)
        {
            try
            {
                var patients = await serviceManager.DoctorService.GetDoctorPatientsAsync(id);
                return Ok(patients);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}/statistics")]
            public async Task<ActionResult<DoctorStatisticsDto>> GetDoctorStatistics(int id)
            {
                try
                {
                    var stats = await serviceManager.DoctorService.GetDoctorStatisticsAsync(id);
                    return Ok(stats);
                }
                catch (Exception ex)
                {
                    return NotFound(new { message = ex.Message });
                }
            }
        }
    }
