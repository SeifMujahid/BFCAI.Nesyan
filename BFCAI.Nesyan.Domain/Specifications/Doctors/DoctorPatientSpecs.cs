using BFCAI.Nesyan.Domain.Entities.Primary.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Domain.Specifications.Doctors
{
    public class DoctorPatientSpecs : BaseSpecifications<Patient, int>
    {
        public DoctorPatientSpecs(int doctorId, int patientId)
        {
            Criteria = P => P.Id == patientId && P.DoctorId == doctorId;
            Includes.Add(P => P.Doctor!);
        }
    }
}
