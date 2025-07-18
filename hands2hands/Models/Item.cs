using System;

namespace hands2hands.Models;

public class Item
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<ItemCategory> ItemCategories { get; set; } = new List<ItemCategory>();
    public ICollection<Media> Media { get; set; } = new List<Media>();
}
