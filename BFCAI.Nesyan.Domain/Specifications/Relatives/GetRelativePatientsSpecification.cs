using BFCAI.Nesyan.Domain.Entities.Primary.Relatives;
using BFCAI.Nesyan.Domain.Entities.Relations.Primary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Domain.Specifications.Relatives
{
    public class GetRelativePatientsSpecification:BaseSpecifications<Relative,int>
    {
        public GetRelativePatientsSpecification(int id):base(id)
        {
            AddStringinclude("Patients.Patient");
            AddStringinclude("Patients.Patient.Reminders");
        }
        public GetRelativePatientsSpecification(int relativeId,int patientId)
        {
            Criteria= r =>r.Id == relativeId &&
                          r.Patients
                          .Any(p =>p.PatientId == patientId); ;
            AddStringinclude("Patients.Patient");
            AddStringinclude("Patients.Patient.Reminders");
        }

    }
}
