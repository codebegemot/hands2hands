using System;

namespace hands2hands.Models;

public enum MediaType
{
    Image,
    Video
}

public class Media
{
    public int Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public MediaType Type { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Foreign key
    public Guid ItemId { get; set; }
    // Navigation property
    public Item Item { get; set; } = null!;
}
