using System;
using System.ComponentModel.DataAnnotations;

namespace BFCAI.Nesyan.Application.Abstraction.Models.MindGames
{
    public class AssignMindGameDto
    {
        [Required]
        public int DoctorId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public string Frequency { get; set; } = null!;
    }
}
