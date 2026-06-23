using BFCAI.Nesyan.Domain.Entities.Primary.Patients;
using BFCAI.Nesyan.Domain.Entities.Primary.Relatives;
using BFCAI.Nesyan.Domain.Entities.Relations.Primary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Domain.Specifications.PatientRelatives
{
    public class RelativePatientRemindersSpecifications:BaseSpecifications<PatientRelative,int>
    {
        public RelativePatientRemindersSpecifications(int relativeId, int patientId)
        {
            Criteria = pr => pr.RelativeId == relativeId &&
                  pr.PatientId == patientId;
            AddStringinclude("Relative");
            AddStringinclude("Patient.Reminders");

        }
    }
}
