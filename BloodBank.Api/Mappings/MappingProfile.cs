using AutoMapper;
using BloodBank.Api.DTOs;
using BloodBank.Api.DTOs.Orders;
using BloodBank.Api.DTOs.Payments;
using BloodBank.Api.DTOs.Inventory;
using BloodBank.Api.Models;

namespace BloodBank.Api.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Donor, DonorDto>();
        CreateMap<CreateDonorDto, Donor>();

        
        // Orders
        CreateMap<Order, OrderDto>();
        CreateMap<CreateOrderDto, Order>();
        CreateMap<UpdateOrderStatusDto, Order>();
        CreateMap<UpdateOrderDeliveryDto, Order>();

        // Payments
        CreateMap<Payment, PaymentDto>();
        CreateMap<CreatePaymentDto, Payment>();
        CreateMap<UpdatePaymentStatusDto, Payment>();

        // Inventory
        CreateMap<Inventory, InventoryDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName));

        CreateMap<CreateInventoryDto, Inventory>();
        CreateMap<UpdateInventoryDto, Inventory>();
    }
}