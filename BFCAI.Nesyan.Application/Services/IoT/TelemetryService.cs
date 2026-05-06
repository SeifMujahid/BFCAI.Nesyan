using BFCAI.Nesyan.Application.Abstraction.Models.IoT;
using BFCAI.Nesyan.Application.Abstraction.Services.IoT;
using BFCAI.Nesyan.Domain.Contracts;
using BFCAI.Nesyan.Domain.Entities.IoT;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Services.IoT
{
    public class TelemetryService : ITelemetryService
    {
        private readonly ITelemetryStore _telemetryStore;
        private readonly IUnitOfWork _unitOfWork;

        public TelemetryService(ITelemetryStore telemetryStore, IUnitOfWork unitOfWork)
        {
            _telemetryStore = telemetryStore;
            _unitOfWork = unitOfWork;
        }

        public async Task AddTelemetryAsync(TelemetryRequestDto dto)
        {
            // Store in fast memory cache
            _telemetryStore.Add(dto.PatientId, dto);

            // Store in persistent database
            var entity = new PatientTelemetry
            {
                PatientId = dto.PatientId,
                Hr = dto.Hr,
                Spo2 = dto.Spo2,
                Steps = dto.Steps,
                Lat = dto.Lat,
                Lng = dto.Lng,
                Status = dto.Status,
                Timestamp = dto.Timestamp
            };

            await _unitOfWork.GetRepository<PatientTelemetry, int>().AddAsync(entity);
            await _unitOfWork.CompleteAsync();
        }

        public TelemetryRequestDto? GetLatestTelemetry(int patientId)
        {
            return _telemetryStore.GetLatest(patientId);
        }

        public IEnumerable<TelemetryRequestDto> GetTelemetryHistory(int patientId, int count)
        {
            return _telemetryStore.GetHistory(patientId, count);
        }
    }
}
