﻿using Microsoft.AspNetCore.Mvc;
using RealtySale.Api.Services.IService;
using RealtySale.Shared.Data;
using RealtySale.Shared.Responses;

namespace RealtySale.Api.Services.Service;

public class PhotoService : IPhotoService
{
    private readonly IWebHostEnvironment _environment;

    public PhotoService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<PhotoServiceResponse> UploadUserPhotoAsync([FromForm] UserImage image)
    {
        var uploadDirectory = _environment.WebRootPath + "\\Upload\\UserImages\\";
        var imageName = image.ImageName;
        var imageExtension = Path.GetExtension(image.Image?.FileName);

        if (image.Image?.Length > 0)
        {
            if (!Directory.Exists(uploadDirectory))
                Directory.CreateDirectory(uploadDirectory);

            await using FileStream fileStream = new(uploadDirectory + imageName + imageExtension, FileMode.Create);

            await image.Image.CopyToAsync(fileStream);
            await fileStream.FlushAsync();

            return new()
            {
                Message = "Image upload successfully",
                IsSuccess = true,
                ImagePath = @"/Upload/UserImages/" + imageName + imageExtension
            };
        }

        return new()
        {
            Message = "Fail image upload",
            IsSuccess = false
        };
    }
}
