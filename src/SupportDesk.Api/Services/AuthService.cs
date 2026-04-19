using SupportDesk.Api.Models;
using SupportDesk.Application.Dtos;
using SupportDesk.Application.Interfaces.Services;

namespace SupportDesk.Api.Services;

public class AuthService(IConfiguration configuration) : IAuthService
{
    private readonly List<AuthUser> _users = configuration
            .GetSection("Auth:Users")
            .Get<List<AuthUser>>() ?? [];

    public (string Username, string Role)? Validate(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            return null;

        var username = request.Username.Trim();
        var password = request.Password.Trim();

        var user = _users.FirstOrDefault(u =>
            u.Username == username &&
            u.Password == password);

        if (user is null)
            return null;

        return (user.Username, user.Role);
    }
}