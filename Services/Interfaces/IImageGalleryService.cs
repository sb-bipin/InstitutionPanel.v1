using InstitutionPanel.Models.Common;
using InstitutionPanel.Models.ViewModels;

namespace InstitutionPanel.Core.Interfaces
{
    public interface IImageGalleryService
    {
        Task<ApiResponse<List<ImageGalleryVM>>> GetAllAsync(Guid institutionId);

        Task<ApiResponse<ImageGalleryVM>> GetByIdAsync(Guid id);

        Task<ApiResponse<bool>> CreateAsync(ImageGalleryVM vm, Guid institutionId);

        Task<ApiResponse<bool>> UpdateAsync(ImageGalleryVM vm);

        Task<ApiResponse<bool>> DeleteAsync(Guid id);
    }
}
