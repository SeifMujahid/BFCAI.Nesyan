using BFCAI.Nesyan.Domain.Entities.Common;
using BFCAI.Nesyan.Domain.Entities.Primary.Doctors;
using BFCAI.Nesyan.Domain.Entities.Primary.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Domain.Entities.Medications
{
    public enum ReminderType
    {
        Medication = 1,
        Appointment = 2,
        Routine = 3
    }
    public enum ReminderFrequency
    {
        OneTime = 1,
        Daily = 2,
        Weekly = 3,
        Monthly = 4,
        Custom = 5
    }
    public class Medication : BaseAuditableEntity<int>
    {
        public ReminderType Type { get; set; }

        public string Title { get; set; } = null!;

        public string? Name { get; set; }
        // Medication only
        public string? Dosage { get; set; }

        // Appointment only
        public string? Location { get; set; }

        // Appointment only
        public string? Specialty { get; set; }

        public ReminderFrequency Frequency { get; set; }

        public DateOnly ReminderDate { get; set; }

        public TimeOnly ReminderTime { get; set; }

        public string? Notes { get; set; }

        public int PatientId { get; set; }

        public Patient Patient { get; set; } = null!;

    }
}

