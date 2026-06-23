namespace BFCAI.Nesyan.Application.Abstraction.Models.MindGames
{
    public class MindGameDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Subtitle { get; set; } = null!;
        public string Image { get; set; } = null!;
        public string Level { get; set; } = null!;
    }
}
