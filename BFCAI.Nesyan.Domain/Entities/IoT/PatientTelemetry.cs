using BFCAI.Nesyan.Domain.Entities.Common;
using BFCAI.Nesyan.Domain.Entities.Primary.Patients;
using System;

namespace BFCAI.Nesyan.Domain.Entities.IoT
{
    public class PatientTelemetry : BaseAuditableEntity<int>
    {
        public int PatientId { get; set; }
        public Patient? Patient { get; set; }

        public double Hr { get; set; }
        public double Spo2 { get; set; }
        public int Steps { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}
