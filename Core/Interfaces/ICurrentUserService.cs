namespace InstitutionPanel.Core.Interfaces
{
    public interface ICurrentUserService
    {
        string UserId { get; }
        string UserName { get; }
        string Email { get; }
        string UserType { get; }
        string? InstitutionId { get; }
        bool IsAuthenticated { get; }
    }

}
