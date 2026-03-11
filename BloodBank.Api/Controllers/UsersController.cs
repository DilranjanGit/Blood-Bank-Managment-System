using BloodBank.Api.Models;
using BloodBank.Api.Services;
using BloodBank.Api.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BloodBank.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;

    public UsersController(IUserService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        return Ok(await _service.GetAllUsers());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _service.GetUserById(id);
        if (user == null) return NotFound();
        return Ok(user);
    }
    [HttpGet("email/{email}")]
    public async Task<IActionResult> GetUserByEmail(string email)
    {
        var user = await _service.GetUserByEmail(email);
        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserDto dto)
    {
        var user = await _service.AddUser(dto);
        return CreatedAtAction(nameof(GetUserByEmail), new { email = user.Email }, user);
    }   
}