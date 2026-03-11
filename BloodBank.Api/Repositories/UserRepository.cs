using BloodBank.Api.DTOs;
using BloodBank.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BloodBank.Api.Repositories;
public class UserRepository : IUserRepository
{
    private readonly BloodBankContext _context;

    public UserRepository(BloodBankContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> GetUserById(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User> GetUserByEmail(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User> AddUser(CreateUserDto user)
    {
        var userEntity = new User
        {
            FullName = user.FullName,
            Email = user.Email,
            PasswordHash = user.Password, // Note: In a real application, you should hash the password before storing it
            Phone = user.Phone,
            RoleId = user.RoleId
        };

        _context.Users.Add(userEntity);
        await _context.SaveChangesAsync();
        return userEntity;
    }
}