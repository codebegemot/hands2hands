using System;

namespace hands2hands.DTOs;

public class ItemRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public List<int> CategoryIds { get; set; } = new List<int>();
    public List<MediaDto> Media { get; set; } = new List<MediaDto>();
}
