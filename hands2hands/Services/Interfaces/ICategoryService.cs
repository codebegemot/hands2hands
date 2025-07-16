using System;
using hands2hands.DTOs;

namespace hands2hands.Services.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryResponseDto>> GetAllAsync();
    Task<CategoryResponseDto?> GetByIdAsync(int id);
    Task<CategoryResponseDto> CreateAsync(CategoryRequestDto dto);
    Task<CategoryResponseDto> UpdateAsync(int id, CategoryRequestDto dto);
    Task<bool> DeleteAsync(int id);
}
