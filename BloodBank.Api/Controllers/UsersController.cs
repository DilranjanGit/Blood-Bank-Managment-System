using BloodBank.Api.Models;
using BloodBank.Api.Services;
using BloodBank.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using BloodBank.Api.Security;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BloodBank.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;
    private readonly JwtService _jwtService;
    private readonly IHttpContextAccessor _http;

    private readonly IPasswordResetService _passwordResetService;


    public UsersController(IUserService service, JwtService jwtService, IHttpContextAccessor http, IPasswordResetService passwordResetService)
    {
        _service = service;
        _jwtService = jwtService;
        _http = http;
        _passwordResetService = passwordResetService;
    }

    [Authorize(Roles = "Administrator,Staff")]
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        return Ok(await _service.GetAllUsers());
    }

    [HttpGet("profile")]
    [Authorize] // Critical: Middleware won't populate User without this
    public IActionResult GetProfile()
    {
    // Common standard claims
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    var role = User.FindFirstValue(ClaimTypes.Role);
    var user = _service.GetUserById(int.Parse(userId ?? "0")).Result;
    
    var profile = new
    {
        UserId = userId,
        FullName = user?.FullName ?? "unknown",
        Email = user?.Email ?? "unknown",
        PhoneNumber = user?.Phone ?? "unknown",
        Role = role ?? "unknown"
    };
    return Ok(profile);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Administrator,Staff")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _service.GetUserById(id);
        if (user == null) return NotFound();
        return Ok(user);
    }
    [HttpGet("email/{email}")]
    [Authorize(Roles = "Administrator,Staff")]
    public async Task<IActionResult> GetUserByEmail(string email)
    {
        var user = await _service.GetUserByEmail(email);
        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(CreateUserDto dto)
    {
        var user = await _service.AddUser(dto);
        return CreatedAtAction(nameof(GetUserByEmail), new { email = user.Email }, user);
    }  

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _service.GetUserByEmail(dto.Email);
        if (user == null || !PasswordHasher.Verify(dto.Password,user.Password))
        {
            return Unauthorized("Invalid email or password.");
        }
        var token = _jwtService.GenerateToken(user.UserId, user.Role);
        return Ok(new { Token = token,Role = user.Role });
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotDto dto, [FromQuery] string? origin)
    {            
        var baseUrl = string.IsNullOrWhiteSpace(origin)
            ? $"{Request.Scheme}://{Request.Host.Value}" // fallback; ideally use Angular app URL from config
            : origin;
               
        var ua = _http.HttpContext?.Request.Headers.UserAgent.ToString() ?? "";
        var ip = _http.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "";

        await _passwordResetService.RequestAsync(dto.Email, baseUrl, ip, ua);

        // Always 200 - generic message (avoid enumeration)
        return Ok(new { message = "If an account exists for this email, a reset link has been sent." });

    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetDto dto)
    {
        
        var ua = _http.HttpContext?.Request.Headers.UserAgent.ToString() ?? "";
        var ip = _http.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "";

        var result = await _passwordResetService.ResetAsync(dto.Token, dto.NewPassword, ip, ua);
        if (!result)
        {
            return BadRequest("Invalid or expired reset token.");
        }
        return Ok(new { message = "Password has been reset successfully." });
    }
    
}