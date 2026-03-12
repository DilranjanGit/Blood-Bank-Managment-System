using AutoMapper;
using BloodBank.Api.DTOs.Orders;
using BloodBank.Api.Models;
using BloodBank.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BloodBank.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _service;
    public OrdersController(IOrderService service) => _service = service;

    [Authorize(Roles = "Administrator,Staff,Donor")]
    [HttpPost]
    public async Task<IActionResult> PlaceOrder([FromBody] CreateOrderDto dto)
    {
        try
        {
            var result = await _service.PlaceOrderAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.OrderId }, result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize(Roles = "Administrator,Staff")]
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _service.GetAllAsync());

    [Authorize(Roles = "Administrator,Staff")]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var o = await _service.GetByIdAsync(id);
        return o == null ? NotFound() : Ok(o);
    }

    [Authorize(Roles = "Administrator,Staff,Donor")]
    [HttpGet("user/{userId:int}")]
    public async Task<IActionResult> GetByUser(int userId) =>
        Ok(await _service.GetByUserAsync(userId));

    [HttpPut("{id:int}/delivery")]
    [Authorize(Roles = "Administrator,Staff")]
    public async Task<IActionResult> UpdateDelivery(int id, [FromBody] UpdateOrderDeliveryDto dto, [FromQuery] int updatedBy)
    {
        var o = await _service.UpdateDeliveryAsync(id, dto, updatedBy);
        return o == null ? NotFound() : Ok(o);
    }

    [HttpPut("{id:int}/status")]
    [Authorize(Roles = "Administrator,Staff")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateOrderStatusDto dto)
    {
        var o = await _service.UpdateStatusAsync(id, dto);
        return o == null ? NotFound() : Ok(o);
    }

    [HttpDelete("{id:int}")]    
    [Authorize(Roles = "Administrator,Staff")]
    public async Task<IActionResult> Cancel(int id, [FromQuery] int updatedBy)
    {
        var ok = await _service.CancelAsync(id, updatedBy);
        return ok ? NoContent() : NotFound();
    }
}