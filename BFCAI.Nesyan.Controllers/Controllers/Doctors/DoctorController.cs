using BFCAI.Nesyan.Application.Abstraction.Models.Doctors;
using BFCAI.Nesyan.Application.Abstraction.Services.Doctors;
using BFCAI.Nesyan.Controllers.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using BFCAI.Nesyan.Application.Abstraction.Models.Patients;


namespace BFCAI.Nesyan.Controllers.Controllers.Doctors
{
    public class DoctorController(IDoctorService DoctorService) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoctorToReturnDto>>> GetDoctors()
        {
            var doctors = await DoctorService.GetDoctorsAsync();
            return Ok(doctors);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorToReturnDto>> GetDoctor(int id)
        {
            var doctor = await DoctorService.GetDoctorAsync(id);

            if (doctor is null)
                return NotFound($"Doctor with id {id} not found");

            return Ok(doctor);
        }
        [HttpPost]
        public async Task<ActionResult<DoctorToReturnDto>> CreateDoctor([FromForm] DoctorCreationRequest request)
        {
            try
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", "Doctors");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var gradDegreeFileName = Guid.NewGuid().ToString() + Path.GetExtension(request.GraduationDegreeFile.FileName);
                var medCardFileName = Guid.NewGuid().ToString() + Path.GetExtension(request.MedicalAssociationCardFile.FileName);

                var gradDegreePath = Path.Combine(uploadsFolder, gradDegreeFileName);
                var medCardPath = Path.Combine(uploadsFolder, medCardFileName);

                using (var stream = new FileStream(gradDegreePath, FileMode.Create))
                {
                    await request.GraduationDegreeFile.CopyToAsync(stream);
                }

                using (var stream = new FileStream(medCardPath, FileMode.Create))
                {
                    await request.MedicalAssociationCardFile.CopyToAsync(stream);
                }

                var dto = new DoctorToCreateDto
                {
                    NationalId = request.NationalId,
                    FName = request.FName,
                    LName = request.LName,
                    UserName = request.UserName,
                    Email = request.Email,
                    Password = request.Password,
                    Gender = request.Gender,
                    Country = request.Country,
                    City = request.City,
                    Age = request.Age,
                    GraduationDegree = $"/Uploads/Doctors/{gradDegreeFileName}",
                    MedicalAssociationCard = $"/Uploads/Doctors/{medCardFileName}"
                };

                var doctor = await DoctorService.CreateDoctorAsync(dto);
                return CreatedAtAction(
                       nameof(GetDoctor),
                       new { id = doctor.Id },
                       doctor);
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
                await DoctorService.UpdateDoctorAsync(dto);
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
                await DoctorService.DeleteDoctorAsync(id);
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
                var patients = await DoctorService.GetDoctorPatientsAsync(id);
                return Ok(patients);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            //}

            //[HttpGet("{id}/statistics")]
            //internal async Task<ActionResult<DoctorStatisticsDto>> GetDoctorStatistics(int id)
            //{
            //    try
            //    {
            //        var stats = await DoctorService.GetDoctorStatisticsAsync(id);
            //        return Ok(stats);
            //    }
            //    catch (Exception ex)
            //    {
            //        return NotFound(new { message = ex.Message });
            //    }
            //}
        }
    }
}
