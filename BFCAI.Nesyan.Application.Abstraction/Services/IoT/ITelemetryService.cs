using BFCAI.Nesyan.Application.Abstraction.Models.IoT;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Abstraction.Services.IoT
{
    public interface ITelemetryService
    {
        Task AddTelemetryAsync(TelemetryRequestDto dto);
        TelemetryRequestDto? GetLatestTelemetry(int patientId);
        IEnumerable<TelemetryRequestDto> GetTelemetryHistory(int patientId, int count);
    }
}
