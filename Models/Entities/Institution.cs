using InstitutionPanel.Models.Common;
using InstitutionPanel.Models.ValueObject;
using System;
using System.ComponentModel.DataAnnotations;

namespace InstitutionPanel.Models.Entities
{
    public class Institution : CommonEntities, IBaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public Email Email { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [MaxLength(100)]
        public string Address { get; set; }

        public ContactNumber ContactNo { get; set; }

        public PhoneNumber PhoneNo { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<ImageGallery> ImageGalleries { get; set; }

    }
}
