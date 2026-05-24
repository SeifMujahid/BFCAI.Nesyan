using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Abstraction.Models._Relations.RelativePatient
{
    public class RelativePatientToCreateDto
    {
        public int RelativeId { get; set; }

        public int PatientId { get; set; }
    }
}
