using SupportDesk.Application.Dtos;

namespace SupportDesk.Application.Interfaces.Services;

public interface IAuthService
{
    (string Username, string Role)? Validate(LoginRequest request);
}