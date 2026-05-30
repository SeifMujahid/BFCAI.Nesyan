using BFCAI.Nesyan.Domain.Entities.Primary.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Domain.Specifications.Patients
{
    public class PatientSerachSpecifications:BaseSpecifications<Patient,int>
    {
        public PatientSerachSpecifications(int patientId)
        {
            Criteria = P => P.Id == patientId;
        }
        public PatientSerachSpecifications(string userName)
        {
            Criteria = P => P.UserName == userName;
        }
        public PatientSerachSpecifications(string nationalId, string email)
        {
            Criteria = P => P.NationalId == nationalId && P.Email == email;
        }
    }
}
