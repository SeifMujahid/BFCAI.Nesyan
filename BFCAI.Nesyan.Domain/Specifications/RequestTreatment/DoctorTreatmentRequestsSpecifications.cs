using BFCAI.Nesyan.Domain.Entities.Primary.Relatives;
using BFCAI.Nesyan.Domain.Entities.Relations.Primary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Domain.Specifications.RequestTreatment
{
    public class DoctorTreatmentRequestsSpecifications:BaseSpecifications<RelativeDoctorRequest,int>
    {
        public DoctorTreatmentRequestsSpecifications(int doctorId,int? orderType)
        {
            switch (orderType)
            {
                case 1:
                    Criteria = P => P.DoctorId == doctorId && P.Status == RequestStatus.Pending;
                    break;
                case 2:
                    Criteria = P => P.DoctorId == doctorId && P.Status == RequestStatus.Selected;
                    break;
                case 3:
                    Criteria = P => P.DoctorId == doctorId && P.Status == RequestStatus.Rejected;
                    break;
                case 4:
                    Criteria = P => P.DoctorId == doctorId && P.Status == RequestStatus.DoctorRemovalPending;
                    break;
                default:
                    Criteria = P => P.DoctorId == doctorId && P.Status == RequestStatus.Accepted;
                    break;
            }
            AddStringinclude("Doctor");
            AddStringinclude("Patient.Assessments");
            AddStringinclude("Relative");
        }
    }
}
