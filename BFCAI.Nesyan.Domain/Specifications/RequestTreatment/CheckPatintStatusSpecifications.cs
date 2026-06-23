using BFCAI.Nesyan.Domain.Entities.Relations.Primary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Domain.Specifications.RequestTreatment
{
    public class CheckPatintStatusSpecifications : BaseSpecifications<TreatmentRequest, int>
    {
        public CheckPatintStatusSpecifications(int patientId, int actorType)
        {
            if (actorType == 1)
                Criteria = P => P.PatientId == patientId && P.DoctorId != null && P.Status == RequestStatus.Selected;
            else
                Criteria = P => P.PatientId == patientId && P.CaregiverId != null && P.Status == RequestStatus.Selected;
        }
    }
}
