using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthAPIController(IAuthService authService) : ControllerBase
{

    private readonly IAuthService _authService = authService;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest registerRequest)
    {
        var result = await _authService.RegisterUser(registerRequest);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        var result = await _authService.Login(loginRequest);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenRequest refreshTokenRequest)
    {
        var result = await _authService.Refresh(refreshTokenRequest.token, refreshTokenRequest.refreshToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequest assignRoleRequest)
    {
        var result = await _authService.AssignRole(assignRoleRequest);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

}   
