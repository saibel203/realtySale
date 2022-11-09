using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using RealtySale.Api.Services.IService;

namespace RealtySale.Api.Services.Service;

public class PhotoService : IPhotoService
{
    private readonly Cloudinary _cloudinary;
    
    public PhotoService(IConfiguration configuration)
    {
        Account account = new(
            configuration["CloudinarySettings:CloudName"], 
            configuration["CloudinarySettings:CloudApiKey"], 
            configuration["CloudinarySettings:CloudApiSecret"]);

        _cloudinary = new Cloudinary(account);
    }
    
    public async Task<ImageUploadResult> UploadPhotoAsync(IFormFile photo)
    {
        var uploadResult = new ImageUploadResult();

        if (photo.Length > 0)
        {
            await using var stream = photo.OpenReadStream();
            
            var uploadParameters = new ImageUploadParams
            {
                File = new FileDescription(photo.FileName, stream),
                Transformation = new Transformation().Height(500).Width(800)
            };

            uploadResult = await _cloudinary.UploadAsync(uploadParameters);
        }

        return uploadResult;
    }
}
