using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace InstitutionPanel.Models.ViewModels
{
    public class ImageGalleryVM
    {
        public Guid Id { get; set; }

        [Required, MaxLength(30)]
        public string Title { get; set; }

        public string? ImageUrl { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        // For upload (Create / Edit)
        public IFormFile ImageFile { get; set; }

        public Guid InstitutionId { get; set; }

        // UI helpers
        public bool IsEdit => Id != Guid.Empty;
    }
}
