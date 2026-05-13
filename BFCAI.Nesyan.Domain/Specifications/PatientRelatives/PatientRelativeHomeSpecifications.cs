using BFCAI.Nesyan.Domain.Entities.Relations.Primary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Domain.Specifications.PatientRelatives
{
    public class PatientRelativeHomeSpecifications:RelativePatientCheckSpecifications
    {
        public PatientRelativeHomeSpecifications(int relativeId):base(relativeId)
        {

            AddIncludes();
        }
        public PatientRelativeHomeSpecifications(int relativeId, int patientId):base(relativeId, patientId)
        {
            Criteria = pr => pr.RelativeId == relativeId &&
                             pr.PatientId == patientId;
            AddStringinclude("Relative");
            AddStringinclude("Patient.PatientTelemetries");
            AddStringinclude("Patient.Assessments");
        }
        private protected override void AddIncludes()
        {
            base.AddIncludes();
            Includes.Add(pr => pr.Relative);
            Includes.Add(pr => pr.Patient);
        }
    }
}
