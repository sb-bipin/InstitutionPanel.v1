using InstitutionPanel.Models.ValueObject;

namespace InstitutionPanel.Models.ViewModels
{
    public class InstitutionVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Email Email { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public ContactNumber ContactNo { get; set; }
        public PhoneNumber PhoneNo { get; set; }
        public bool IsActive { get; set; }
        public string Created { get; set; }
        public string? UpdatedAt { get; set; }
    }
}
