using BFCAI.Nesyan.Domain.Entities.Primary.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Domain.Specifications.Patients
{
    public class PatientSpecifications:BaseSpecifications<Patient,int>
    {
        public PatientSpecifications(int id):base(id)
        {
            Includes.Add(P => P.PatientTelemetries);
            Includes.Add(P => P.Assessments);
        }
        public PatientSpecifications(int id,int t):base(id)
        {
            Includes.Add(P => P.Reminders);
        }
    }
}
