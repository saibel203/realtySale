namespace RealtySale.Shared.DTOs;

public class PhotoDto
{
    public int Id { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }
}
