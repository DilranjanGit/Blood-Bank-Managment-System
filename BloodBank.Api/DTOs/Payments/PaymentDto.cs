
namespace BloodBank.Api.DTOs.Payments;


public class PaymentDto
{
    public int PaymentId { get; set; }
    public int OrderId { get; set; }
    public string PaymentMethod { get; set; } // Card/UPI/NetBanking
    public string PaymentGateway { get; set; } // Stripe/PayPal/etc.
    public string TransactionId { get; set; }
    public decimal AmountPaid { get; set; }
    public string PaymentStatus { get; set; } // Success/Failed/Pending
    public DateTime PaymentDate { get; set; }
}
