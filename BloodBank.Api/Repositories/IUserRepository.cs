using BloodBank.Api.Models;

namespace BloodBank.Api.Repositories;
public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllUsers();
    Task<User> GetUserById(int id);
    Task<User> GetUserByEmail(string email);
    Task<User> AddUser(User user);
    Task<User> UpdateUser(User user);
    Task<Role> GetRoleById(int roleId);
    Task<PasswordResetToken> AddPasswordResetToken(PasswordResetToken token);
    Task<IEnumerable<PasswordResetToken>> GetPasswordResetToken();
    Task<PasswordResetToken> UpdatePasswordResetToken(PasswordResetToken token);
}