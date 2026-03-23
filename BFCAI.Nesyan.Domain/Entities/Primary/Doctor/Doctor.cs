using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Domain.Entities.Primary.Doctor
{
    public class Doctor:User
    {
        public string GraduationDegree { get; set; } = null!;
        public string MedicalAssociationCard { get; set; } = null!;
    }
}
