using BFCAI.Nesyan.Application.Abstraction.Models.Patients;
using BFCAI.Nesyan.Application.Abstraction.Services;
using BFCAI.Nesyan.Controllers.Controllers.Base;
using BFCAI.Nesyan.Controllers.Errors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Controllers.Controllers.Patients
{
    public class FamilyMembersController(IServiceManager serviceManager) : BaseApiController
    {
        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<IEnumerable<FamilyMemberDto>>> GetPatientFamilyMembers(int patientId)
        {
            try
            {
                var members = await serviceManager.FamilyMembersService.GetFamilyMembersByPatientIdAsync(patientId);
                return Ok(members);
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponse(404, ex.Message));
            }
        }

        [HttpPost]
        public async Task<ActionResult<FamilyMemberDto>> CreateFamilyMember([FromForm] FamilyMemberCreateDto dto)
        {
            try
            {
                var member = await serviceManager.FamilyMembersService.CreateFamilyMemberAsync(dto);
                return CreatedAtAction(nameof(GetPatientFamilyMembers), new { patientId = member.PatientId }, member);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<FamilyMemberDto>> UpdateFamilyMember(int id, [FromForm] FamilyMemberUpdateDto dto)
        {
            try
            {
                var member = await serviceManager.FamilyMembersService.UpdateFamilyMemberAsync(id, dto);
                return Ok(member);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFamilyMember(int id)
        {
            try
            {
                var success = await serviceManager.FamilyMembersService.DeleteFamilyMemberAsync(id);
                if (!success)
                    return NotFound(new ApiResponse(404, $"Family member with ID {id} not found."));
                return NoContent(); // 204
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }
    }
}
