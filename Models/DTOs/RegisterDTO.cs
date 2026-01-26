using InstitutionPanel.Models.ValueObject;
using System.ComponentModel.DataAnnotations;

namespace InstitutionPanel.Models.DTOs
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public Email Email { get; set; }

        [Required]
        [MinLength(8),MaxLength(30)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public Guid? InstitutionId { get; set; }
    }
}
