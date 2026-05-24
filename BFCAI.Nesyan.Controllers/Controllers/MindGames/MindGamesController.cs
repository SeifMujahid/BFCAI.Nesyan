using BFCAI.Nesyan.Application.Abstraction.Models.MindGames;
using BFCAI.Nesyan.Application.Abstraction.Services.MindGames;
using BFCAI.Nesyan.Controllers.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Controllers.Controllers.MindGames
{
    public class MindGamesController(IMindGamesService MindGamesService) : BaseApiController
    {
        [HttpGet("catalog")]
        public async Task<ActionResult<IEnumerable<MindGameDto>>> GetGameCatalog()
        {
            try
            {
                var games = await MindGamesService.GetGameCatalogAsync();
                return Ok(games);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MindGameDto>> GetGameById(int id)
        {
            try
            {
                var game = await MindGamesService.GetMindGameByIdAsync(id);
                if (game == null) return NotFound(new { Message = $"Mind game with ID {id} not found." });
                return Ok(game);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<MindGameDto>> CreateGame([FromForm] MindGameCreateDto dto)
        {
            try
            {
                var game = await MindGamesService.CreateMindGameAsync(dto);
                return CreatedAtAction(nameof(GetGameById), new { id = game.Id }, game);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MindGameDto>> UpdateGame(int id, [FromForm] MindGameUpdateDto dto)
        {
            try
            {
                var game = await MindGamesService.UpdateMindGameAsync(id, dto);
                return Ok(game);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteGame(int id)
        {
            try
            {
                var success = await MindGamesService.DeleteMindGameAsync(id);
                if (!success) return NotFound(new { Message = $"Mind game with ID {id} not found." });
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<IEnumerable<PatientMindGameDto>>> GetPatientGames(int patientId)
        {
            try
            {
                var games = await MindGamesService.GetPatientGamesAsync(patientId);
                return Ok(games);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("patient/{patientId}/assign/{gameId}")]
        public async Task<ActionResult> AssignGameToPatient(int patientId, int gameId, [FromBody] AssignMindGameDto dto)
        {
            try
            {
                await MindGamesService.AssignGameToPatientAsync(patientId, gameId, dto);
                return Ok(new { Message = "Game assigned successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("patient/{patientId}/remove/{gameId}")]
        public async Task<ActionResult> RemoveGameFromPatient(int patientId, int gameId)
        {
            try
            {
                await MindGamesService.RemoveGameFromPatientAsync(patientId, gameId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("patient/{patientId}/pattern-game")]
        public async Task<ActionResult<PatternGameRecordDto>> SubmitPatternGameResult(int patientId, [FromBody] PatternGameRecordToCreateDto dto)
        {
            try
            {
                var result = await MindGamesService.SubmitPatternGameResultAsync(patientId, dto);
                return CreatedAtAction(nameof(GetPatientPatternGameHistory), new { patientId }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("patient/{patientId}/pattern-game/history")]
        public async Task<ActionResult<IEnumerable<PatternGameRecordDto>>> GetPatientPatternGameHistory(int patientId)
        {
            try
            {
                var history = await MindGamesService.GetPatientPatternGameHistoryAsync(patientId);
                return Ok(history);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpGet("patient/{patientId}/report")]
        public async Task<ActionResult<CognitiveReportDto>> GetCognitiveReport(int patientId)
        {
            try
            {
                var report = await MindGamesService.GetCognitiveReportAsync(patientId);
                return Ok(report);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
