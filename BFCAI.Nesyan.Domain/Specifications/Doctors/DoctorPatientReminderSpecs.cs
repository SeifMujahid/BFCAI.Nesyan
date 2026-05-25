using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Domain.Specifications.Doctors
{
    public class DoctorPatientReminderSpecs:DoctorPatientSpecs
    {
        public DoctorPatientReminderSpecs(int doctorId,int patientId):base(doctorId, patientId)
        {
            AddStringinclude("Reminders");
        }
    }
}
