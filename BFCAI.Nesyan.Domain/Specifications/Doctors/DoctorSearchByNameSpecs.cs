using BFCAI.Nesyan.Domain.Entities.Primary.Doctors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Domain.Specifications.Doctors
{
    public class DoctorSearchByNameSpecs:BaseSpecifications<Doctor,int>
    {
        public DoctorSearchByNameSpecs(string name)
        {
            Criteria = d =>(d.FName + " " + d.LName).Contains(name);
        }
    }
}
