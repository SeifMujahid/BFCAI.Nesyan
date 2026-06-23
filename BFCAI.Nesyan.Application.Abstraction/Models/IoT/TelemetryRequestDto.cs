using System;

namespace BFCAI.Nesyan.Application.Abstraction.Models.IoT
{
    public class TelemetryRequestDto
    {
        public int PatientId { get; set; }
        public double Hr { get; set; }
        public double Spo2 { get; set; }
        public int Steps { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
