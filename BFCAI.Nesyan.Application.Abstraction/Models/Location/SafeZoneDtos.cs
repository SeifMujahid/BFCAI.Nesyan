using System;

namespace BFCAI.Nesyan.Application.Abstraction.Models.Location
{
    public class SafeZoneDto
    {
        public string SafeZoneId { get; set; } = null!;
        public int PatientId { get; set; }
        public string Name { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public int Type { get; set; } // 0 = Polygon, 1 = Circle
        public GeofenceGeometryDto Geometry { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateSafeZoneDto
    {
        public string Name { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public int Type { get; set; } // 0 = Polygon, 1 = Circle
        public GeofenceGeometryDto Geometry { get; set; } = null!;
    }

    public class UpdateSafeZoneDto
    {
        public string Name { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public int Type { get; set; } // 0 = Polygon, 1 = Circle
        public GeofenceGeometryDto Geometry { get; set; } = null!;
        public bool IsActive { get; set; }
    }

    public class SafeZoneResponseDto
    {
        public string SafeZoneId { get; set; } = null!;
        public string Status { get; set; } = null!; // ACTIVE / UPDATED
        public string Message { get; set; } = null!;
    }

    public class DeleteSafeZoneResponseDto
    {
        public string Status { get; set; } = "DELETED";
        public string Message { get; set; } = "Safe zone removed successfully.";
    }
}
