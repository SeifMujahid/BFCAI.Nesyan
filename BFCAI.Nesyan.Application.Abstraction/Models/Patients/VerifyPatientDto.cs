using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Abstraction.Models.Patients
{
    public class VerifyPatientDto
    {
        public string NationalId { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
