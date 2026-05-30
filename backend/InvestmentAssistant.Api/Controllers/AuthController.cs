using Microsoft.AspNetCore.Mvc;
using InvestmentAssistant.Api.Services;
using InvestmentAssistant.Api.Models.Responses;
using InvestmentAssistant.Api.Models.Requests;
using InvestmentAssistant.Api.Repositories;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace InvestmentAssistant.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUserRepository _userRepository;

    public AuthController(IAuthService authService, IUserRepository userRepository)
    {
        _authService = authService;
        _userRepository = userRepository;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var response = await _authService.LoginAsync(request);
            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var response = await _authService.RegisterAsync(request);
            return CreatedAtAction(nameof(Register), response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }


[Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userId == null || !Guid.TryParse(userId.Value, out var userIdGuid))
        {
            return Unauthorized(new { message = "Token jest nieważny." });
        }

        var user = await _userRepository.GetByIdAsync(userIdGuid);
        if (user == null)
        {
            return NotFound(new { message = "Użytkownik nie został znaleziony." });
        }

        return Ok(new UserResponse(
            Id: user.Id,
            Email: user.Email,
            FullName: user.FullName,
            PhoneNumber: user.PhoneNumber,
            CreatedAt: user.CreatedAt
        ));

    }
}
