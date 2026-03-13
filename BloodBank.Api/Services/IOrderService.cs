using BloodBank.Api.Models;
using BloodBank.Api.Repositories;
using Microsoft.EntityFrameworkCore;
using BloodBank.Api.DTOs.Orders;

namespace BloodBank.Api.Services;

// Services/IOrderService.cs
public interface IOrderService
{
    Task<OrderDto> PlaceOrderAsync(CreateOrderDto dto);
    Task<IEnumerable<OrderDto>> GetAllAsync();
    Task<IEnumerable<OrderDto>> GetByUserAsync(int userId);
    Task<OrderDto> GetByIdAsync(int orderId);
    Task<OrderDto> UpdateDeliveryAsync(int orderId, UpdateOrderDeliveryDto dto);
    Task<OrderDto> UpdateStatusAsync(int orderId, UpdateOrderStatusDto dto);
    Task<bool> CancelAsync(int orderId);
}