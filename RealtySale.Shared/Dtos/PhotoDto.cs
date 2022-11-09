namespace RealtySale.Shared.DTOs;

public class PhotoDto
{
    public string ImageUrl { get; set; } = string.Empty;
    public string PublicId { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }
}
