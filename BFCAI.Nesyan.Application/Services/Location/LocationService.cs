using BFCAI.Nesyan.Application.Abstraction.Models.Location;
using BFCAI.Nesyan.Application.Abstraction.Services.Location;
using BFCAI.Nesyan.Domain.Contracts;
using BFCAI.Nesyan.Domain.Entities.Location;
using BFCAI.Nesyan.Domain.Entities.Primary.Patients;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Services.Location
{
    public class LocationService : ILocationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

        public LocationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SafeZoneDto>> GetSafeZonesAsync(int patientId)
        {
            var safeZones = await _unitOfWork.GetRepository<SafeZone, int>()
                .GetTableNoTracking()
                .Where(x => x.PatientId == patientId)
                .ToListAsync();

            return safeZones.Select(MapToSafeZoneDto);
        }

        public async Task<SafeZoneResponseDto> CreateSafeZoneAsync(int patientId, CreateSafeZoneDto dto)
        {
            var safeZone = new SafeZone
            {
                PatientId = patientId,
                Name = dto.Name,
                Phone = dto.Phone,
                Type = (GeofenceType)dto.Type,
                Geometry = JsonSerializer.Serialize(dto.Geometry, _jsonOptions),
                IsActive = true
            };

            await _unitOfWork.GetRepository<SafeZone, int>().AddAsync(safeZone);
            await _unitOfWork.CompleteAsync();

            return new SafeZoneResponseDto
            {
                SafeZoneId = $"sz_{safeZone.Id}",
                Status = "ACTIVE",
                Message = "Safe zone successfully created."
            };
        }

        public async Task<SafeZoneResponseDto> UpdateSafeZoneAsync(int patientId, int safeZoneId, UpdateSafeZoneDto dto)
        {
            var repo = _unitOfWork.GetRepository<SafeZone, int>();
            var safeZone = await repo.Get(safeZoneId);

            if (safeZone == null || safeZone.PatientId != patientId)
            {
                throw new KeyNotFoundException($"Safe zone with ID {safeZoneId} not found for patient {patientId}.");
            }

            safeZone.Name = dto.Name;
            safeZone.Phone = dto.Phone;
            safeZone.Type = (GeofenceType)dto.Type;
            safeZone.Geometry = JsonSerializer.Serialize(dto.Geometry, _jsonOptions);
            safeZone.IsActive = dto.IsActive;

            repo.Update(safeZone);
            await _unitOfWork.CompleteAsync();

            return new SafeZoneResponseDto
            {
                SafeZoneId = $"sz_{safeZone.Id}",
                Status = "UPDATED",
                Message = "Safe zone updated successfully."
            };
        }

        public async Task<bool> DeleteSafeZoneAsync(int patientId, int safeZoneId)
        {
            var repo = _unitOfWork.GetRepository<SafeZone, int>();
            var safeZone = await repo.Get(safeZoneId);

            if (safeZone == null || safeZone.PatientId != patientId)
            {
                return false;
            }

            repo.Delete(safeZone);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<LocationSubmitResponseDto> SubmitLocationAsync(int patientId, LocationSubmitDto dto)
        {
            // 1. Fetch Patient and their Active Safe Zones
            var patient = await _unitOfWork.GetRepository<Patient, int>()
                .GetTableNoTracking()
                .Include(p => p.Caregiver)
                .Include(p => p.PatientRelatives)
                    .ThenInclude(pr => pr.Relative)
                .FirstOrDefaultAsync(p => p.Id == patientId);

            if (patient == null)
            {
                throw new KeyNotFoundException($"Patient with ID {patientId} not found.");
            }

            var activeZones = await _unitOfWork.GetRepository<SafeZone, int>()
                .GetTableNoTracking()
                .Where(z => z.PatientId == patientId && z.IsActive)
                .ToListAsync();

            // 2. Evaluate Geofences
            var evaluatedZonesResult = new Dictionary<string, string>();
            bool isInsideAtLeastOneZone = false;
            var breachedZones = new List<SafeZone>();

            if (activeZones.Any())
            {
                foreach (var zone in activeZones)
                {
                    bool isInsideThisZone = false;
                    try
                    {
                        var geom = JsonSerializer.Deserialize<GeofenceGeometryDto>(zone.Geometry, _jsonOptions);
                        if (geom != null)
                        {
                            if (zone.Type == GeofenceType.Circle)
                            {
                                if (geom.Center != null && geom.Radius.HasValue)
                                {
                                    double dist = CalculateHaversineDistance(
                                        dto.Lat, dto.Lng,
                                        geom.Center.Lat, geom.Center.Lng);
                                    isInsideThisZone = dist <= geom.Radius.Value;
                                }
                            }
                            else if (zone.Type == GeofenceType.Polygon)
                            {
                                if (geom.Points != null && geom.Points.Any())
                                {
                                    isInsideThisZone = IsPointInPolygon(dto.Lat, dto.Lng, geom.Points);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error parsing geometry for safe zone {zone.Id}: {ex.Message}");
                    }

                    evaluatedZonesResult[$"sz_{zone.Id}"] = isInsideThisZone ? "INSIDE" : "OUTSIDE";
                    if (isInsideThisZone)
                    {
                        isInsideAtLeastOneZone = true;
                    }
                    else
                    {
                        breachedZones.Add(zone);
                    }
                }
            }
            else
            {
                // Default: if no zones configured, patient is considered safe (INSIDE)
                isInsideAtLeastOneZone = true;
            }

            // 3. Process State Machine & Transitions
            var currentStatusStr = isInsideAtLeastOneZone ? "INSIDE" : "OUTSIDE";
            var geofenceStatus = isInsideAtLeastOneZone ? GeofenceStatus.INSIDE : GeofenceStatus.OUTSIDE;
            var alertsTriggered = new List<AlertTriggeredDto>();

            // Load tracked patient to perform updates
            var patientToUpdate = await _unitOfWork.GetRepository<Patient, int>().Get(patientId);
            if (patientToUpdate != null)
            {
                var previousStatus = patientToUpdate.GeofenceStatus;

                patientToUpdate.LastKnownLat = dto.Lat;
                patientToUpdate.LastKnownLng = dto.Lng;
                patientToUpdate.LastLocationUpdated = dto.Timestamp;
                patientToUpdate.GeofenceStatus = geofenceStatus;

                _unitOfWork.GetRepository<Patient, int>().Update(patientToUpdate);

                // Transition: INSIDE -> OUTSIDE
                if (previousStatus == GeofenceStatus.INSIDE && geofenceStatus == GeofenceStatus.OUTSIDE)
                {
                    int notifiedContacts = await SendGeofenceBreachAlertsAsync(patient, breachedZones, dto.Lat, dto.Lng, dto.Timestamp);

                    foreach (var zone in breachedZones)
                    {
                        var violation = new GeofenceViolation
                        {
                            PatientId = patientId,
                            SafeZoneId = zone.Id,
                            ExitLat = dto.Lat,
                            ExitLng = dto.Lng,
                            ExitedAt = dto.Timestamp,
                            EnteredAt = null,
                            Status = ViolationStatus.ACTIVE,
                            NotificationSent = true
                        };
                        await _unitOfWork.GetRepository<GeofenceViolation, int>().AddAsync(violation);

                        alertsTriggered.Add(new AlertTriggeredDto
                        {
                            SafeZoneId = $"sz_{zone.Id}",
                            AlertType = "BREACH",
                            NotifiedContactsCount = notifiedContacts
                        });
                    }
                }
                // Transition: OUTSIDE -> INSIDE
                else if (previousStatus == GeofenceStatus.OUTSIDE && geofenceStatus == GeofenceStatus.INSIDE)
                {
                    await SendSafeReturnNotificationsAsync(patient, dto.Timestamp);

                    // Resolve active violations
                    var activeViolations = await _unitOfWork.GetRepository<GeofenceViolation, int>()
                        .GetTableNoTracking()
                        .Where(v => v.PatientId == patientId && v.Status == ViolationStatus.ACTIVE)
                        .ToListAsync();

                    var violationRepo = _unitOfWork.GetRepository<GeofenceViolation, int>();
                    foreach (var v in activeViolations)
                    {
                        var violationToUpdate = await violationRepo.Get(v.Id);
                        if (violationToUpdate != null)
                        {
                            violationToUpdate.EnteredAt = dto.Timestamp;
                            violationToUpdate.Status = ViolationStatus.RESOLVED;
                            violationRepo.Update(violationToUpdate);
                        }
                    }
                }
            }

            // 4. Log to Location History
            var history = new LocationHistory
            {
                PatientId = patientId,
                Lat = dto.Lat,
                Lng = dto.Lng,
                RecordedAt = dto.Timestamp
            };
            await _unitOfWork.GetRepository<LocationHistory, int>().AddAsync(history);

            // 5. Save Changes
            await _unitOfWork.CompleteAsync();

            return new LocationSubmitResponseDto
            {
                PatientId = $"p_{patientId}",
                CurrentStatus = currentStatusStr,
                EvaluatedZones = evaluatedZonesResult,
                AlertsTriggered = alertsTriggered
            };
        }

        public async Task<PatientLastLocationDto> GetLastKnownLocationAsync(int patientId)
        {
            var patient = await _unitOfWork.GetRepository<Patient, int>()
                .GetTableNoTracking()
                .FirstOrDefaultAsync(p => p.Id == patientId);

            if (patient == null)
            {
                throw new KeyNotFoundException($"Patient with ID {patientId} not found.");
            }

            var activeViolations = await _unitOfWork.GetRepository<GeofenceViolation, int>()
                .GetTableNoTracking()
                .Include(v => v.SafeZone)
                .Where(v => v.PatientId == patientId && v.Status == ViolationStatus.ACTIVE)
                .ToListAsync();

            var lastKnown = patient.LastKnownLat.HasValue && patient.LastKnownLng.HasValue && patient.LastLocationUpdated.HasValue
                ? new LastKnownLocationDto
                {
                    Lat = patient.LastKnownLat.Value,
                    Lng = patient.LastKnownLng.Value,
                    UpdatedAt = patient.LastLocationUpdated.Value
                }
                : null;

            return new PatientLastLocationDto
            {
                PatientId = $"p_{patientId}",
                Name = $"{patient.FName} {patient.LName}",
                LastKnownLocation = lastKnown,
                GeofenceStatus = patient.GeofenceStatus.ToString(),
                ActiveBreaches = activeViolations.Select(v => new ActiveBreachDto
                {
                    SafeZoneId = $"sz_{v.SafeZoneId}",
                    ZoneName = v.SafeZone.Name,
                    ExitedAt = v.ExitedAt
                }).ToList()
            };
        }

        public async Task<IEnumerable<LocationHistoryDto>> GetLocationHistoryAsync(int patientId, DateTime from, DateTime to, int limit = 100)
        {
            var history = await _unitOfWork.GetRepository<LocationHistory, int>()
                .GetTableNoTracking()
                .Where(h => h.PatientId == patientId && h.RecordedAt >= from && h.RecordedAt <= to)
                .OrderBy(h => h.RecordedAt)
                .Take(limit)
                .ToListAsync();

            return history.Select(h => new LocationHistoryDto
            {
                Lat = h.Lat,
                Lng = h.Lng,
                RecordedAt = h.RecordedAt
            });
        }

        public async Task<IEnumerable<GeofenceViolationDto>> GetViolationsAsync(int patientId)
        {
            var violations = await _unitOfWork.GetRepository<GeofenceViolation, int>()
                .GetTableNoTracking()
                .Include(v => v.SafeZone)
                .Where(v => v.PatientId == patientId)
                .OrderByDescending(v => v.ExitedAt)
                .ToListAsync();

            return violations.Select(v => new GeofenceViolationDto
            {
                ViolationId = $"v_{v.Id}",
                SafeZoneId = $"sz_{v.SafeZoneId}",
                ZoneName = v.SafeZone.Name,
                ExitedAt = v.ExitedAt,
                EnteredAt = v.EnteredAt,
                DurationMinutes = v.EnteredAt.HasValue 
                    ? (int?)Math.Ceiling((v.EnteredAt.Value - v.ExitedAt).TotalMinutes) 
                    : null,
                Status = v.Status.ToString()
            });
        }

        #region Geofencing Mathematical Algorithms

        private double CalculateHaversineDistance(double lat1, double lng1, double lat2, double lng2)
        {
            const double EarthRadiusMeters = 6371000.0;

            double dLat = (lat2 - lat1) * Math.PI / 180.0;
            double dLng = (lng2 - lng1) * Math.PI / 180.0;

            double rLat1 = lat1 * Math.PI / 180.0;
            double rLat2 = lat2 * Math.PI / 180.0;

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(rLat1) * Math.Cos(rLat2) *
                       Math.Sin(dLng / 2) * Math.Sin(dLng / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return EarthRadiusMeters * c;
        }

        private bool IsPointInPolygon(double lat, double lng, List<CenterPoint> polygonPoints)
        {
            bool inside = false;
            int count = polygonPoints.Count;
            if (count < 3) return false;

            int j = count - 1;
            for (int i = 0; i < count; i++)
            {
                var pi = polygonPoints[i];
                var pj = polygonPoints[j];

                bool intersect = ((pi.Lng > lng) != (pj.Lng > lng)) &&
                    (lat < (pj.Lat - pi.Lat) * (lng - pi.Lng) / (pj.Lng - pi.Lng) + pi.Lat);

                if (intersect)
                {
                    inside = !inside;
                }
                j = i;
            }

            return inside;
        }

        #endregion

        #region Notifications & High-Priority Alerts Sending Simulators

        private async Task<int> SendGeofenceBreachAlertsAsync(Patient patient, List<SafeZone> breachedZones, double lat, double lng, DateTime exitedAt)
        {
            int contactsAlerted = 0;
            string patientName = $"{patient.FName} {patient.LName}";
            string zonesList = string.Join(", ", breachedZones.Select(z => z.Name));

            // Log FCM High-Priority Push Alerts Details
            Console.WriteLine("========================================");
            Console.WriteLine("🚨 HIGH-PRIORITY GEOFENCE BREACH ALERT 🚨");
            Console.WriteLine($"Patient: {patientName} (ID: {patient.Id}) left safe zone(s): {zonesList}");
            Console.WriteLine($"Location: Lat {lat}, Lng {lng} at {exitedAt}");
            Console.WriteLine("========================================");

            // 1. Notify Caregiver via FCM Push Notification
            if (patient.Caregiver != null)
            {
                string caregiverFcm = patient.Caregiver.FcmToken ?? "dummy_token_caregiver_123";
                Console.WriteLine($"[FCM Push] Sent to Caregiver '{patient.Caregiver.FName} {patient.Caregiver.LName}' (Token: {caregiverFcm})");
                Console.WriteLine($"Body: Emergency: {patientName} left {zonesList}. Tap to track real-time location.");
                contactsAlerted++;
            }

            // 2. Notify Relatives via FCM Push Notification
            foreach (var pr in patient.PatientRelatives)
            {
                if (pr.Relative != null)
                {
                    string relativeFcm = pr.Relative.FcmToken ?? "dummy_token_relative_456";
                    Console.WriteLine($"[FCM Push] Sent to Relative '{pr.Relative.FName} {pr.Relative.LName}' (Token: {relativeFcm})");
                    Console.WriteLine($"Body: Emergency: {patientName} left {zonesList}. Tap to track real-time location.");
                    contactsAlerted++;
                }
            }

            // 3. Emergency Contact SMS Fallback Simulator (Twilio / Infobip)
            foreach (var zone in breachedZones)
            {
                if (!string.IsNullOrEmpty(zone.Phone))
                {
                    Console.WriteLine($"[SMS Fallback] Sent SMS to Emergency Phone: {zone.Phone}");
                    Console.WriteLine($"Text: NESYAN ALERT: Patient {patientName} has left the safe zone {zone.Name} at {exitedAt}. Track them at: http://maps.google.com/maps?q={lat},{lng}");
                    contactsAlerted++;
                }
            }

            // Return notified count (Signal simulated completion)
            await Task.Delay(50); // Simulate network overhead
            return contactsAlerted;
        }

        private async Task SendSafeReturnNotificationsAsync(Patient patient, DateTime enteredAt)
        {
            string patientName = $"{patient.FName} {patient.LName}";
            Console.WriteLine("========================================");
            Console.WriteLine("💚 Patient Returned to Safe Zone 💚");
            Console.WriteLine($"Patient: {patientName} has returned inside safe zone boundaries at {enteredAt}");
            Console.WriteLine("========================================");

            if (patient.Caregiver != null)
            {
                Console.WriteLine($"[FCM Silent Info] Sent to Caregiver '{patient.Caregiver.FName} {patient.Caregiver.LName}' - Return resolved.");
            }

            foreach (var pr in patient.PatientRelatives)
            {
                if (pr.Relative != null)
                {
                    Console.WriteLine($"[FCM Silent Info] Sent to Relative '{pr.Relative.FName} {pr.Relative.LName}' - Return resolved.");
                }
            }

            await Task.Delay(50); // Simulate network overhead
        }

        #endregion

        #region Helpers & Mapping

        private SafeZoneDto MapToSafeZoneDto(SafeZone safeZone)
        {
            GeofenceGeometryDto geometry = new();
            try
            {
                geometry = JsonSerializer.Deserialize<GeofenceGeometryDto>(safeZone.Geometry, _jsonOptions) ?? new();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deserializing geometry for safe zone {safeZone.Id}: {ex.Message}");
            }

            return new SafeZoneDto
            {
                SafeZoneId = $"sz_{safeZone.Id}",
                PatientId = safeZone.PatientId,
                Name = safeZone.Name,
                Phone = safeZone.Phone,
                Type = (int)safeZone.Type,
                Geometry = geometry,
                IsActive = safeZone.IsActive,
                CreatedAt = safeZone.CreatedOn
            };
        }

        #endregion
    }
}
