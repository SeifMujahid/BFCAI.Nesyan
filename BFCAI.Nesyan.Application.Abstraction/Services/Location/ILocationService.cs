using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BFCAI.Nesyan.Application.Abstraction.Models.Location;

namespace BFCAI.Nesyan.Application.Abstraction.Services.Location
{
    public interface ILocationService
    {
        Task<IEnumerable<SafeZoneDto>> GetSafeZonesAsync(int patientId);
        Task<SafeZoneResponseDto> CreateSafeZoneAsync(int patientId, CreateSafeZoneDto dto);
        Task<SafeZoneResponseDto> UpdateSafeZoneAsync(int patientId, int safeZoneId, UpdateSafeZoneDto dto);
        Task<bool> DeleteSafeZoneAsync(int patientId, int safeZoneId);
        Task<LocationSubmitResponseDto> SubmitLocationAsync(int patientId, LocationSubmitDto dto);
        Task<PatientLastLocationDto> GetLastKnownLocationAsync(int patientId);
        Task<IEnumerable<LocationHistoryDto>> GetLocationHistoryAsync(int patientId, DateTime from, DateTime to, int limit = 100);
        Task<IEnumerable<GeofenceViolationDto>> GetViolationsAsync(int patientId);
    }
}
