namespace SupportDesk.Application.Interfaces.Services;

public interface ITokenService
{
    string GenerateToken(string username, string role);
}