using System;

namespace BFCAI.Nesyan.Application.Abstraction.Models.MindGames
{
    public class PatientMindGameDto
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public int MindGameId { get; set; }
        public MindGameDto MindGame { get; set; } = null!;
        public DateTime AddedDate { get; set; }
        public DateTime StartDate { get; set; }
        public string Frequency { get; set; } = null!;
    }
}
