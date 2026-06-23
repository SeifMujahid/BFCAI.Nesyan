using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Abstraction.Models.Doctors
{
    public class DoctorSummaryDto
    {
        public int DoctorId { get; set; }
        public string FullName { get; set; } = null!;
        public int Age { get; set; }
        public string Gender { get; set; } = null!;
        public string GraduationDegree { get; set; } = null!;
        public string MedicalAssociationCard { get; set; } = null!;
        public string Specialization { get; set; } = null!;
    }
}
