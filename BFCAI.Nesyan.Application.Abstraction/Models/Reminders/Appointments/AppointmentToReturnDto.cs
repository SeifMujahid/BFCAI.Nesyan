using System;

namespace BFCAI.Nesyan.Application.Abstraction.Models.Appointments
{
    public class AppointmentToReturnDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Specialty { get; set; } = null!;

        public string? Location { get; set; }

        public DateOnly ReminderDate { get; set; }

        public TimeOnly ReminderTime { get; set; }

        public string Frequency { get; set; } = null!;

        public string? Notes { get; set; }

        public bool IsCompleted { get; set; }

    }
}
