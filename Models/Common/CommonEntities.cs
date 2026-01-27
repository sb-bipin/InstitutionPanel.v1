namespace InstitutionPanel.Models.Common
{
    public abstract class CommonEntities
    {
        public string CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
