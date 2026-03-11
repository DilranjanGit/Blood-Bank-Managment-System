
namespace BloodBank.Api.DTOs;

public class DonorDto
{
    public int DonorId { get; set; }
    public int UserId { get; set; }
    public string BloodGroup { get; set; }
    public int Age { get; set; }
    public string Gender { get; set; }
    public string Address { get; set; }
}