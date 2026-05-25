using BFCAI.Nesyan.Domain.Entities.Primary.Doctors;
using BFCAI.Nesyan.Domain.Entities.Relations.Primary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Domain.Specifications.RequestTreatment
{
    public class RelativeTreatmentRequestSpecifications:BaseSpecifications<RelativeDoctorRequest,int>
    {
        public RelativeTreatmentRequestSpecifications(int relativeId,int? orderType )
        {
            switch (orderType)
            {
                case 1:
                    Criteria = P => P.RelativeId == relativeId && P.Status==RequestStatus.Pending;
                    break;
                case 2:
                    Criteria = P => P.RelativeId == relativeId && P.Status == RequestStatus.Selected;
                    break;
                case 3: 
                    Criteria = P => P.RelativeId == relativeId && P.Status == RequestStatus.Rejected;
                    break;
                case 4:
                    Criteria = P => P.RelativeId == relativeId && P.Status == RequestStatus.DoctorRemovalPending;
                    break;
                default:
                    Criteria = P => P.RelativeId == relativeId && P.Status == RequestStatus.Accepted;
                    break;
            }
            AddStringinclude("Doctor");
            AddStringinclude("Patient.Assessments");
            AddStringinclude("Relative");
        }
    }
}
