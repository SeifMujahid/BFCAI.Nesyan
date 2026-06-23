using BFCAI.Nesyan.Domain.Entities.Medications;
using System;
using System.ComponentModel.DataAnnotations;

namespace BFCAI.Nesyan.Application.Abstraction.Models.Reminders
{
    public class ReminderToCreateDto
    {
        public ReminderType Type { get; set; }

        public string Title { get; set; } = null!;

        // Medication name OR Doctor name
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

        public bool IsCompleted { get; set; }

        public int PatientId { get; set; }
    }
}
