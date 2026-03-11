using AutoMapper;
using BloodBank.Api.Models;
using BloodBank.Api.Repositories;
using Microsoft.EntityFrameworkCore;
using BloodBank.Api.DTOs.Payments;

namespace BloodBank.Api.Services;


public class PaymentService : IPaymentService
{
    private readonly BloodBankContext _ctx;
    private readonly IPaymentRepository _payments;
    private readonly IOrderRepository _orders;
    private readonly IMapper _mapper;

    public PaymentService(BloodBankContext ctx, IPaymentRepository payments, IOrderRepository orders, IMapper mapper)
    {
        _ctx = ctx;
        _payments = payments;
        _orders = orders;
        _mapper = mapper;
    }

    public async Task<PaymentDto> CreateAsync(CreatePaymentDto dto)
    {
        using var tx = await _ctx.Database.BeginTransactionAsync();

        var order = await _orders.GetByIdAsync(dto.OrderId);
        if (order == null) throw new KeyNotFoundException("Order not found.");

        // Optional integrity check
        if (dto.AmountPaid != order.TotalAmount)
            throw new InvalidOperationException("Payment amount mismatch.");

        var payment = _mapper.Map<Payment>(dto);
        payment.PaymentDate = DateTime.UtcNow;

        await _payments.AddAsync(payment);

        // If payment succeeded, confirm order
        if (string.Equals(dto.PaymentStatus, "Success", StringComparison.OrdinalIgnoreCase))
        {
            order.OrderStatus = "Confirmed";
            await _orders.UpdateAsync(order);
            await _orders.AddStatusHistoryAsync(new OrderStatusHistory
            {
                OrderId = order.OrderId,
                Status = "Confirmed",
                UpdatedBy = order.UserId,
                UpdatedAt = DateTime.UtcNow
            });
        }

        await tx.CommitAsync();
        return _mapper.Map<PaymentDto>(payment);
    }

    public async Task<IEnumerable<PaymentDto>> GetByOrderAsync(int orderId)
    {
        var list = await _payments.GetByOrderAsync(orderId);
        return _mapper.Map<IEnumerable<PaymentDto>>(list);
    }

    public async Task<PaymentDto> GetByIdAsync(int paymentId)
    {
        var p = await _payments.GetByIdAsync(paymentId);
        return p == null ? null : _mapper.Map<PaymentDto>(p);
    }

    public async Task<PaymentDto> UpdateStatusAsync(int paymentId, UpdatePaymentStatusDto dto)
    {
        using var tx = await _ctx.Database.BeginTransactionAsync();

        var payment = await _payments.GetByIdAsync(paymentId);
        if (payment == null) return null;

        payment.PaymentStatus = dto.PaymentStatus;
        if (!string.IsNullOrWhiteSpace(dto.TransactionId))
            payment.TransactionId = dto.TransactionId;

        await _payments.UpdateAsync(payment);

        // Sync order status on payment success
        var order = await _orders.GetByIdAsync(payment.OrderId);
        if (order != null && string.Equals(dto.PaymentStatus, "Success", StringComparison.OrdinalIgnoreCase))
        {
            if (!string.Equals(order.OrderStatus, "Confirmed", StringComparison.OrdinalIgnoreCase))
            {
                order.OrderStatus = "Confirmed";
                await _orders.UpdateAsync(order);
                await _orders.AddStatusHistoryAsync(new OrderStatusHistory
                {
                    OrderId = order.OrderId,
                    Status = "Confirmed",
                    UpdatedBy = order.UserId,
                    UpdatedAt = DateTime.UtcNow
                });
            }
        }

        await tx.CommitAsync();
        return _mapper.Map<PaymentDto>(payment);
    }

    // Useful for gateway webhooks
    public async Task HandleGatewayWebhookAsync(string transactionId, string newStatus)
    {
        var payment = await _payments.GetByTransactionAsync(transactionId);
        if (payment == null) return;
        await UpdateStatusAsync(payment.PaymentId, new UpdatePaymentStatusDto
        {
            PaymentStatus = newStatus,
            TransactionId = transactionId
        });
    }
}