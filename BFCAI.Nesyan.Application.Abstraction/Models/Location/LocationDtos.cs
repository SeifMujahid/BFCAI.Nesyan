using System;
using System.Collections.Generic;

namespace BFCAI.Nesyan.Application.Abstraction.Models.Location
{
    public class LocationSubmitDto
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public class AlertTriggeredDto
    {
        public string SafeZoneId { get; set; } = null!;
        public string AlertType { get; set; } = "BREACH";
        public int NotifiedContactsCount { get; set; }
    }

    public class LocationSubmitResponseDto
    {
        public string PatientId { get; set; } = null!;
        public string CurrentStatus { get; set; } = null!; // INSIDE / OUTSIDE
        public Dictionary<string, string> EvaluatedZones { get; set; } = new();
        public List<AlertTriggeredDto> AlertsTriggered { get; set; } = new();
    }

    public class LastKnownLocationDto
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ActiveBreachDto
    {
        public string SafeZoneId { get; set; } = null!;
        public string ZoneName { get; set; } = null!;
        public DateTime ExitedAt { get; set; }
    }

    public class PatientLastLocationDto
    {
        public string PatientId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public LastKnownLocationDto? LastKnownLocation { get; set; }
        public string GeofenceStatus { get; set; } = null!; // INSIDE / OUTSIDE
        public List<ActiveBreachDto> ActiveBreaches { get; set; } = new();
    }

    public class LocationHistoryDto
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
        public DateTime RecordedAt { get; set; }
    }

    public class GeofenceViolationDto
    {
        public string ViolationId { get; set; } = null!;
        public string SafeZoneId { get; set; } = null!;
        public string ZoneName { get; set; } = null!;
        public DateTime ExitedAt { get; set; }
        public DateTime? EnteredAt { get; set; }
        public int? DurationMinutes { get; set; }
        public string Status { get; set; } = null!; // ACTIVE / RESOLVED
    }
}
