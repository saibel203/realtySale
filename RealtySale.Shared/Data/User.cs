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
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}
