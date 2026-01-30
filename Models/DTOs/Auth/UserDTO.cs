using Microsoft.AspNetCore.Mvc.Rendering;

namespace InstitutionPanel.Models.DTOs.Auth
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public UserType UserType { get; set; }
        public Guid? InstitutionId { get; set; }
        public List<SelectListItem> Institutions { get; set; } = new();

    }
}
