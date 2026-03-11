using BloodBank.Api.DTOs;
using BloodBank.Api.Models;

namespace BloodBank.Api.Repositories;
public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllUsers();
    Task<User> GetUserById(int id);
    Task<User> GetUserByEmail(string email);
    Task<User> AddUser(CreateUserDto user);
}