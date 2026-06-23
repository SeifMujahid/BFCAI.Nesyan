using BFCAI.Nesyan.Application.Abstraction.Models.IoT;
using BFCAI.Nesyan.Application.Abstraction.Services.IoT;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace BFCAI.Nesyan.Application.Services.IoT
{
    public class TelemetryStore : ITelemetryStore
    {
        private readonly ConcurrentDictionary<int, List<TelemetryRequestDto>> _telemetryData = new ConcurrentDictionary<int, List<TelemetryRequestDto>>();
        private readonly object _lock = new object();
        private const int MaxHistory = 1000;

        public void Add(int patientId, TelemetryRequestDto dto)
        {
            var list = _telemetryData.GetOrAdd(patientId, _ => new List<TelemetryRequestDto>());
            lock (list)
            {
                list.Add(dto);
                if (list.Count > MaxHistory)
                {
                    list.RemoveAt(0);
                }
            }
        }

        public TelemetryRequestDto? GetLatest(int patientId)
        {
            if (_telemetryData.TryGetValue(patientId, out var list))
            {
                lock (list)
                {
                    return list.LastOrDefault();
                }
            }
            return null;
        }

        public IEnumerable<TelemetryRequestDto> GetHistory(int patientId, int count)
        {
            if (_telemetryData.TryGetValue(patientId, out var list))
            {
                lock (list)
                {
                    return list.AsEnumerable().Reverse().Take(count).ToList();
                }
            }
            return Enumerable.Empty<TelemetryRequestDto>();
        }
    }
}
