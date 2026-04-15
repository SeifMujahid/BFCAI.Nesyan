//using AutoMapper;
//using BFCAI.Nesyan.Application.Abstraction.Models.MindGames;
//using BFCAI.Nesyan.Application.Abstraction.Services.MindGames;
//using BFCAI.Nesyan.Domain.Contracts;
//using BFCAI.Nesyan.Domain.Entities.Primary.MindGames;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace BFCAI.Nesyan.Application.Services.MindGames
//{
//    public class MindGamesService(IUnitOfWork UnitOfWork, IMapper Mapper) : IMindGamesService
//    {
//        public async Task<IEnumerable<MindGameDto>> GetGameCatalogAsync()
//        {
//            var repo = UnitOfWork.GetRepository<MindGame, int>();
//            var games = await repo.GetAllAsync(false);
//            return Mapper.Map<IEnumerable<MindGameDto>>(games);
//        }

//        public async Task<IEnumerable<PatientMindGameDto>> GetPatientGamesAsync(int patientId)
//        {
//            var repo = UnitOfWork.GetRepository<PatientMindGame, int>();
//            var gameRepo = UnitOfWork.GetRepository<MindGame, int>();

//            var allAssignments = await repo.GetAllAsync(false);
//            var patientAssignments = allAssignments.Where(pg => pg.PatientId == patientId).ToList();
            
//            var dtos = Mapper.Map<List<PatientMindGameDto>>(patientAssignments);
//            var allGames = await gameRepo.GetAllAsync(false);
            
//            foreach(var dto in dtos)
//            {
//                var game = allGames.FirstOrDefault(g => g.Id == dto.MindGameId);
//                if (game != null) 
//                {
//                    dto.MindGame = Mapper.Map<MindGameDto>(game);
//                }
//            }
//            return dtos;
//        }

//        public async Task AssignGameToPatientAsync(int patientId, int gameId)
//        {
//            var repo = UnitOfWork.GetRepository<PatientMindGame, int>();
            
//            var allAssignments = await repo.GetAllAsync(false);
//            var existing = allAssignments.FirstOrDefault(x => x.PatientId == patientId && x.MindGameId == gameId);
//            if (existing != null) throw new Exception("Game already assigned to this patient.");

//            var assignment = new PatientMindGame
//            {
//                PatientId = patientId,
//                MindGameId = gameId,
//                AssignedOn = DateTime.UtcNow
//            };

//            await repo.AddAsync(assignment);
//            await UnitOfWork.CompleteAsync();
//        }

//        public async Task RemoveGameFromPatientAsync(int patientId, int gameId)
//        {
//            var repo = UnitOfWork.GetRepository<PatientMindGame, int>();
            
//            var allAssignments = await repo.GetAllAsync(false);
//            var assignment = allAssignments.FirstOrDefault(x => x.PatientId == patientId && x.MindGameId == gameId);
//            if (assignment == null) throw new Exception("Game assignment not found.");

//            repo.Delete(assignment);
//            await UnitOfWork.CompleteAsync();
//        }
//    }
//}
