using AutoMapper;
using BFCAI.Nesyan.Application.Abstraction.Models.Relatives;
using BFCAI.Nesyan.Application.Abstraction.Services.Relatives;
using BFCAI.Nesyan.Domain.Contracts;
using BFCAI.Nesyan.Domain.Entities.Primary.Relatives;
using BFCAI.Nesyan.Domain.Specifications.Relatives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace BFCAI.Nesyan.Application.Services.Relatives
{
    public class RelativeService(IUnitOfWork UnitOfWork, IMapper Mapper) : IRelativeService
    {
        public async Task<IEnumerable<RelativeToReturnDto>> GetRelativesAsync()
        {
            var repo = UnitOfWork.GetRepository<Relative, int>();
            var relatives = await repo.GetAllAsync();
            return Mapper.Map<IEnumerable<RelativeToReturnDto>>(relatives);
        }

        public async Task<RelativeToReturnDto> GetRelativeAsync(int id)
        {
            var repo = UnitOfWork.GetRepository<Relative, int>();
            var relative = await repo.Get(id);
            if (relative is null) throw new Exception("Relative not found");
            return Mapper.Map<RelativeToReturnDto>(relative);
        }

        public async Task<RelativeToReturnDto> CreateRelativeAsync(RelativeToCreateDto relativeToCreate)
        {
            var repo = UnitOfWork.GetRepository<Relative, int>();

            var existingRelatives = await repo.GetAllAsync();
            if (existingRelatives.Any(d => d.NationalId == relativeToCreate.NationalId))
                throw new Exception("NationalId is already registered.");
            if (existingRelatives.Any(d => d.Email.Equals(relativeToCreate.Email, StringComparison.OrdinalIgnoreCase)))
                throw new Exception("Email is already registered.");
            if (existingRelatives.Any(d => d.UserName.Equals(relativeToCreate.UserName, StringComparison.OrdinalIgnoreCase)))
                throw new Exception("UserName is already taken.");

            var relative = Mapper.Map<Relative>(relativeToCreate);
            relative.Password = BCrypt.Net.BCrypt.HashPassword(relative.Password);
            relative.CreatedOn = DateTime.UtcNow;
            relative.CreatedBy = relative.UserName;
            relative.LastModifiedOn = DateTime.UtcNow;
            relative.LastModifiedBy = relative.UserName;

            await repo.AddAsync(relative);
            await UnitOfWork.CompleteAsync();

            return Mapper.Map<RelativeToReturnDto>(relative);
        }

        private string SaveFile(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0) return string.Empty;
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", folderName);
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            return $"/uploads/{folderName}/{uniqueFileName}";
        }

        public async Task UpdateRelativeAsync(RelativeToReturnDto relativeToUpdate)
        {
            var repo = UnitOfWork.GetRepository<Relative, int>();
            var relative = await repo.Get(relativeToUpdate.Id);
            if (relative is null) throw new Exception("Relative not found");

            Mapper.Map(relativeToUpdate, relative);

            if (relativeToUpdate.Image != null)
            {
                relative.ImageUrl = SaveFile(relativeToUpdate.Image, "relatives/avatars");
            }

            relative.LastModifiedOn = DateTime.UtcNow;
            relative.LastModifiedBy = relative.UserName;

            repo.Update(relative);
            await UnitOfWork.CompleteAsync();
        }

        public async Task DeleteRelativeAsync(int id)
        {
            var repo = UnitOfWork.GetRepository<Relative, int>();
            var relative = await repo.Get(id);
            if (relative is null) throw new Exception("Relative not found");

            repo.Delete(relative);
            await UnitOfWork.CompleteAsync();
        }

        public async Task<RelativeProfileDto> GetRelativeProfileAsync(int id)
        {
            var specs = new GetRelativePatientsSpecification(id);
            var relative = await UnitOfWork.GetRepository<Relative, int>().GetWithSpecAsync(specs);
            if (relative is null)
                throw new Exception("Relative not found");
            return Mapper.Map<RelativeProfileDto>(relative);
        }
    }
}
