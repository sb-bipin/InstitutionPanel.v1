using InstitutionPanel.Models.Enums;
using InstitutionPanel.Models.ValueObject;
using Microsoft.AspNetCore.Identity;

namespace InstitutionPanel.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public UserType UserType { get; set; }


        public Guid? InstitutionId { get; set; }
        public virtual Institution Institution { get; set; }
    }
}
