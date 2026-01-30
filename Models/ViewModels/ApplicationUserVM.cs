using InstitutionPanel.Models.ValueObject;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace InstitutionPanel.Models.ViewModels
{
    public class ApplicationUserVM
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid? InstitutionId { get; set; }
        public bool IsActive { get; set; }
        public UserType UserType { get; set; }
        public string CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public string InstitutionName { get; set; }
        public List<SelectListItem> Institutions { get; set; } = new();

    }
}
