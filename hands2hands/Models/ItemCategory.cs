using System;

namespace hands2hands.Models;

public class ItemCategory
{
    public Guid ItemId { get; set; }
    public Item Item { get; set; } = null!;
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
}
