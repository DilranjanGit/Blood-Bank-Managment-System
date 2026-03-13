
namespace BloodBank.Api.DTOs;
public class CreateUserDto
{
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? Phone { get; set; }
    public int RoleId { get; set; }

    public string BloodGroup { get; set; } = null!;

    public int Age { get; set; }
    public string? Gender { get; set; }
    public string? Address { get; set; }

}