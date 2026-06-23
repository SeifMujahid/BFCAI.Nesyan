using BFCAI.Nesyan.Domain.Entities.Relations;
using BFCAI.Nesyan.Domain.Entities.Relations.Primary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Domain.Entities.Primary.Relatives
{
    public class Relative:User
    {
        public IEnumerable<PatientRelative>Patients { get; set; }=new List<PatientRelative>();
    }
}
