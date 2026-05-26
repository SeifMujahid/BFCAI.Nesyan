using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Abstraction.Models.Patients
{
    public class PatientSummaryV2Dto
    {
        public int PatientId { get; set; }
        public string FullName { get; set; } = null!;
        public int Age { get; set; }
        public string? ImageUrl { get; set; }
        public string Gender { get; set; } = null!;

    }
}
