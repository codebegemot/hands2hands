using System;
using hands2hands.Models;

namespace hands2hands.DTOs;

public class MediaInfoDto
{
    public int Id { get; set; }
    public MediaType Type { get; set; }
    public string Url { get; set; } = string.Empty;
}
