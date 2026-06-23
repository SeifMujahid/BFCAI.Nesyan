using BFCAI.Nesyan.Domain.Entities.Common;
using BFCAI.Nesyan.Domain.Entities.Primary.Patients;
using System;
using System.Collections.Generic;

namespace BFCAI.Nesyan.Domain.Entities.Location
{
    public enum GeofenceType
    {
        Polygon = 0,
        Circle = 1
    }

    public class SafeZone : BaseAuditableEntity<int>
    {
        public int PatientId { get; set; }
        public Patient Patient { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public GeofenceType Type { get; set; }
        public string Geometry { get; set; } = null!; // Serialized JSON string
        public bool IsActive { get; set; } = true;

        public ICollection<GeofenceViolation> GeofenceViolations { get; set; } = new List<GeofenceViolation>();
    }
}
