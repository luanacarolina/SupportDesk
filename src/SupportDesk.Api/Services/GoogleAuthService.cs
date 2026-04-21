using Google.Apis.Auth;
using SupportDesk.Application.Interfaces.Services;

namespace SupportDesk.Api.Services;

public class GoogleAuthService(IConfiguration configuration) : IGoogleAuthService
{
    private readonly IConfiguration _configuration = configuration;

    public async Task<(string Email, string Name)?> ValidateAsync(string idToken, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(idToken))
            return null;

        var clientId = _configuration["GoogleAuth:ClientId"];
        if (string.IsNullOrWhiteSpace(clientId))
            throw new InvalidOperationException("GoogleAuth:ClientId não configurado.");

        var settings = new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = [clientId]
        };

        try
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

            if (payload is null || string.IsNullOrWhiteSpace(payload.Email))
                return null;

            return (payload.Email, payload.Name ?? payload.Email);
        }
        catch (InvalidJwtException)
        {
            return null;
        }
    }
}
