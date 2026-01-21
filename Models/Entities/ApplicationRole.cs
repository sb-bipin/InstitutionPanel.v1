using Microsoft.AspNetCore.Identity;

namespace InstitutionPanel.Models.Entities
{
    public class ApplicationRole : IdentityRole
    {
        public string? Description { get; set; }
    }
}
