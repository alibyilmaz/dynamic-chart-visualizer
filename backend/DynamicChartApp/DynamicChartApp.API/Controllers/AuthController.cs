using Microsoft.AspNetCore.Mvc;
using DynamicChartApp.Application.Services;
using System.Net.Http.Headers;
using System.Text;

namespace DynamicChartApp.API.Controllers;

public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}



[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("token")]
    public async Task<IActionResult> GetToken([FromBody] LoginRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            return BadRequest("Username and password are required");

        var token = await _authService.GenerateTokenAsync(request.Username, request.Password);
        if (token == null)
            return Unauthorized("Invalid credentials");

        return Ok(new { token });
    }
}
