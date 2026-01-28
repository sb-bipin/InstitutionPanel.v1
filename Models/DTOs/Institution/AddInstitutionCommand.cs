using InstitutionPanel.Models.ValueObject;
using System.ComponentModel.DataAnnotations;

namespace InstitutionPanel.Models.DTOs.Institution
{
    public class AddInstitutionCommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public string PhoneNo { get; set; }
        public bool IsActive { get; set; }
    }
}
