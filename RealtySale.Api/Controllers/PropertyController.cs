using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealtySale.Api.Repositories.IRepository;
using RealtySale.Api.Services.IService;
using RealtySale.Shared.Data;
using RealtySale.Shared.DTOs;

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
        var properties = await _unitOfWork.PropertyRepository.GetPropertiesAsync(sellRent);
        var propertyListDto = _mapper.Map<IEnumerable<PropertyListDto>>(properties);
        return Ok(propertyListDto);
    }

    [HttpGet("detail/{id}")] // /api/property/detail/{id}
    public async Task<IActionResult> GetPropertyDetail(long id)
    {
        var property = await _unitOfWork.PropertyRepository.GetPropertyDetailsAsync(id);
        var propertyDto = _mapper.Map<PropertyDetailDto>(property);
        return Ok(propertyDto);
    }

    [HttpPost("add")] // /api/property/add
    [Authorize]
    public async Task<IActionResult> AddProperty(PropertyDto propertyDto)
    {
        var property = _mapper.Map<Property>(propertyDto);
        var userId = GetUserId();
        
        property.PostedBy = userId;
        property.LastUpdatedBy = userId;
        await _unitOfWork.PropertyRepository.AddPropertyAsync(property);
        await _unitOfWork.SaveAsync();
        
        return StatusCode(201);
    }

    [HttpPost("add/photo/{propId}")] // /api/property/add/photo/{propId}
    public async Task<IActionResult> AddPropertyPhoto(IFormFile file, int propId)
    {
        var result = await _photoService.UploadPhotoAsync(file);

        if (result.Error is not null)
            return BadRequest(result.Error.Message);

        var property = await _unitOfWork.PropertyRepository.GetPropertyByIdAsync(propId);
        var photo = new Photo
        {
            PublicId = result.PublicId,
            ImageUrl = result.SecureUrl.AbsoluteUri
        };

        if (property.Photos?.Count == 0)
            photo.IsPrimary = true;

        property.Photos?.Add(photo);
        await _unitOfWork.SaveAsync();
            
        return StatusCode(201);
    }
}
