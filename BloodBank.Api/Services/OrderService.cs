using AutoMapper;
using BloodBank.Api.Models;
using BloodBank.Api.Repositories;
using Microsoft.EntityFrameworkCore;
using BloodBank.Api.DTOs.Orders;
using System.Security.Claims;

namespace BloodBank.Api.Services;

public class OrderService : IOrderService
{
    private readonly BloodBankContext _ctx;
    private readonly IOrderRepository _orders;
    private readonly IInventoryQueries _inventory;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private int _currentUserId => int.Parse(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
    public OrderService(BloodBankContext ctx, IOrderRepository orders, IInventoryQueries inventory, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _ctx = ctx;
        _orders = orders;
        _inventory = inventory;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<OrderDto> PlaceOrderAsync(CreateOrderDto dto)
    {
        // Transaction ensures consistency between Orders, Inventory, and History
        int _currentUserId =int.Parse(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
        using var tx = await _ctx.Database.BeginTransactionAsync();
        var orderNumber = _ctx.Orders.OrderByDescending(o => o.OrderId).Select(o => o.OrderNumber).FirstOrDefault();    
        var stock = await _inventory.GetInventoryAsync(dto.ProductId, dto.BloodGroup);
        if (stock == null || stock.QuantityAvailable < dto.Quantity)
            throw new InvalidOperationException("Insufficient inventory for the requested product/blood group.");

        // Reserve inventory
        stock.QuantityAvailable -= dto.Quantity;
        stock.LastUpdated = DateTime.UtcNow;
        _ctx.Inventories.Update(stock);

        // Create order
        var order = _mapper.Map<Order>(dto);
        order.OrderNumber = GenerateRandomOrder(orderNumber);
        order.OrderStatus = "Pending"; // or "AwaitingPayment"
        order.UserId = _currentUserId; // Assuming UserId is passed in DTO or obtained from context
        order.CreatedAt = DateTime.UtcNow;

        _ctx.Orders.Add(order);
        await _ctx.SaveChangesAsync();

        // Status history
        var history = new OrderStatusHistory
        {
            OrderId = order.OrderId,
            Status = order.OrderStatus,
            UpdatedBy = _currentUserId,
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

    public async Task<OrderDto> UpdateDeliveryAsync(int orderId, UpdateOrderDeliveryDto dto)
    {
        var order = await _orders.GetByIdAsync(orderId);
        if (order == null) return null;

        order.DeliveryDate = dto.DeliveryDate;
        if(!string.IsNullOrEmpty(dto.DeliveryAddress))
        {
            order.DeliveryAddress = dto.DeliveryAddress;
        }

        await _orders.UpdateAsync(order);

        await _orders.AddStatusHistoryAsync(new OrderStatusHistory
        {
            OrderId = orderId,
            Status = $"DeliveryUpdated",
            UpdatedBy = _currentUserId,
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
            UpdatedBy = _currentUserId,
            UpdatedAt = DateTime.UtcNow
        });

        await tx.CommitAsync();
        return _mapper.Map<OrderDto>(order);
    }

    public Task<bool> CancelAsync(int orderId) =>
        UpdateStatusAsync(orderId, new UpdateOrderStatusDto { Status = "Cancelled", UpdatedBy = _currentUserId })
        .ContinueWith(t => t.Result != null);
    public string GenerateRandomOrder(string lastOrderNumber = null)
    {
        if(lastOrderNumber == null)
        {
           return "DON-00000001";
        }    
        string code =(int.Parse(lastOrderNumber.Split('-')[1]) + 1).ToString("D8");
        return $"DON-{code}";
    }

}
