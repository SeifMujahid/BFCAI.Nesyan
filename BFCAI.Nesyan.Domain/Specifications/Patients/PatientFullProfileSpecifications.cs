using BFCAI.Nesyan.Domain.Entities.Primary.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Domain.Specifications.Patients
{
    public class PatientFullProfileSpecifications : BaseSpecifications<Patient, int>
    {
        public PatientFullProfileSpecifications(int id) : base(id)
        {
            Includes.Add(p => p.PatientTelemetries);
            Includes.Add(p => p.Assessments);
            Includes.Add(p => p.Reminders);
            Includes.Add(p => p.Doctor!);
            Includes.Add(p => p.Caregiver!);
            AddStringinclude("PatientRelatives.Relative");
        }
    }
}
