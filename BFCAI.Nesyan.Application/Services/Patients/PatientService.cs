using BFCAI.Nesyan.Application.Abstraction.Models.Patients;
using BFCAI.Nesyan.Application.Abstraction.Models.Medications;
using BFCAI.Nesyan.Application.Abstraction.Models.MindGames;

using BFCAI.Nesyan.Application.Abstraction.Services.Patients;
using BFCAI.Nesyan.Domain.Contracts;
using BFCAI.Nesyan.Domain.Entities.Primary.Patients;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BFCAI.Nesyan.Domain.Entities.MindGames;
using BFCAI.Nesyan.Domain.Entities.Relations.MindGames;

namespace BFCAI.Nesyan.Application.Services.Patients
{
    public class PatientService(IUnitOfWork UnitOfWork, IMapper Mapper) : IPatientService
    {
        public async Task UpdatePatientStageAsync(int patientId, int newStage)
        {
            var repo = UnitOfWork.GetRepository<Patient, int>();
            var patient = await repo.Get(patientId);

            if (patient == null)
                throw new Exception("Patient not found");

            patient.CurrentStage = (AlzheimerStage)newStage;
            patient.LastModifiedOn = DateTime.UtcNow;

            repo.Update(patient);
            await UnitOfWork.CompleteAsync();
        }

        public async Task<PatientFullProfileDto> GetPatientProfileAsync(int patientId)
        {
            var patientRepo = UnitOfWork.GetRepository<Patient, int>();
            var patient = await patientRepo.Get(patientId);
            if (patient == null) throw new Exception("Patient not found");

            var pgRepo = UnitOfWork.GetRepository<MindGameSession, int>();
            var gameRepo = UnitOfWork.GetRepository<MindGame, int>();

            var allPG = await pgRepo.GetAllAsync(false);
            var patientGames = allPG.Where(pg => pg.PatientId == patientId).ToList();

            var gameDtos = Mapper.Map<List<PatientMindGameDto>>(patientGames);
            var allGames = await gameRepo.GetAllAsync(false);
            foreach (var g in gameDtos)
            {
                var gameEntity = allGames.FirstOrDefault(x => x.Id == g.MindGameId);
                if (gameEntity != null) g.MindGame = Mapper.Map<MindGameDto>(gameEntity);
            }

            var profileDto = Mapper.Map<PatientFullProfileDto>(patient);
            profileDto.AssignedGames = gameDtos;

            return profileDto;
        }
    }
}
