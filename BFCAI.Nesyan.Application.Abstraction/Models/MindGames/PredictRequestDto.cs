using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BFCAI.Nesyan.Application.Abstraction.Models.MindGames
{
    public class PredictRequestDto
    {
        [JsonPropertyName("patient_id")]
        public string PatientId { get; set; } = null!;

        [JsonPropertyName("date")]
        public string Date { get; set; } = null!;

        [JsonPropertyName("sessions")]
        public List<PredictSessionDto> Sessions { get; set; } = null!;
    }

    public class PredictSessionDto
    {
        [JsonPropertyName("session_id")]
        public int SessionId { get; set; }

        [JsonPropertyName("score")]
        public double Score { get; set; }

        [JsonPropertyName("time_taken")]
        public double TimeTaken { get; set; }
    }
}
