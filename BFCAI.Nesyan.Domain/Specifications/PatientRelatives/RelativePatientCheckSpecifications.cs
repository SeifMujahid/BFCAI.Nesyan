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
    public class RelativePatientCheckSpecifications:BaseSpecifications<PatientRelative,int>
    {
        public RelativePatientCheckSpecifications(int relativeId):base(relativeId)
        {
            
        }
        public RelativePatientCheckSpecifications(int relativeId, int patientId)
        {
            Criteria = pr => pr.RelativeId == relativeId &&
             pr.PatientId == patientId;
        }
    }
}
