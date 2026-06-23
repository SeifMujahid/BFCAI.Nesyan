using BFCAI.Nesyan.Domain.Entities.Primary.Relatives;
using BFCAI.Nesyan.Domain.Entities.Relations.Primary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Domain.Specifications.RequestTreatment
{
    public class DoctorOrCaregiverTreatmentRequestsSpecifications : BaseSpecifications<TreatmentRequest, int>
    {
        public DoctorOrCaregiverTreatmentRequestsSpecifications(int actorId, int? orderType, int actorType)
        {
            var status = orderType switch
            {
                1 => RequestStatus.Rejected,
                2 => RequestStatus.Pending,
                3 => RequestStatus.Accepted,
                4 => RequestStatus.Selected,
                5 => RequestStatus.RemovalPending,
                _ => RequestStatus.Pending
            };
            Criteria = r => (actorType == 1 ? r.DoctorId == actorId : r.CaregiverId == actorId) && r.Status == status;
            AddStringinclude(actorType == 1 ? "Doctor" : "Caregiver");
            AddStringinclude("Patient.Assessments");
            AddStringinclude("Relative");
        }
    }
}
