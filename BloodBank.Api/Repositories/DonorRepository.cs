using BloodBank.Api.Models;
using Microsoft.EntityFrameworkCore;    

namespace BloodBank.Api.Repositories;
public class DonorRepository : IDonorRepository
{
    private readonly BloodBankContext _context;

    public DonorRepository(BloodBankContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Donor>> GetAllAsync()
    {
        return await _context.Donors
            .Include(d => d.User)
            .ToListAsync();
    }

    public async Task<Donor> GetByIdAsync(int id)
    {
        return await _context.Donors
            .Include(d => d.User)
            .FirstOrDefaultAsync(x => x.DonorId == id);
    }

    public async Task<Donor> CreateAsync(Donor donor)
    {
        _context.Donors.Add(donor);
        await _context.SaveChangesAsync();
        return donor;
    }

    public async Task<Donor> UpdateAsync(Donor donor)
    {
        _context.Donors.Update(donor);
        await _context.SaveChangesAsync();
        return donor;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var donor = await _context.Donors.FindAsync(id);
        if (donor == null) return false;

        _context.Donors.Remove(donor);
        await _context.SaveChangesAsync();
        return true;
    }
}