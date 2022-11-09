using CloudinaryDotNet.Actions;

namespace RealtySale.Api.Services.IService;

public interface IPhotoService
{
    Task<ImageUploadResult> UploadPhotoAsync(IFormFile photo);
}