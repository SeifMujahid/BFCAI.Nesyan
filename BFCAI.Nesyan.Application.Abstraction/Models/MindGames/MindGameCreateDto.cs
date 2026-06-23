using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BFCAI.Nesyan.Application.Abstraction.Models.MindGames
{
    public class MindGameCreateDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(500)]
        public string Subtitle { get; set; } = null!;

        [Required]
        public IFormFile Image { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string Level { get; set; } = null!;
    }
}
