using BloodBank.Api.DTOs;


namespace BloodBank.Api.Services;
public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllUsers();
    Task<UserDto> GetUserById(int id);
    Task<UserDto> GetUserByEmail(string email);
    Task<UserDto> AddUser(CreateUserDto user);
}