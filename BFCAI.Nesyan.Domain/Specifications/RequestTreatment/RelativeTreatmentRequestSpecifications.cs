using BFCAI.Nesyan.Domain.Entities.Primary.Doctors;
using BFCAI.Nesyan.Domain.Entities.Primary.Patients;
using BFCAI.Nesyan.Domain.Entities.Relations.Primary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Domain.Specifications.RequestTreatment
{
    public class RelativeTreatmentRequestSpecifications:BaseSpecifications<TreatmentRequest,int>
    {
        public RelativeTreatmentRequestSpecifications(int relativeId,int actorType, int? orderType )
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
            if (actorType == 1)
            {
                Criteria = r => r.RelativeId == relativeId && r.DoctorId != null && r.Status == status;
                AddStringinclude("Doctor");
            }
            else
            {
                Criteria = r => r.RelativeId == relativeId && r.CaregiverId != null && r.Status == status;
                AddStringinclude("Caregiver");
            }
            AddStringinclude("Patient.Assessments");
            AddStringinclude("Relative");
        }
    }
}
