using BFCAI.Nesyan.Application.Abstraction.Models.IoT;
using System.Collections.Generic;

namespace BFCAI.Nesyan.Application.Abstraction.Services.IoT
{
    public interface ITelemetryStore
    {
        void Add(int patientId, TelemetryRequestDto dto);
        TelemetryRequestDto? GetLatest(int patientId);
        IEnumerable<TelemetryRequestDto> GetHistory(int patientId, int count);
    }
}
