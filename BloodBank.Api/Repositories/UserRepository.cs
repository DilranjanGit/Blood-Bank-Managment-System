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

    public async Task AddUser(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }
}