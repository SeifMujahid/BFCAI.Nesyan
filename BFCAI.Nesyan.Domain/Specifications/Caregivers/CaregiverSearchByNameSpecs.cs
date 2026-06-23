using BFCAI.Nesyan.Domain.Entities.Primary.Caregivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Domain.Specifications.Caregivers
{
    public class CaregiverSearchByNameSpecs:BaseSpecifications<Caregiver,int>
    {
        public CaregiverSearchByNameSpecs(string name)
        {
            Criteria = d => (d.FName + " " + d.LName).Contains(name);
        }
    }
}
