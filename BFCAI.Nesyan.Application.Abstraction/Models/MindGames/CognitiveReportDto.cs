using System;
using System.Text.Json.Serialization;

namespace BFCAI.Nesyan.Application.Abstraction.Models.MindGames
{
    public class CognitiveReportDto
    {
        [JsonPropertyName("patient_id")]
        public string PatientId { get; set; } = null!;

        [JsonPropertyName("prediction")]
        public string Prediction { get; set; } = null!;

        [JsonPropertyName("confidence")]
        public double Confidence { get; set; }

        [JsonPropertyName("probabilities")]
        public ProbabilitiesDto Probabilities { get; set; } = null!;

        [JsonPropertyName("alert")]
        public string Alert { get; set; } = null!;

        [JsonPropertyName("predicted_at")]
        public DateTime PredictedAt { get; set; }
    }

    public class ProbabilitiesDto
    {
        [JsonPropertyName("declining")]
        public double Declining { get; set; }

        [JsonPropertyName("improving")]
        public double Improving { get; set; }

        [JsonPropertyName("stable")]
        public double Stable { get; set; }
    }
}
