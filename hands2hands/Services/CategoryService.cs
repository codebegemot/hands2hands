using System;
using hands2hands.DTOs;
using hands2hands.Models;
using hands2hands.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace hands2hands.Services;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;
    public CategoryService(AppDbContext context) => _context = context;

    public async Task<IEnumerable<CategoryResponseDto>> GetAllAsync()
    {
        var categories = await _context.Categories
            .ToListAsync();

        return categories.Select(c => new CategoryResponseDto
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            CreatedAt = c.CreatedAt,
            UpdatedAt = c.UpdatedAt
        });
    }

    public async Task<CategoryResponseDto?> GetByIdAsync(int id)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == id);

        return category is null ? null : new CategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt
        };
    }

    public async Task<CategoryResponseDto> CreateAsync(CategoryRequestDto dto)
    {
        var entity = new Category
        {
            Name = dto.Name,
            Description = dto.Description ?? string.Empty,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Categories.Add(entity);
        await _context.SaveChangesAsync();

        return new CategoryResponseDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description ?? string.Empty,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }

    public async Task<CategoryResponseDto> UpdateAsync(int id, CategoryRequestDto dto)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category is null) throw new Exception("Category not found");

        category.Name = dto.Name;
        category.Description = dto.Description ?? string.Empty;
        category.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new CategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description ?? string.Empty,
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category is null) return false;

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return true;
    }
}
