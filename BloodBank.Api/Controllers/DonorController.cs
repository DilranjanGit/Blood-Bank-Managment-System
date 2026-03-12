using BloodBank.Api.DTOs;
using BloodBank.Api.Models;
using BloodBank.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloodBank.Api.Controllers;

[Authorize(Roles = "Administrator,Staff")]
[ApiController]
[Route("api/[controller]")]
public class DonorController : ControllerBase
{
    private readonly IDonorService _service;

    public DonorController(IDonorService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAllDonors());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var donor = await _service.GetDonorById(id);
        if (donor == null) return NotFound();
        return Ok(donor);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateDonorDto dto)
    {
        var result = await _service.CreateDonor(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.DonorId }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CreateDonorDto dto)
    {
        var result = await _service.UpdateDonor(id, dto);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _service.DeleteDonor(id);
        if (!success) return NotFound();
        return NoContent();
    }
}