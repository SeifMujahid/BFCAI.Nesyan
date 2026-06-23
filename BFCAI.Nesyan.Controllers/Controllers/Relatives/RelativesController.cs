using BFCAI.Nesyan.Application.Abstraction.Models.Relatives;
using BFCAI.Nesyan.Application.Abstraction.Services;
using BFCAI.Nesyan.Controllers.Controllers.Base;
using BFCAI.Nesyan.Controllers.Errors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Controllers.Controllers.Relatives
{
    public class RelativesController(IServiceManager serviceManager) : BaseApiController
    {
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<RelativeToReturnDto>>> GetRelatives()
        //{
        //    var relatives = await serviceManager.RelativeService.GetRelativesAsync();
        //    return Ok(relatives);
        //}

        [HttpGet("{id}")]
        public async Task<ActionResult<RelativeToReturnDto>> GetRelative(int id)
        {
            try
            {
                var relative = await serviceManager.RelativeService.GetRelativeAsync(id);
                return Ok(relative);
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponse(404, $"Relative with id {id} not found: {ex.Message}"));
            }
        }

        [HttpGet("{id}/profile")]
        public async Task<ActionResult<RelativeProfileDto>> GetRelativeProfile(int id)
        {
            try
            {
                var profile = await serviceManager.RelativeService.GetRelativeProfileAsync(id);
                return Ok(profile);
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponse(404, ex.Message));
            }
        }

        [HttpPost]
        public async Task<ActionResult<RelativeToReturnDto>> CreateRelative(RelativeToCreateDto request)
        {
            try
            {
                var relative = await serviceManager.RelativeService.CreateRelativeAsync(request);
                return Ok(relative);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateRelative([FromForm] RelativeToReturnDto dto)
        {
            try
            {
                await serviceManager.RelativeService.UpdateRelativeAsync(dto);
                return NoContent(); // 204
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponse(404, ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRelative(int id)
        {
            try
            {
                await serviceManager.RelativeService.DeleteRelativeAsync(id);
                return NoContent(); // 204
            }
            catch (Exception ex)
            {
                return NotFound(new ApiResponse(404, ex.Message));
            }
        }

    }
}
