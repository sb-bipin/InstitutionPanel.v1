using InstitutionPanel.Models.Common;
using InstitutionPanel.Models.DTOs.Institution;
using InstitutionPanel.Models.ViewModels;

namespace InstitutionPanel.Services.Interfaces
{
    public interface IInstitutionService
    {
        Task<bool> IsInstitutionActiveAsync(Guid institutionId);
        Task<ApiResponse<InstitutionVM>> CreateInstitutionAsync(AddInstitutionCommand command);
        Task<ApiResponse<bool>> UpdateInstitutionAsync(UpdateInstitutionCommand command);
        Task<ApiResponse<bool>> DeleteInstitutionAsync(Guid id);
        Task<ApiResponse<InstitutionVM>> GetInstitutionByIdAsync(Guid id);
        Task<ApiResponse<List<InstitutionVM>>> GetAllInstitutionAsync();
    }

}
