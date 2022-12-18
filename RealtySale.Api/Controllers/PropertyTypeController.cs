using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealtySale.Api.Repositories.IRepository;
using RealtySale.Shared.DTOs;
using RealtySale.Shared.Errors;

namespace RealtySale.Api.Controllers;

public class PropertyTypeController : BaseController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PropertyTypeController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet("list")] // /api/propertyType/list
    public async Task<IActionResult> GetAllPropertyTypes()
    {
        var propertyTypesResult = await _unitOfWork.PropertyTypeRepository.GetPropertyTypesAsync();
        var error = new ApiError();
        
        if (propertyTypesResult.IsSuccess)
        {
            var propertyTypes = propertyTypesResult.PropertyTypes;
            var propertyTypesDto = _mapper.Map<IEnumerable<KeyValuePairDto>>(propertyTypes);
            return Ok(propertyTypesDto);
        }

        error.ErrorCode = BadRequest().StatusCode;
        error.ErrorMessage = propertyTypesResult.Message;

        return BadRequest(error);
    }
}
