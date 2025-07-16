using System;
using hands2hands.Models;

namespace hands2hands.DTOs;

public class MediaDto
{
    public MediaType Type { get; set; }
    public string Url { get; set; } = string.Empty;
}
