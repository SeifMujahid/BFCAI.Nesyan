using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BFCAI.Nesyan.Domain.Entities.Primary.Patients;

namespace BFCAI.Nesyan.Domain.Entities.Primary.Doctors
{
    public class Doctor:User
    {
        public string GraduationDegree { get; set; } = null!;
        public string MedicalAssociationCard { get; set; } = null!;
        public string Specialization { get; set; } = null!;
        public ICollection<Patient> Patients { get; set; } = new List<Patient>();


    }
}
