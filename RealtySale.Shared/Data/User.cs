using System.ComponentModel.DataAnnotations;

namespace RealtySale.Shared.Data;

public class User : BaseDataEntity
{
    [Required] 
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required] 
    public byte[] Password { get; set; } = Array.Empty<byte>();

    public byte[]? PasswordKey { get; set; } = Array.Empty<byte>();
    public string? FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; } = string.Empty;
    public string? TelegramLink { get; set; } = string.Empty;
    public string? InstagramLink { get; set; } = string.Empty;
    public string? FacebookLink { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public string? UserImage { get; set; } = string.Empty;
    public string? Phone { get; set; } = string.Empty;
}
