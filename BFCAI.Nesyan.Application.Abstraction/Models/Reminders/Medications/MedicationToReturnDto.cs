using System;

namespace BFCAI.Nesyan.Application.Abstraction.Models.Reminders.Medications
{
    public class MedicationToReturnDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Dosage { get; set; } = null!;

        public DateOnly ReminderDate { get; set; }

        public TimeOnly ReminderTime { get; set; }

        public string Frequency { get; set; } = null!;

        public string? Notes { get; set; }

        public bool IsCompleted { get; set; }

    }
}
