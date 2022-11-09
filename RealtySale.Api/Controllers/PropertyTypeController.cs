using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealtySale.Api.Repositories.IRepository;
using RealtySale.Shared.DTOs;

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
        var propertyTypes = await _unitOfWork.PropertyTypeRepository.GetPropertyTypesAsync();
        var propertyTypesDto = _mapper.Map<IEnumerable<KeyValuePairDto>>(propertyTypes);
        return Ok(propertyTypesDto);
    }
}
