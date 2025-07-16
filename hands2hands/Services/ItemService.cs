using System;
using hands2hands.DTOs;
using hands2hands.Models;
using hands2hands.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace hands2hands.Services;

public class ItemService : IItemService
{
    private readonly AppDbContext _context;
    public ItemService(AppDbContext context) => _context = context;

public async Task<IEnumerable<ItemResponseDto>> GetAllAsync()
    {
        var items = await _context.Items
            .Include(i => i.ItemCategories).ThenInclude(ic => ic.Category)
            .Include(i => i.Media)
            .ToListAsync();

        return items.Select(i => ToResponseDto(i));
    }

    public async Task<ItemResponseDto?> GetByIdAsync(Guid id)
    {
        var item = await _context.Items
            .Include(i => i.ItemCategories).ThenInclude(ic => ic.Category)
            .Include(i => i.Media)
            .FirstOrDefaultAsync(i => i.Id == id);

        return item is null ? null : ToResponseDto(item);
    }

    // public async Task<ItemResponseDto> CreateAsync(ItemRequestDto dto)
    // {
    //     var entity = new Item
    //     {
    //         Name        = dto.Name,
    //         Description = dto.Description,
    //         Price       = dto.Price,
    //         ItemCategories = dto.CategoryIds?
    //             .Select(cid => new ItemCategory { CategoryId = cid })
    //             .ToList() ?? new List<ItemCategory>(),
    //         Media = dto.Media?
    //             .Select(m => new Media { Type = m.Type, Url = m.Url })
    //             .ToList() ?? new List<Media>()
    //     };

    //     _context.Items.Add(entity);
    //     await _context.SaveChangesAsync();

    //     var created = await _context.Items
    //         .Include(i => i.ItemCategories).ThenInclude(ic => ic.Category)
    //         .Include(i => i.Media)
    //         .FirstOrDefaultAsync(i => i.Id == entity.Id);

    //     return ToResponseDto(created!);
    // }
    
    public async Task<ItemResponseDto> CreateAsync(ItemRequestDto dto)
    {
        List<Category> categories = new();
        if (dto.CategoryIds is { Count: > 0 })
        {
            categories = await _context.Categories
                .Where(c => dto.CategoryIds.Contains(c.Id))
                .ToListAsync();
        }

        var entity = new Item
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,

            ItemCategories = categories
                .Select(c => new ItemCategory { Category = c, CategoryId = c.Id })
                .ToList(),

            Media = dto.Media?
                .Select(m => new Media { Type = m.Type, Url = m.Url })
                .ToList() 
            ?? new List<Media>()
        };

        _context.Items.Add(entity);
        await _context.SaveChangesAsync();

        return ToResponseDto(entity);
    }


    private static ItemResponseDto ToResponseDto(Item i) => new()
    {
        Id = i.Id,
        Name = i.Name,
        Description = i.Description,
        Price = i.Price,
        CreatedAt = i.CreatedAt,
        UpdatedAt = i.UpdatedAt,
        Categories = i.ItemCategories
            .Select(ic => new CategoryInfoDto
            {
                Id = ic.Category.Id,
                Name = ic.Category.Name
            }).ToList(),
        Media = i.Media
            .Select(m => new MediaInfoDto
            {
                Id = m.Id,
                Type = m.Type,
                Url = m.Url
            }).ToList()
    };

    public async Task<ItemResponseDto> UpdateAsync(Guid id, ItemRequestDto dto)
    {
        var item = await _context.Items
            .Include(i => i.ItemCategories).ThenInclude(ic => ic.Category)
            .Include(i => i.Media)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (item is null) throw new Exception("Item not found");

        item.Name = dto.Name;
        item.Description = dto.Description;
        item.Price = dto.Price;
        item.UpdatedAt = DateTime.UtcNow;

        // Update categories
        if (dto.CategoryIds is { Count: > 0 })
        {
            var categories = await _context.Categories
                .Where(c => dto.CategoryIds.Contains(c.Id))
                .ToListAsync();

            item.ItemCategories = categories
                .Select(c => new ItemCategory { Category = c, CategoryId = c.Id })
                .ToList();
        }

        // Update media
        if (dto.Media is { Count: > 0 })
        {
            item.Media = dto.Media
                .Select(m => new Media { Type = m.Type, Url = m.Url })
                .ToList();
        }

        await _context.SaveChangesAsync();

        var updated = await _context.Items
            .Include(i => i.ItemCategories).ThenInclude(ic => ic.Category)
            .Include(i => i.Media)
            .FirstOrDefaultAsync(i => i.Id == item.Id);

        return ToResponseDto(updated!);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var item = await _context.Items
            .Include(i => i.ItemCategories)
            .Include(i => i.Media)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (item is null) return false;

        _context.Items.Remove(item);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<ItemResponseDto>> GetByCategoryIdAsync(int categoryId)
    {
        var items = await _context.Items
            .Include(i => i.ItemCategories)
            .ThenInclude(ic => ic.Category)
            .Include(i => i.Media)
            .Where(i => i.ItemCategories.Any(ic => ic.CategoryId == categoryId))
            .ToListAsync();

        return items.Select(i => ToResponseDto(i));
    }
}
