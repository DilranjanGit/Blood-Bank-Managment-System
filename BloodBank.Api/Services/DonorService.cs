using AutoMapper;
using BloodBank.Api.DTOs;
using BloodBank.Api.Models;
using BloodBank.Api.Repositories;

namespace BloodBank.Api.Services;
public class DonorService : IDonorService
{
    private readonly IDonorRepository _repository;
    private readonly IMapper _mapper;

    public DonorService(IDonorRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DonorDto>> GetAllDonors()
    {
        var donors = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<DonorDto>>(donors);
    }

    public async Task<DonorDto> GetDonorById(int id)
    {
        var donor = await _repository.GetByIdAsync(id);
        return donor == null ? null : _mapper.Map<DonorDto>(donor);
    }

    public async Task<DonorDto> CreateDonor(CreateDonorDto dto)
    {
        var donor = _mapper.Map<Donor>(dto);
        donor = await _repository.CreateAsync(donor);
        return _mapper.Map<DonorDto>(donor);
    }

    public async Task<DonorDto> UpdateDonor(int id, CreateDonorDto dto)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null) return null;

        _mapper.Map(dto, existing);
        await _repository.UpdateAsync(existing);

        return _mapper.Map<DonorDto>(existing);
    }

    public async Task<bool> DeleteDonor(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}