using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealtySale.Api.Repositories.IRepository;
using RealtySale.Api.Services.IService;
using RealtySale.Shared.Data;
using RealtySale.Shared.DTOs;
using RealtySale.Shared.Errors;

namespace RealtySale.Api.Controllers;

public class PropertyController : BaseController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IPhotoService _photoService;

    public PropertyController(IUnitOfWork unitOfWork, IMapper mapper, IPhotoService photoService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _photoService = photoService;
    }

    [HttpGet("list/{sellRent}")] // /api/property/list/{sellRent} (0/1)
    public async Task<IActionResult> GetPropertiesList(byte sellRent)
    {
        var result = await _unitOfWork.PropertyRepository.GetPropertiesAsync(sellRent);
        if (result.IsSuccess)
        {
            var properties = result.Properties;
            var propertyListDto = _mapper.Map<IEnumerable<PropertyListDto>>(properties);
            return Ok(propertyListDto);
        }

        return BadRequest();
    }

    [HttpGet("detail/{id}")] // /api/property/detail/{id}
    public async Task<IActionResult> GetPropertyDetail(long id)
    {
        var result = await _unitOfWork.PropertyRepository.GetPropertyDetailsAsync(id);
        if (result.IsSuccess)
        {
            var property = result.Property;
            var propertyDto = _mapper.Map<PropertyDetailDto>(property);
            return Ok(propertyDto);
        }

        return BadRequest();
    }

    [HttpPost("add")] // /api/property/add
    [Authorize]
    public async Task<IActionResult> AddProperty(PropertyDto propertyDto)
    {
        var property = _mapper.Map<Property>(propertyDto);
        var userId = GetUserId();
        var error = new ApiError();
        
        property.PostedBy = userId;
        property.LastUpdatedBy = userId;
        
        var result = await _unitOfWork.PropertyRepository.AddPropertyAsync(property);
        if (result.IsSuccess)
        {
            await _unitOfWork.SaveAsync();
            return StatusCode(201);
        }

        error.ErrorCode = BadRequest().StatusCode;
        error.ErrorMessage = result.Message;
        return BadRequest(error);
    }

    [HttpPost("add/photo/{propertyId}")] // /api/property/add/photo/{propertyId}
    [Authorize]
    public async Task<IActionResult> AddPropertyPhoto(IFormFile file, long propertyId)
    {
        var userId = GetUserId();
        var result = await _photoService.UploadPropertyPhotoAsync(file);
        var error = new ApiError();

        if (!result.IsSuccess)
        {
            error.ErrorCode = BadRequest().StatusCode;
            error.ErrorMessage = result.Message;
            return BadRequest(error);
        }

        var propertyResult = await _unitOfWork.PropertyRepository.GetPropertyByIdAsync(propertyId);

        if (propertyResult.IsSuccess)
        {
            var property = propertyResult.Property!;

            if (property.PostedBy != userId)
            {
                error.ErrorCode = BadRequest().StatusCode;
                error.ErrorMessage = "You are not authorized to change the photo";
                return BadRequest(error);
            }

            var photo = new Photo
            {
                ImageUrl = result.ImagePath
            };

            if (property.Photos?.Count == 0)
                photo.IsPrimary = true;

            property.Photos?.Add(photo);
            await _unitOfWork.SaveAsync();

            return StatusCode(201, new { ImageUrl = result.ImagePath });
        }

        error.ErrorCode = BadRequest().StatusCode;
        error.ErrorMessage = result.Message;
        return BadRequest(error);
    }

    [HttpPost("set-primary-photo/{propertyId}/{photoId}")] // /api/property/set-primary-photo/{propertyId}/{photoId}
    [Authorize]
    public async Task<IActionResult> SetPrimaryPhoto(int propertyId, int photoId)
    {
        var userId = GetUserId();
        
        var propertyResult = await _unitOfWork.PropertyRepository.GetPropertyByIdAsync(propertyId);
        var property = propertyResult.Property;

        var error = new ApiError();
        
        if (!propertyResult.IsSuccess)
        {
            error.ErrorCode = BadRequest().StatusCode;
            error.ErrorMessage = propertyResult.Message;
            return BadRequest(error);
        }

        if (property is null)
        {
            error.ErrorCode = BadRequest().StatusCode;
            error.ErrorMessage = "No such property or photo exists";
            return BadRequest(error);
        }

        if (property.PostedBy != userId)
        {
            error.ErrorCode = BadRequest().StatusCode;
            error.ErrorMessage = "You are not authorized to change the photo";
            return BadRequest(error);
        }

        if (property.Photos is null)
        {
            error.ErrorCode = BadRequest().StatusCode;
            error.ErrorMessage = "Property hasn't photo";
            return BadRequest(error);
        }

        var photo = property.Photos.FirstOrDefault(p => p.Id == photoId);

        if (photo is null)
        {
            error.ErrorCode = BadRequest().StatusCode;
            error.ErrorMessage = "No such property or photo exists";
            return BadRequest(error);
        }

        if (photo.IsPrimary)
        {
            error.ErrorCode = BadRequest().StatusCode;
            error.ErrorMessage = "This is already a primary photo";
            return BadRequest(error);
        }

        var currentPrimary = property.Photos.FirstOrDefault(p => p.IsPrimary);

        if (currentPrimary is not null)
            currentPrimary.IsPrimary = false;
        
        photo.IsPrimary = true;

        if (await _unitOfWork.SaveAsync())
            return NoContent();

        
        error.ErrorCode = BadRequest().StatusCode;
        error.ErrorMessage = "Some error has occured, failed to set primary photo";
        return BadRequest(error);
    }
    
    [HttpDelete("delete-photo/{propertyId}/{photoId}")] // /api/property/delete-photo/{propertyId}/{photoId}
    [Authorize]
    public async Task<IActionResult> DeletePhoto(int propertyId, int photoId)
    {
        var userId = GetUserId();
        
        var propertyResult = await _unitOfWork.PropertyRepository.GetPropertyByIdAsync(propertyId);
        var property = propertyResult.Property;

        var error = new ApiError();
        
        if (!propertyResult.IsSuccess)
        {
            error.ErrorCode = BadRequest().StatusCode;
            error.ErrorMessage = propertyResult.Message;
            return BadRequest(error);
        }

        if (property is null)
        {
            error.ErrorCode = BadRequest().StatusCode;
            error.ErrorMessage = "No such property or photo exists";
            return BadRequest(error);
        }

        if (property.PostedBy != userId)
        {
            error.ErrorCode = BadRequest().StatusCode;
            error.ErrorMessage = "You are not authorized to delete the photo";
            return BadRequest(error);
        }

        if (property.Photos is null)
        {
            error.ErrorCode = BadRequest().StatusCode;
            error.ErrorMessage = "Property hasn't photo";
            return BadRequest(error);
        }

        var photo = property.Photos.FirstOrDefault(p => p.Id == photoId);

        if (photo is null)
        {
            error.ErrorCode = BadRequest().StatusCode;
            error.ErrorMessage = "No such property or photo exists";
            return BadRequest(error);
        }

        if (photo.IsPrimary)
        {
            error.ErrorCode = BadRequest().StatusCode;
            error.ErrorMessage = "You can not delete primary photo";
            return BadRequest(error);
        }

        property.Photos.Remove(photo);

        if (await _unitOfWork.SaveAsync())
            return Ok();

        
        error.ErrorCode = BadRequest().StatusCode;
        error.ErrorMessage = "Some error has occured, failed to delete photo";
        return BadRequest(error);
    }
    
    [HttpGet("getAll/{username}")]
    public async Task<IActionResult> GetAllUserProperties(string username)
    {
        var result = await _unitOfWork.PropertyRepository.GetUserPropertiesAsync(username);
        var error = new ApiError();
        
        if (result.IsSuccess)
            return Ok(result.Properties);

        error.ErrorCode = NotFound().StatusCode;
        error.ErrorMessage = result.Message;
        
        return NotFound(error);
    }
}
