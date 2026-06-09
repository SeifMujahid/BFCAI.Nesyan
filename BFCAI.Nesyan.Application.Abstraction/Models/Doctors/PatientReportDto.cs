using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using BFCAI.Nesyan.Application.Abstraction.Models.Routines;
using BFCAI.Nesyan.Application.Abstraction.Models.MindGames;

namespace BFCAI.Nesyan.Application.Abstraction.Models.Doctors
{
    public class PatientReportDto
    {
        [JsonPropertyName("patient_id")]
        public int PatientId { get; set; }

        [JsonPropertyName("full_name")]
        public string FullName { get; set; } = null!;

        [JsonPropertyName("age")]
        public int Age { get; set; }

        [JsonPropertyName("gender")]
        public string Gender { get; set; } = null!;

        [JsonPropertyName("chronic_disease")]
        public string ChronicDisease { get; set; } = null!;

        [JsonPropertyName("alzheimer_stage")]
        public string AlzheimerStage { get; set; } = null!;

        [JsonPropertyName("blood_type")]
        public string BloodType { get; set; } = null!;

        [JsonPropertyName("cognitive_prediction")]
        public CognitivePredictionSectionDto CognitivePrediction { get; set; } = null!;

        [JsonPropertyName("mind_games_statistics")]
        public MindGamesStatisticsDto MindGamesStatistics { get; set; } = null!;

        [JsonPropertyName("routines_statistics")]
        public RoutinesStatisticsDto RoutinesStatistics { get; set; } = null!;

        [JsonPropertyName("medications_statistics")]
        public MedicationsStatisticsDto MedicationsStatistics { get; set; } = null!;

        [JsonPropertyName("telemetry_statistics")]
        public TelemetryStatisticsDto TelemetryStatistics { get; set; } = null!;
    }

    public class CognitivePredictionSectionDto
    {
        [JsonPropertyName("is_available")]
        public bool IsAvailable { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = null!;

        [JsonPropertyName("prediction")]
        public string? Prediction { get; set; }

        [JsonPropertyName("confidence")]
        public double? Confidence { get; set; }

        [JsonPropertyName("risk_score")]
        public int? RiskScore { get; set; }

        [JsonPropertyName("probabilities")]
        public ProbabilitiesDto? Probabilities { get; set; }

        [JsonPropertyName("alert")]
        public string? Alert { get; set; }

        [JsonPropertyName("explanation")]
        public List<ExplanationDto>? Explanation { get; set; }

        [JsonPropertyName("predicted_at")]
        public DateTime? PredictedAt { get; set; }
    }

    public class MindGamesStatisticsDto
    {
        [JsonPropertyName("total_assigned_games")]
        public int TotalAssignedGames { get; set; }

        [JsonPropertyName("total_sessions_completed")]
        public int TotalSessionsCompleted { get; set; }

        [JsonPropertyName("average_score")]
        public double AverageScore { get; set; }

        [JsonPropertyName("highest_score")]
        public double HighestScore { get; set; }

        [JsonPropertyName("assigned_games")]
        public List<PatientMindGameDto> AssignedGames { get; set; } = new();

        [JsonPropertyName("recent_game_records")]
        public List<PatternGameRecordDto> RecentGameRecords { get; set; } = new();
    }

    public class RoutinesStatisticsDto
    {
        [JsonPropertyName("total_routines")]
        public int TotalRoutines { get; set; }

        [JsonPropertyName("completed_routines")]
        public int CompletedRoutines { get; set; }

        [JsonPropertyName("adherence_rate")]
        public double AdherenceRate { get; set; }

        [JsonPropertyName("routines_list")]
        public List<RoutineToReturnDto> RoutinesList { get; set; } = new();
    }

    public class MedicationsStatisticsDto
    {
        [JsonPropertyName("total_medications")]
        public int TotalMedications { get; set; }

        [JsonPropertyName("completed_medications")]
        public int CompletedMedications { get; set; }

        [JsonPropertyName("adherence_rate")]
        public double AdherenceRate { get; set; }

        [JsonPropertyName("medications_list")]
        public List<RoutineToReturnDto> MedicationsList { get; set; } = new();
    }

    public class TelemetryStatisticsDto
    {
        [JsonPropertyName("has_telemetry")]
        public bool HasTelemetry { get; set; }

        [JsonPropertyName("average_heart_rate")]
        public double AverageHeartRate { get; set; }

        [JsonPropertyName("latest_heart_rate")]
        public double LatestHeartRate { get; set; }

        [JsonPropertyName("average_oxygen_level")]
        public double AverageOxygenLevel { get; set; }

        [JsonPropertyName("latest_oxygen_level")]
        public double LatestOxygenLevel { get; set; }

        [JsonPropertyName("latest_telemetry_time")]
        public DateTime? LatestTelemetryTime { get; set; }

        [JsonPropertyName("total_steps_tracked")]
        public int TotalStepsTracked { get; set; }
    }
}
