using BFCAI.Nesyan.Domain.Entities.Common;
using BFCAI.Nesyan.Domain.Entities.Primary.Patients;
using System;

namespace BFCAI.Nesyan.Domain.Entities.Location
{
    public class LocationHistory : BaseEntity<int>
    {
        public int PatientId { get; set; }
        public Patient Patient { get; set; } = null!;
        public double Lat { get; set; }
        public double Lng { get; set; }
        public DateTime RecordedAt { get; set; } = DateTime.UtcNow;
    }
}
