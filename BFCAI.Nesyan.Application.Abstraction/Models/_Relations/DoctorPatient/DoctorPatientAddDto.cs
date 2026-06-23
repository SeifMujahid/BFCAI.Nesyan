using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Abstraction.Models._Relations.DoctorPatient
{
    public class DoctorPatientAddDto
    {
        public string NationalIdDoctor { get; set; } = null!;
        public string EmailDoctor { get; set; } = null!;
        public int PatientId { get; set; }
    }
}
