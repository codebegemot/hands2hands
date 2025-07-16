using System;
using hands2hands.DTOs;

namespace hands2hands.Services.Interfaces;

public interface IItemService
{
    Task<IEnumerable<ItemResponseDto>> GetAllAsync();
    Task<ItemResponseDto?> GetByIdAsync(Guid id);
    Task<ItemResponseDto> CreateAsync(ItemRequestDto dto);
    Task<ItemResponseDto> UpdateAsync(Guid id, ItemRequestDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<IEnumerable<ItemResponseDto>> GetByCategoryIdAsync(int categoryId);
}
