using BloodBank.Api.DTOs.Payments;

namespace BloodBank.Api.Services;


public interface IPaymentService
{
    Task<PaymentDto> CreateAsync(CreatePaymentDto dto);
    Task<IEnumerable<PaymentDto>> GetByOrderAsync(int orderId);
    Task<PaymentDto> GetByIdAsync(int paymentId);
    Task<PaymentDto> UpdateStatusAsync(int paymentId, UpdatePaymentStatusDto dto);
    Task HandleGatewayWebhookAsync(string transactionId, string newStatus);
}