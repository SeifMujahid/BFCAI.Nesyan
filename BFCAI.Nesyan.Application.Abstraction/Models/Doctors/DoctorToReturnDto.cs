using BFCAI.Nesyan.Application.Abstraction.Models.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Abstraction.Models.Doctors
{
    public class DoctorToReturnDto
    {
        public int Id { get; set; }
        public string FName { get; set; } = null!;
        public string LName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string City { get; set; } = null!;
        public int Age { get; set; }
        public string GraduationDegree { get; set; } = null!;
        public string MedicalAssociationCard { get; set; } = null!;
        public ICollection<PatientSummaryDto>? Patients { get; set; }
    }
}
