using BFCAI.Nesyan.Domain.Entities.Alerts;
using BFCAI.Nesyan.Domain.Entities.Primary.Patient;
using BFCAI.Nesyan.Domain.Entities.Primary.Relative;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Domain.Entities.Relations.Alerts
{
    public class PatientRelativeAlert
    {
        public int PatientId { get; set; }
        public Patient Patient { get; set; } = null!;

        public int RelativeId { get; set; }
        public Relative Relative { get; set; } = null!;

        public int AlertId { get; set; }
        public Alert Alert { get; set; } = null!;

        public DateTime AddedDate { get; set; }
        public string Notes { get; set; } = null!;
    }
}
