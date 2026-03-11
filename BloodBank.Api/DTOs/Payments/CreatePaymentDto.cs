
namespace BloodBank.Api.DTOs.Payments;


public class CreatePaymentDto
{
    public int OrderId { get; set; }
    public string PaymentMethod { get; set; }
    public string PaymentGateway { get; set; }
    public string TransactionId { get; set; }
    public decimal AmountPaid { get; set; }
    public string PaymentStatus { get; set; } // initial status from gateway response
}
