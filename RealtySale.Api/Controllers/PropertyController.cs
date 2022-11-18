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

    [HttpPost("add/photo/{propId}")] // /api/property/add/photo/{propId}
    public async Task<IActionResult> AddPropertyPhoto(IFormFile file, int propId)
    {
        var result = await _photoService.UploadPhotoAsync(file);
    
        if (result.Error is not null)
            return BadRequest(result.Error.Message);
    
        var resultResponse = await _unitOfWork.PropertyRepository.GetPropertyByIdAsync(propId);

        if (resultResponse.IsSuccess)
        {
            var property = resultResponse.Property;
            var photo = new Photo
            {
                PublicId = result.PublicId,
                ImageUrl = result.SecureUrl.AbsoluteUri
            };
    
            if (property?.Photos?.Count == 0)
                photo.IsPrimary = true;
    
            property?.Photos?.Add(photo);
            await _unitOfWork.SaveAsync();
            
            return StatusCode(201);   
        }

        return BadRequest();
    }
}
