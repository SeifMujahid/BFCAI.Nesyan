using BFCAI.Nesyan.Domain.Entities.Common;
using BFCAI.Nesyan.Domain.Entities.MindGames;
using BFCAI.Nesyan.Domain.Entities.Primary.Doctor;
using BFCAI.Nesyan.Domain.Entities.Primary.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Domain.Entities.Relations.MindGames
{
    public class MindGameSession:BaseAuditableEntity<int>
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; } = null!;

        public int PatientId { get; set; }
        public Patient Patient { get; set; } = null!;

        public int MindGameId { get; set; }
        public MindGame MindGame { get; set; } = null!;

        public DateTime AddedDate { get; set; }
        public DateTime StartDate { get; set; }
        public string Frequency { get; set; } = null!;
    }
}
