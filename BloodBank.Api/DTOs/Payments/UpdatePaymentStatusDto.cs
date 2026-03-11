
namespace BloodBank.Api.DTOs.Payments;


public class UpdatePaymentStatusDto
{
    public string PaymentStatus { get; set; } // Success/Failed/Pending
    public string TransactionId { get; set; } // Idempotency safety for webhooks
}
