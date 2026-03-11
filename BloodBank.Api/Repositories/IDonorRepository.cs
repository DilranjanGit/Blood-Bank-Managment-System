using BloodBank.Api.Models; 

namespace BloodBank.Api.Repositories;

public interface IDonorRepository
{
    Task<IEnumerable<Donor>> GetAllAsync();
    Task<Donor> GetByIdAsync(int id);
    Task<Donor> CreateAsync(Donor donor);
    Task<Donor> UpdateAsync(Donor donor);
    Task<bool> DeleteAsync(int id);
}