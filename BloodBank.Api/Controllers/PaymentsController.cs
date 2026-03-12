using AutoMapper;
using BloodBank.Api.DTOs.Payments;
using BloodBank.Api.Models;
using BloodBank.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace BloodBank.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _service;
    public PaymentsController(IPaymentService service) => _service = service;

    [Authorize(Roles = "Administrator,Staff,Donor")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePaymentDto dto)
    {
        try
        {
            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.PaymentId }, result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles ="Administrator,Staff,Donor")]
    public async Task<IActionResult> GetById(int id)
    {
        var p = await _service.GetByIdAsync(id);
        return p == null ? NotFound() : Ok(p);
    }

    [HttpGet("order/{orderId:int}")]
    [Authorize(Roles ="Administrator,Staff,Donor")]
    public async Task<IActionResult> GetByOrder(int orderId) =>
        Ok(await _service.GetByOrderAsync(orderId));

    [HttpPut("{id:int}/status")]
    [Authorize(Roles = "Administrator,Staff")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdatePaymentStatusDto dto)
    {
        var p = await _service.UpdateStatusAsync(id, dto);
        return p == null ? NotFound() : Ok(p);
    }

    // Public webhook endpoint (secure with a secret header in real deployments)
    [HttpPost("webhook")]
    [AllowAnonymous]
    public async Task<IActionResult> Webhook([FromQuery] string transactionId, [FromQuery] string status)
    {
        await _service.HandleGatewayWebhookAsync(transactionId, status);
        return Ok();
    }
}