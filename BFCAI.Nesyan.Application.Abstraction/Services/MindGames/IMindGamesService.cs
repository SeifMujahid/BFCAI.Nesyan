using BFCAI.Nesyan.Application.Abstraction.Models.MindGames;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Abstraction.Services.MindGames
{
    public interface IMindGamesService
    {
        Task<IEnumerable<MindGameDto>> GetGameCatalogAsync();
        Task<MindGameDto?> GetMindGameByIdAsync(int id);
        Task<MindGameDto> CreateMindGameAsync(MindGameCreateDto dto);
        Task<MindGameDto> UpdateMindGameAsync(int id, MindGameUpdateDto dto);
        Task<bool> DeleteMindGameAsync(int id);
        Task<IEnumerable<PatientMindGameDto>> GetPatientGamesAsync(int patientId);
        Task AssignGameToPatientAsync(int patientId, int gameId, AssignMindGameDto dto);
        Task RemoveGameFromPatientAsync(int patientId, int gameId);
        Task<PatternGameRecordDto> SubmitPatternGameResultAsync(int patientId, PatternGameRecordToCreateDto dto);
        Task<IEnumerable<PatternGameRecordDto>> GetPatientPatternGameHistoryAsync(int patientId);
        Task<CognitiveReportDto> GetCognitiveReportAsync(int patientId);
    }
}
