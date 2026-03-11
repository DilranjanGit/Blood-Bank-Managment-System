using BloodBank.Api.DTOs;

namespace BloodBank.Api.Services;
public interface IDonorService
{
    Task<IEnumerable<DonorDto>> GetAllDonors();
    Task<DonorDto> GetDonorById(int id);
    Task<DonorDto> CreateDonor(CreateDonorDto dto);
    Task<DonorDto> UpdateDonor(int id, CreateDonorDto dto);
    Task<bool> DeleteDonor(int id);
}