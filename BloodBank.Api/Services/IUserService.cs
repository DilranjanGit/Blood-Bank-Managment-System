using BloodBank.Api.DTOs;


namespace BloodBank.Api.Services;
public interface IUserService
{
    Task<IEnumerable<CreateUserDto>> GetAllUsers();
    Task<CreateUserDto> GetUserById(int id);
    Task<CreateUserDto> GetUserByEmail(string email);
    Task<CreateUserDto> AddUser(CreateUserDto user);
}