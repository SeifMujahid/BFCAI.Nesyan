using BFCAI.Nesyan.Application.Abstraction.Models.IoT;
using BFCAI.Nesyan.Application.Abstraction.Services;
using BFCAI.Nesyan.Application.Abstraction.Services.IoT;
using BFCAI.Nesyan.Controllers.Errors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Controllers.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TelemetryController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly ITelemetryNotifier _telemetryNotifier;

        public TelemetryController(IServiceManager serviceManager, ITelemetryNotifier telemetryNotifier)
        {
            _serviceManager = serviceManager;
            _telemetryNotifier = telemetryNotifier;
        }

        [HttpPost]
        public async Task<IActionResult> AddTelemetry([FromBody] TelemetryRequestDto dto)
        {
            if (dto == null || dto.PatientId <= 0)
                return BadRequest(new ApiResponse(400, "Invalid telemetry data or missing PatientId."));

            await _serviceManager.TelemetryService.AddTelemetryAsync(dto);
            await _telemetryNotifier.NotifyTelemetryAsync(dto);

            return Ok(new { message = "Telemetry added and broadcasted successfully." });
        }

        [HttpGet("latest/{patientId}")]
        public IActionResult GetLatestTelemetry(int patientId)
        {
            var latest = _serviceManager.TelemetryService.GetLatestTelemetry(patientId);
            if (latest == null)
                return NotFound(new ApiResponse(404, "No telemetry data available for this patient."));

            return Ok(latest);
        }

        [HttpGet("history/{patientId}")]
        public IActionResult GetTelemetryHistory(int patientId, [FromQuery] int count = 100)
        {
            var history = _serviceManager.TelemetryService.GetTelemetryHistory(patientId, count);
            return Ok(history);
        }
    }
}
