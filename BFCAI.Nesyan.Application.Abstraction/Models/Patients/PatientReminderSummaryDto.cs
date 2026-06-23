using System;

namespace BFCAI.Nesyan.Application.Abstraction.Models.Patients
{
    public class PatientReminderSummaryDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string? Name { get; set; }

        public string? Dosage { get; set; }

        public string Type { get; set; } = null!; // Medication, Appointment, Routine

        public DateOnly ReminderDate { get; set; }

        public TimeOnly ReminderTime { get; set; }

        public string Frequency { get; set; } = null!;

        public string? Notes { get; set; }
    }
}
