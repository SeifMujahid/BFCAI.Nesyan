using System;

namespace BFCAI.Nesyan.Application.Abstraction.Models.MindGames
{
    public class PatternGameRecordDto
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string PatternLevel { get; set; } = null!;
        public DateTime DateTime { get; set; }
        public int Score { get; set; }
        public int Rounds { get; set; }
        public int Time { get; set; }
    }
}
