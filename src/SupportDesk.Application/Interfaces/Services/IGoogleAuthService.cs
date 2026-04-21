namespace SupportDesk.Application.Interfaces.Services;

public interface IGoogleAuthService
{
    Task<(string Email, string Name)?> ValidateAsync(string idToken, CancellationToken cancellationToken = default);
}