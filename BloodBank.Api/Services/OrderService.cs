using AutoMapper;
using BloodBank.Api.Models;
using BloodBank.Api.Repositories;
using Microsoft.EntityFrameworkCore;
using BloodBank.Api.DTOs.Orders;

namespace BloodBank.Api.Services;

public class OrderService : IOrderService
{
    private readonly BloodBankContext _ctx;
    private readonly IOrderRepository _orders;
    private readonly IInventoryQueries _inventory;
    private readonly IMapper _mapper;

    public OrderService(BloodBankContext ctx, IOrderRepository orders, IInventoryQueries inventory, IMapper mapper)
    {
        _ctx = ctx;
        _orders = orders;
        _inventory = inventory;
        _mapper = mapper;
    }

    public async Task<OrderDto> PlaceOrderAsync(CreateOrderDto dto)
    {
        // Transaction ensures consistency between Orders, Inventory, and History
        using var tx = await _ctx.Database.BeginTransactionAsync();

        var stock = await _inventory.GetInventoryAsync(dto.ProductId, dto.BloodGroup);
        if (stock == null || stock.QuantityAvailable < dto.Quantity)
            throw new InvalidOperationException("Insufficient inventory for the requested product/blood group.");

        // Reserve inventory
        stock.QuantityAvailable -= dto.Quantity;
        stock.LastUpdated = DateTime.UtcNow;
        _ctx.Inventories.Update(stock);

        // Create order
        var order = _mapper.Map<Order>(dto);
        order.OrderStatus = "Pending"; // or "AwaitingPayment"
        order.CreatedAt = DateTime.UtcNow;

        _ctx.Orders.Add(order);
        await _ctx.SaveChangesAsync();

        // Status history
        var history = new OrderStatusHistory
        {
            OrderId = order.OrderId,
            Status = order.OrderStatus,
            UpdatedBy = dto.UserId,
            UpdatedAt = DateTime.UtcNow
        };
        _ctx.OrderStatusHistories.Add(history);

        await _ctx.SaveChangesAsync();
        await tx.CommitAsync();

        return _mapper.Map<OrderDto>(order);
    }

    public async Task<IEnumerable<OrderDto>> GetAllAsync()
    {
        var list = await _orders.GetAllAsync();
        return _mapper.Map<IEnumerable<OrderDto>>(list);
    }

    public async Task<IEnumerable<OrderDto>> GetByUserAsync(int userId)
    {
        var list = await _orders.GetByUserAsync(userId);
        return _mapper.Map<IEnumerable<OrderDto>>(list);
    }

    public async Task<OrderDto> GetByIdAsync(int orderId)
    {
        var entity = await _orders.GetByIdAsync(orderId);
        return entity == null ? null : _mapper.Map<OrderDto>(entity);
    }

    public async Task<OrderDto> UpdateDeliveryAsync(int orderId, UpdateOrderDeliveryDto dto, int updatedBy)
    {
        var order = await _orders.GetByIdAsync(orderId);
        if (order == null) return null;

        order.DeliveryDate = dto.DeliveryDate;
        order.DeliveryAddress = dto.DeliveryAddress;

        await _orders.UpdateAsync(order);

        await _orders.AddStatusHistoryAsync(new OrderStatusHistory
        {
            OrderId = orderId,
            Status = $"DeliveryUpdated",
            UpdatedBy = updatedBy,
            UpdatedAt = DateTime.UtcNow
        });

        return _mapper.Map<OrderDto>(order);
    }

    public async Task<OrderDto> UpdateStatusAsync(int orderId, UpdateOrderStatusDto dto)
    {
        using var tx = await _ctx.Database.BeginTransactionAsync();

        var order = await _orders.GetByIdAsync(orderId);
        if (order == null) return null;

        // If cancelling, restore inventory
        if (dto.Status.Equals("Cancelled", StringComparison.OrdinalIgnoreCase)
            && !order.OrderStatus.Equals("Cancelled", StringComparison.OrdinalIgnoreCase))
        {
            var stock = await _inventory.GetInventoryAsync(order.ProductId, order.BloodGroup);
            if (stock != null)
            {
                stock.QuantityAvailable += order.Quantity;
                stock.LastUpdated = DateTime.UtcNow;
                _ctx.Inventories.Update(stock);
            }
        }

        order.OrderStatus = dto.Status;
        await _orders.UpdateAsync(order);

        await _orders.AddStatusHistoryAsync(new OrderStatusHistory
        {
            OrderId = orderId,
            Status = dto.Status,
            UpdatedBy = dto.UpdatedBy,
            UpdatedAt = DateTime.UtcNow
        });

        await tx.CommitAsync();
        return _mapper.Map<OrderDto>(order);
    }

    public Task<bool> CancelAsync(int orderId, int updatedBy) =>
        UpdateStatusAsync(orderId, new UpdateOrderStatusDto { Status = "Cancelled", UpdatedBy = updatedBy })
        .ContinueWith(t => t.Result != null);
}
