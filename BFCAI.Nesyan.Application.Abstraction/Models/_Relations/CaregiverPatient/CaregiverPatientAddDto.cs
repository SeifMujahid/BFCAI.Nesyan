using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Abstraction.Models._Relations.CaregiverPatient
{
    public class CaregiverPatientAddDto
    {
        public string NationalIdcaregavier { get; set; } = null!;
        public string Emailcaregavier { get; set; } = null!;
        public int PatientId { get; set; }
    }
}
