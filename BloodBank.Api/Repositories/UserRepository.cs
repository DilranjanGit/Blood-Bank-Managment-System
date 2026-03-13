using System.Linq;
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
        return await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == id);
    }

    public async Task<User> GetUserByEmail(string email)
    {
        return await _context.Users.Include(u=>u.Role).FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User> AddUser(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> UpdateUser(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<Role> GetRoleById(int roleId)
    {
        return await _context.Roles.FindAsync(roleId);
    }

    public async Task<PasswordResetToken> AddPasswordResetToken(PasswordResetToken token)
    {
        _context.PasswordResetTokens.Add(token);
        await _context.SaveChangesAsync();
        return token;
    }

    public async Task<IEnumerable<PasswordResetToken>> GetPasswordResetToken()
    {
        return await _context.PasswordResetTokens.Where(t => t.UsedAt == null && t.ExpiresAt > DateTime.UtcNow)
            .ToListAsync();
    }

    public async Task<PasswordResetToken> UpdatePasswordResetToken(PasswordResetToken token)
    {
        _context.PasswordResetTokens.Update(token);
        await _context.SaveChangesAsync();
        return token;
    }
}