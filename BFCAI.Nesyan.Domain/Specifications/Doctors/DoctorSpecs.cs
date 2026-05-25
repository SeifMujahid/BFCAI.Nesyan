using BFCAI.Nesyan.Domain.Entities.Primary.Doctors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Domain.Specifications.Doctors
{
    public class DoctorSpecs:BaseSpecifications<Doctor,int>
    {
        public DoctorSpecs()
        {

            AddIncludes();
        }
        public DoctorSpecs(int Id):base(Id)
        {
            AddIncludes();
        }
        private protected override void AddIncludes()
        {
            base.AddIncludes();

            Includes.Add(P => P.Patients);
        }
    }
}
