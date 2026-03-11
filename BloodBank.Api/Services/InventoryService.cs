using AutoMapper;
using BloodBank.Api.DTOs.Inventory;
using BloodBank.Api.Models;
using BloodBank.Api.Repositories;

namespace BloodBank.Api.Services;

public class InventoryService : IInventoryService
{
    private readonly IInventoryRepository _repository;
    private readonly IMapper _mapper;

    public InventoryService(IInventoryRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<InventoryDto>> GetAllAsync()
    {
        var list = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<InventoryDto>>(list);
    }

    public async Task<InventoryDto> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity == null ? null : _mapper.Map<InventoryDto>(entity);
    }

    public async Task<InventoryDto> CreateAsync(CreateInventoryDto dto)
    {
        // Prevent duplicate product/blood group combo
        var existing = await _repository.GetByProductGroupAsync(dto.ProductId, dto.BloodGroup);
        if (existing != null)
            throw new InvalidOperationException("Inventory record for this Product & Blood Group already exists.");

        var entity = _mapper.Map<Inventory>(dto);
        var created = await _repository.AddAsync(entity);

        return _mapper.Map<InventoryDto>(created);
    }

    public async Task<InventoryDto> UpdateAsync(int id, UpdateInventoryDto dto)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null) return null;

        entity.QuantityAvailable = dto.QuantityAvailable;
        var updated = await _repository.UpdateAsync(entity);

        return _mapper.Map<InventoryDto>(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }
}