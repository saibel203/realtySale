using Microsoft.AspNetCore.Mvc;
using RealtySale.Shared.Data;
using RealtySale.Shared.Responses;

namespace RealtySale.Api.Services.IService;

public interface IPhotoService
{
    Task<PhotoServiceResponse> UploadUserPhotoAsync([FromForm] UserImage image);
}