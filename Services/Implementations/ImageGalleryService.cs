using InstitutionPanel.Core.Interfaces;
using InstitutionPanel.Data;
using InstitutionPanel.Models.Common;
using InstitutionPanel.Models.Entities;
using InstitutionPanel.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace InstitutionPanel.Core.Services
{
    public class ImageGalleryService : IImageGalleryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ImageGalleryService(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<ApiResponse<List<ImageGalleryVM>>> GetAllAsync(Guid institutionId)
        {
            var data = await _context.ImageGalleries
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new ImageGalleryVM
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    ImageUrl = x.ImageUrl,
                    InstitutionId = x.InstitutionId
                })
                .ToListAsync();

            return ApiResponse<List<ImageGalleryVM>>.SuccessResponse(data);
        }

        public async Task<ApiResponse<ImageGalleryVM>> GetByIdAsync(Guid id)
        {
            var vm = await _context.ImageGalleries
                .Where(x => x.Id == id)
                .Select(x => new ImageGalleryVM
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    ImageUrl = x.ImageUrl,
                    InstitutionId = x.InstitutionId
                })
                .FirstOrDefaultAsync();

            if (vm == null)
                return ApiResponse<ImageGalleryVM>.FailureResponse("Image not found");

            return ApiResponse<ImageGalleryVM>.SuccessResponse(vm);
        }

        public async Task<ApiResponse<bool>> CreateAsync(ImageGalleryVM vm, Guid institutionId)
        {
            try
            {
                var imagePath = await SaveImageAsync(vm.ImageFile);

                var entity = new ImageGallery
                {
                    Id = Guid.NewGuid(),
                    Title = vm.Title,
                    Description = vm.Description,
                    ImageUrl = imagePath,
                    InstitutionId = institutionId,
                };

                _context.ImageGalleries.Add(entity);
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Image created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.FailureResponse("Failed to create image", new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<bool>> UpdateAsync(ImageGalleryVM vm)
        {
            try
            {
                var entity = await _context.ImageGalleries.FindAsync(vm.Id);
                if (entity == null)
                    return ApiResponse<bool>.FailureResponse("Image not found");

                entity.Title = vm.Title;
                entity.Description = vm.Description;

                if (vm.ImageFile != null)
                    entity.ImageUrl = await SaveImageAsync(vm.ImageFile);

                await _context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Image updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.FailureResponse("Failed to update image", new List<string> { ex.Message });
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(Guid id)
        {
            try
            {
                var entity = await _context.ImageGalleries.FindAsync(id);
                if (entity == null)
                    return ApiResponse<bool>.FailureResponse("Image not found");

                _context.ImageGalleries.Remove(entity);
                await _context.SaveChangesAsync();

                return ApiResponse<bool>.SuccessResponse(true, "Image deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.FailureResponse("Failed to delete image", new List<string> { ex.Message });
            }
        }

        // ================= Image Upload =================
        private async Task<string?> SaveImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            var folder = Path.Combine(_env.WebRootPath, "uploads/gallery");
            Directory.CreateDirectory(folder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var path = Path.Combine(folder, fileName);

            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/uploads/gallery/{fileName}";
        }
    }
}
