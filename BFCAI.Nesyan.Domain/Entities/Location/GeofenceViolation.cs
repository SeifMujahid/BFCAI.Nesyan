using BFCAI.Nesyan.Domain.Entities.Common;
using BFCAI.Nesyan.Domain.Entities.Primary.Patients;
using System;

namespace BFCAI.Nesyan.Domain.Entities.Location
{
    public enum ViolationStatus
    {
        ACTIVE,
        RESOLVED
    }

    public class GeofenceViolation : BaseEntity<int>
    {
        public int PatientId { get; set; }
        public Patient Patient { get; set; } = null!;

        public int SafeZoneId { get; set; }
        public SafeZone SafeZone { get; set; } = null!;

        public double ExitLat { get; set; }
        public double ExitLng { get; set; }
        public DateTime ExitedAt { get; set; }
        public DateTime? EnteredAt { get; set; }
        public ViolationStatus Status { get; set; } = ViolationStatus.ACTIVE;
        public bool NotificationSent { get; set; }
    }
}
