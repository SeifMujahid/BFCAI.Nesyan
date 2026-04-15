using BFCAI.Nesyan.Domain.Entities.Primary.Doctor;
using BFCAI.Nesyan.Domain.Entities.Primary.Patient;
using BFCAI.Nesyan.Domain.Entities.Primary.Relative;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Domain.Entities.Relations.Primary
{
    public class PatientDoctor
    {
        public int PatientId { get; set; }
        public Patient Patient { get; set; } = null!;
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; } = null!;

        public DateTime EnrollmentDate { get; set; }
    }
}
