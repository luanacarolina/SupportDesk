using Microsoft.AspNetCore.Mvc;
using SupportDesk.Application.Dtos;
using SupportDesk.Application.Interfaces.Services;

namespace SupportDesk.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(ITokenService tokenService, IConfiguration configuration, IAuthService authService, IGoogleAuthService googleAuthService) : ControllerBase
{
    private readonly ITokenService _tokenService = tokenService;
    private readonly IConfiguration _configuration = configuration;
    private readonly IAuthService _authService = authService;
    private readonly IGoogleAuthService _googleAuthService = googleAuthService;

    [HttpPost("login")]
    public ActionResult<LoginResponse> Login([FromBody] LoginRequest request)
    {
        var user = _authService.Validate(request);

        if (user is null)
            return Unauthorized(new { message = "Usuário ou senha inválidos" });

        var token = _tokenService.GenerateToken(user.Value.Username, user.Value.Role);

        var expires = int.Parse(_configuration["Jwt:ExpiresInMinutes"]!);

        return Ok(new LoginResponse
        {
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(expires)
        });
    }
    [HttpPost("google-login")]
    public async Task<ActionResult<LoginResponse>> GoogleLogin(
      [FromBody] LoginWithGoogleRequest request,
      CancellationToken cancellationToken)
    {
        var googleUser = await _googleAuthService.ValidateAsync(request.IdToken, cancellationToken);

        if (googleUser is null)
            return Unauthorized(new { message = "Token Google inválido." });

        var token = _tokenService.GenerateToken(googleUser.Value.Email, "User");
        var expires = int.Parse(_configuration["Jwt:ExpiresInMinutes"]!);

        return Ok(new LoginResponse
        {
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(expires)
        });
    }
}