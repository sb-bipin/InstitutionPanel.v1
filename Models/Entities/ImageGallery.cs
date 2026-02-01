using InstitutionPanel.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace InstitutionPanel.Models.Entities
{
    public class ImageGallery : CommonEntities, IBaseEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Title { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }

        public Guid InstitutionId { get; set; }
        public virtual Institution Institution { get; set; }
    }
}
