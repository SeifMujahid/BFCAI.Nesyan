using BFCAI.Nesyan.Domain.Entities.Primary.Caregivers;
using BFCAI.Nesyan.Domain.Entities.Primary.Patients;
using BFCAI.Nesyan.Domain.Entities.Primary.Relatives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Domain.Specifications.Caregivers
{
    public class CaregiverMainSpecs:BaseSpecifications<Patient,int>
    {
        public CaregiverMainSpecs(int caregiverId,int patientId)
        {
            Criteria = Criteria = P => P.CaregiverId == caregiverId &&
                                  P.Id == patientId;
            Includes.Add(P => P.Caregiver!);
            Includes.Add(P => P.PatientTelemetries);
            Includes.Add(P => P.Assessments);

        }
        public CaregiverMainSpecs(int caregiverId, int patientId,int d)
        {
            Criteria = Criteria = P => P.CaregiverId == caregiverId &&
                                  P.Id == patientId;
            Includes.Add(P => P.Caregiver!);
            Includes.Add(P => P.Reminders);
        }
    }
}
