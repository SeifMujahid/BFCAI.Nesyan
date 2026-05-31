using BFCAI.Nesyan.Application.Abstraction.Models.Location;
using BFCAI.Nesyan.Application.Abstraction.Services;
using BFCAI.Nesyan.Controllers.Controllers.Base;
using BFCAI.Nesyan.Controllers.Errors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Controllers.Controllers.Location
{
    [Route("api/v1/patients/{patientId}")]
    [ApiController]
    public class LocationController : BaseApiController
    {
        private readonly IServiceManager _serviceManager;

        public LocationController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet("safe-zones")]
        public async Task<IActionResult> GetSafeZones(int patientId)
        {
            try
            {
                var result = await _serviceManager.LocationService.GetSafeZonesAsync(patientId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [HttpPost("safe-zones")]
        public async Task<IActionResult> CreateSafeZone(int patientId, [FromBody] CreateSafeZoneDto dto)
        {
            if (dto == null)
                return BadRequest(new ApiResponse(400, "Invalid safe zone data."));

            try
            {
                var result = await _serviceManager.LocationService.CreateSafeZoneAsync(patientId, dto);
                return StatusCode(201, result); // 201 Created
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [HttpPut("safe-zones/{safeZoneId}")]
        public async Task<IActionResult> UpdateSafeZone(int patientId, int safeZoneId, [FromBody] UpdateSafeZoneDto dto)
        {
            if (dto == null)
                return BadRequest(new ApiResponse(400, "Invalid safe zone data."));

            try
            {
                var result = await _serviceManager.LocationService.UpdateSafeZoneAsync(patientId, safeZoneId, dto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse(404, ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [HttpDelete("safe-zones/{safeZoneId}")]
        public async Task<IActionResult> DeleteSafeZone(int patientId, int safeZoneId)
        {
            try
            {
                var deleted = await _serviceManager.LocationService.DeleteSafeZoneAsync(patientId, safeZoneId);
                if (!deleted)
                    return NotFound(new ApiResponse(404, $"Safe zone with ID {safeZoneId} not found for patient {patientId}."));

                return Ok(new DeleteSafeZoneResponseDto());
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [HttpPost("location")]
        public async Task<IActionResult> SubmitLocation(int patientId, [FromBody] LocationSubmitDto dto)
        {
            if (dto == null)
                return BadRequest(new ApiResponse(400, "Invalid coordinates data."));

            try
            {
                var result = await _serviceManager.LocationService.SubmitLocationAsync(patientId, dto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse(404, ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [HttpGet("location/current")]
        public async Task<IActionResult> GetLastKnownLocation(int patientId)
        {
            try
            {
                var result = await _serviceManager.LocationService.GetLastKnownLocationAsync(patientId);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse(404, ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [HttpGet("location/history")]
        public async Task<IActionResult> GetLocationHistory(int patientId, [FromQuery] DateTime from, [FromQuery] DateTime to, [FromQuery] int limit = 100)
        {
            if (from == default || to == default)
                return BadRequest(new ApiResponse(400, "Both 'from' and 'to' date parameters are required and must be valid ISO dates."));

            try
            {
                var result = await _serviceManager.LocationService.GetLocationHistoryAsync(patientId, from, to, limit);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }

        [HttpGet("location/violations")]
        public async Task<IActionResult> GetViolations(int patientId)
        {
            try
            {
                var result = await _serviceManager.LocationService.GetViolationsAsync(patientId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, ex.Message));
            }
        }
    }
}
