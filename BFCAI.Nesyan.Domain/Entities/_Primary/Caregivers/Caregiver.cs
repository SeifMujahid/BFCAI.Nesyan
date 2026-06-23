using BFCAI.Nesyan.Domain.Entities.Medications;
using BFCAI.Nesyan.Domain.Entities.Primary.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Domain.Entities.Primary.Caregivers
{
    public class Caregiver : User
    {
        public ICollection<Patient> Patients { get; set; } = new List<Patient>();
    }
}
