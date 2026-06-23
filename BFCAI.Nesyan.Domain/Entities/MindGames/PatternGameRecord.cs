using BFCAI.Nesyan.Domain.Entities.Common;
using BFCAI.Nesyan.Domain.Entities.Primary.Patients;
using System;

namespace BFCAI.Nesyan.Domain.Entities.MindGames
{
    public class PatternGameRecord : BaseAuditableEntity<int>
    {
        public int PatientId { get; set; }
        public Patient Patient { get; set; } = null!;
        public PatternLevel PatternLevel { get; set; }
        public DateTime DateTime { get; set; }
        public int Score { get; set; }
        public int Rounds { get; set; }
        public int Time { get; set; }
    }
}
