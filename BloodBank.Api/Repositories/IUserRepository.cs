using BloodBank.Api.Models;

namespace BloodBank.Api.Repositories;
public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllUsers();
    Task<User> GetUserById(int id);
    Task AddUser(User user);
}