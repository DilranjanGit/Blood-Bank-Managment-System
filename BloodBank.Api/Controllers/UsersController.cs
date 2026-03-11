using BloodBank.Api.Models;
using BloodBank.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BloodBank.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _repo;

    public UsersController(IUserRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        return Ok(await _repo.GetAllUsers());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _repo.GetUserById(id);
        if (user == null) return NotFound();
        return Ok(user);
    }
}