using System;

namespace BFCAI.Nesyan.Application.Abstraction.Models.Routines
{
    public class RoutineToReturnDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public DateOnly ReminderDate { get; set; }

        public TimeOnly ReminderTime { get; set; }

        public string Frequency { get; set; } = null!;

        public string? Notes { get; set; }

        public bool IsCompleted { get; set; }

    }
}
