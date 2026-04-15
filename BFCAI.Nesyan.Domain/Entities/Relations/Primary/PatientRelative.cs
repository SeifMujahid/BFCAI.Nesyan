using BFCAI.Nesyan.Domain.Entities.Primary.Patient;
using BFCAI.Nesyan.Domain.Entities.Primary.Relative;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Domain.Entities.Relations.Primary
{
    public class PatientRelative
    {
        public int PatientId { get; set; }
        public Patient Patient { get; set; } = null!;
        public int RelativeId { get; set; }
        public Relative Relative { get; set; } = null!;

        public DateTime EnrollmentDate { get; set; }

    }
}
