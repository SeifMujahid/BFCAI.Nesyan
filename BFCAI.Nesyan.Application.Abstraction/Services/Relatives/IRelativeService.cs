using BFCAI.Nesyan.Application.Abstraction.Models.Relatives;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Abstraction.Services.Relatives
{
    public interface IRelativeService
    {
        Task<IEnumerable<RelativeToReturnDto>> GetRelativesAsync();
        Task<RelativeToReturnDto> GetRelativeAsync(int id);
        Task<RelativeToReturnDto> CreateRelativeAsync(RelativeToCreateDto relativeToCreate);
        Task UpdateRelativeAsync(RelativeToReturnDto relativeToUpdate);
        Task DeleteRelativeAsync(int id);
        Task<RelativeProfileDto> GetRelativeProfileAsync(int id);
    }
}
