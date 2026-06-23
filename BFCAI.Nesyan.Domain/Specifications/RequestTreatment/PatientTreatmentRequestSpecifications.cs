using BFCAI.Nesyan.Domain.Entities.Relations.Primary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Domain.Specifications.RequestTreatment
{
    public class PatientTreatmentRequestSpecifications : BaseSpecifications<TreatmentRequest, int>
    {
        public PatientTreatmentRequestSpecifications(int patientId, int actorType, int? orderType)
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
                Criteria = r => r.PatientId == patientId && r.DoctorId != null && r.Status == status;
                AddStringinclude("Doctor");
            }
            else
            {
                Criteria = r => r.PatientId == patientId && r.CaregiverId != null && r.Status == status;
                AddStringinclude("Caregiver");
            }
            AddStringinclude("Patient.Assessments");
            AddStringinclude("Relative");
        }
    }
}
