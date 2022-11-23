using Microsoft.AspNetCore.Http;

namespace RealtySale.Shared.Data;

public class UserImage
{
    public IFormFile? Image { get; set; }
    public string ImageName { get; set; } = string.Empty;
}
