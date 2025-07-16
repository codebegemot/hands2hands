using System;

namespace hands2hands.DTOs;

public class ItemResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public List<CategoryInfoDto> Categories { get; set; } = new();
    public List<MediaInfoDto> Media { get; set; } = new();
}
