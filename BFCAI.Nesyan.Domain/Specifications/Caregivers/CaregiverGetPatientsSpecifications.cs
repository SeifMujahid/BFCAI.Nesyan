using BFCAI.Nesyan.Domain.Entities.Primary.Caregivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Domain.Specifications.Caregivers
{
    public class CaregiverGetPatientsSpecifications:BaseSpecifications<Caregiver,int>
    {
        public CaregiverGetPatientsSpecifications(int id):base(id)
        {
            Includes.Add(P => P.Patients);
        }
    }
}
