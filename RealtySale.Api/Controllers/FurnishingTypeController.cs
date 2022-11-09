using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealtySale.Api.Repositories.IRepository;
using RealtySale.Shared.DTOs;

namespace RealtySale.Api.Controllers;

public class FurnishingTypeController : BaseController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public FurnishingTypeController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet("list")] // /api/furnishingType/list
    public async Task<IActionResult> GetAllFurnishingTypes()
    {
        var furnishingTypes = await _unitOfWork.FurnishingTypeRepository.GetFurnishingTypesAsync();
        var furnishingTypesDto = _mapper.Map<IEnumerable<KeyValuePairDto>>(furnishingTypes);
        return Ok(furnishingTypesDto);
    }
}
