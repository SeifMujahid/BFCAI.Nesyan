using AutoMapper;
using BFCAI.Nesyan.Application.Abstraction.Models.Patients;
using BFCAI.Nesyan.Application.Abstraction.Services.Patients;
using BFCAI.Nesyan.Domain.Contracts;
using BFCAI.Nesyan.Domain.Entities.Primary.Patients;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Services.Patients
{
    public class FamilyMembersService(
        IUnitOfWork UnitOfWork, 
        IMapper Mapper,
        IHttpClientFactory HttpClientFactory,
        IConfiguration Configuration) : IFamilyMembersService
    {
        private async Task<string> SaveFileAsync(IFormFile file, string subFolder)
        {
            if (file == null || file.Length == 0) return string.Empty;
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", "FamilyMembers", subFolder);
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return $"/Uploads/FamilyMembers/{subFolder}/{uniqueFileName}";
        }

        public async Task<IEnumerable<FamilyMemberDto>> GetFamilyMembersByPatientIdAsync(int patientId)
        {
            var repo = UnitOfWork.GetRepository<FamilyMember, int>();
            var all = await repo.GetAllAsync(false);
            var filtered = all.Where(f => f.PatientId == patientId).ToList();
            return Mapper.Map<IEnumerable<FamilyMemberDto>>(filtered);
        }

        public async Task<FamilyMemberDto> CreateFamilyMemberAsync(FamilyMemberCreateDto dto)
        {
            var patientRepo = UnitOfWork.GetRepository<Patient, int>();
            var patient = await patientRepo.Get(dto.PatientId);
            if (patient == null) throw new Exception("Patient not found.");

            var repo = UnitOfWork.GetRepository<FamilyMember, int>();
            var member = Mapper.Map<FamilyMember>(dto);

            if (dto.Image != null)
            {
                member.ImageUrl = await SaveFileAsync(dto.Image, "Images");
            }
            if (dto.Audio != null)
            {
                member.AudioUrl = await SaveFileAsync(dto.Audio, "Audios");
            }

            member.CreatedOn = DateTime.UtcNow;
            member.CreatedBy = "System";
            member.LastModifiedOn = DateTime.UtcNow;
            member.LastModifiedBy = "System";

            await repo.AddAsync(member);
            await UnitOfWork.CompleteAsync();

            // Enroll voice speaker if audio file is uploaded
            if (dto.Audio != null && !string.IsNullOrEmpty(member.AudioUrl))
            {
                await EnrollVoiceSpeakerAsync(dto.PatientId, dto.Name, dto.Relation, member.AudioUrl);
            }

            return Mapper.Map<FamilyMemberDto>(member);
        }

        public async Task<FamilyMemberDto> UpdateFamilyMemberAsync(int id, FamilyMemberUpdateDto dto)
        {
            var repo = UnitOfWork.GetRepository<FamilyMember, int>();
            var member = await repo.Get(id);
            if (member == null) throw new Exception("Family member not found.");

            var patientRepo = UnitOfWork.GetRepository<Patient, int>();
            var patient = await patientRepo.Get(dto.PatientId);
            if (patient == null) throw new Exception("Patient not found.");

            Mapper.Map(dto, member);

            if (dto.Image != null)
            {
                member.ImageUrl = await SaveFileAsync(dto.Image, "Images");
            }
            if (dto.Audio != null)
            {
                member.AudioUrl = await SaveFileAsync(dto.Audio, "Audios");
            }

            member.LastModifiedOn = DateTime.UtcNow;
            member.LastModifiedBy = "System";

            repo.Update(member);
            await UnitOfWork.CompleteAsync();

            // Re-enroll if a new audio file is uploaded
            if (dto.Audio != null && !string.IsNullOrEmpty(member.AudioUrl))
            {
                await EnrollVoiceSpeakerAsync(dto.PatientId, dto.Name, dto.Relation, member.AudioUrl);
            }

            return Mapper.Map<FamilyMemberDto>(member);
        }

        public async Task<bool> DeleteFamilyMemberAsync(int id)
        {
            var repo = UnitOfWork.GetRepository<FamilyMember, int>();
            var member = await repo.Get(id);
            if (member == null) return false;

            repo.Delete(member);
            await UnitOfWork.CompleteAsync();
            return true;
        }

        public async Task<FamilyMemberDto?> IdentifySpeakerVoiceAsync(int patientId, IFormFile audioFile)
        {
            if (audioFile == null || audioFile.Length == 0)
                throw new ArgumentException("Audio file is empty or missing.", nameof(audioFile));

            var baseUrl = Configuration["VoiceModelServiceUrl"] 
                ?? Environment.GetEnvironmentVariable("VOICE_MODEL_SERVICE_URL") 
                ?? "https://web-production-e9f4.up.railway.app";

            var url = $"{baseUrl.TrimEnd('/')}/identify";

            try
            {
                var client = HttpClientFactory.CreateClient();
                using var requestContent = new MultipartFormDataContent();

                var fileStream = audioFile.OpenReadStream();
                var fileContent = new StreamContent(fileStream);
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(audioFile.ContentType ?? "audio/wav");

                requestContent.Add(fileContent, "audio", audioFile.FileName);
                requestContent.Add(new StringContent(patientId.ToString()), "patient_id");

                var response = await client.PostAsync(url, requestContent);
                if (!response.IsSuccessStatusCode)
                {
                    var errorMsg = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[VoiceModel Error] Failed to identify speaker. Status: {response.StatusCode}, Response: {errorMsg}");
                    return null;
                }

                var responseString = await response.Content.ReadAsStringAsync();
                var identifyResult = JsonSerializer.Deserialize<VoiceIdentifyResponse>(responseString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var identifiedName = identifyResult?.Name ?? identifyResult?.Speaker;
                if (string.IsNullOrEmpty(identifiedName))
                {
                    Console.WriteLine($"[VoiceModel Warning] Response deserialized but Name/Speaker was empty: {responseString}");
                    return null;
                }

                // Query database to find matching FamilyMember
                var repo = UnitOfWork.GetRepository<FamilyMember, int>();
                var all = await repo.GetAllAsync(false);
                var matched = all.FirstOrDefault(f => 
                    f.PatientId == patientId && 
                    string.Equals(f.Name, identifiedName, StringComparison.OrdinalIgnoreCase)
                );

                if (matched == null)
                {
                    Console.WriteLine($"[VoiceModel Match Failure] Identified '{identifiedName}' but no matching family member found in DB for patient {patientId}");
                    return null;
                }

                return Mapper.Map<FamilyMemberDto>(matched);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[VoiceModel Exception] Error calling voice identify API: {ex.Message}");
                return null;
            }
        }

        private async Task EnrollVoiceSpeakerAsync(int patientId, string name, string relation, string audioUrl)
        {
            if (string.IsNullOrEmpty(audioUrl)) return;

            var baseUrl = Configuration["VoiceModelServiceUrl"] 
                ?? Environment.GetEnvironmentVariable("VOICE_MODEL_SERVICE_URL") 
                ?? "https://web-production-e9f4.up.railway.app";

            var url = $"{baseUrl.TrimEnd('/')}/enroll";

            try
            {
                var localPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", audioUrl.TrimStart('/'));
                if (!File.Exists(localPath))
                {
                    Console.WriteLine($"[VoiceModel Error] Local audio file not found at path: {localPath}");
                    return;
                }

                var client = HttpClientFactory.CreateClient();
                using var requestContent = new MultipartFormDataContent();

                // Open the local file on disk
                using var fileStream = new FileStream(localPath, FileMode.Open, FileAccess.Read);
                var fileContent = new StreamContent(fileStream);
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("audio/wav");

                var fileName = Path.GetFileName(localPath);
                requestContent.Add(fileContent, "audio", fileName);
                requestContent.Add(new StringContent(name), "name");
                requestContent.Add(new StringContent(relation), "relation");
                requestContent.Add(new StringContent(patientId.ToString()), "patient_id");

                var response = await client.PostAsync(url, requestContent);
                if (!response.IsSuccessStatusCode)
                {
                    var errorMsg = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"[VoiceModel Error] Failed to enroll speaker. Status: {response.StatusCode}, Response: {errorMsg}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[VoiceModel Exception] Error calling voice enroll API: {ex.Message}");
            }
        }

        private class VoiceIdentifyResponse
        {
            public string? Name { get; set; }
            public string? Speaker { get; set; }
            public string? Relation { get; set; }
        }
    }
}
