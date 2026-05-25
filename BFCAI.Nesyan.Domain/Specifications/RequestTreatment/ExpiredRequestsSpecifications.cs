using BFCAI.Nesyan.Domain.Entities.Relations.Primary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Domain.Specifications.RequestTreatment
{
    public class ExpiredRequestsSpecifications:BaseSpecifications<RelativeDoctorRequest,int>
    {
        public ExpiredRequestsSpecifications()
        {
            Criteria= r =>
                r.Status == RequestStatus.DoctorRemovalPending &&
                r.RequestDate <= DateTime.UtcNow.AddDays(-7);
        }
    }
}
