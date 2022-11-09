using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RealtySale.Api.Repositories.IRepository;
using RealtySale.Shared.Data;
using RealtySale.Shared.DTOs;

namespace RealtySale.Api.Controllers;

public class CityController : BaseController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public CityController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet("getAll")] // /api/city/getAll
    public async Task<ActionResult<IEnumerable<City>>> GetAllCities()
    {
        var result = await _unitOfWork.CityRepository.GetAllCitiesAsync();

        var citiesDto = _mapper.Map<IEnumerable<CityDto>>(result.CitiesList);

        if (result.IsSuccess)
            return Ok(citiesDto);
        
        return BadRequest(result.Message);
    }

    [HttpPost("new")] // /api/city/new
    public async Task<ActionResult<CityDto>> NewCity(CityDto? cityDto)
    {
        var city = _mapper.Map<City>(cityDto);
        var result = await _unitOfWork.CityRepository.NewCityAsync(city);
        
        if (result.IsSuccess)
        {
            await _unitOfWork.SaveAsync();
            return Ok(result.Message);
        }

        return BadRequest(result.Message);
    }

    [HttpPut("update/{id}")] // /api/city/update/{id}
    public async Task<ActionResult<CityDto>> UpdateCity(long id, CityDto? cityDto)
    {
        if (id != cityDto?.Id)
            return BadRequest("City not found");
        
        var result = await _unitOfWork.CityRepository.GetCityAsync(id);

        if (result.IsSuccess)
        {
            _mapper.Map(cityDto, result.City);
            await _unitOfWork.SaveAsync();

            return Ok(result.City);   
        }

        return NotFound(result.Message);
    }
    
    [HttpPut("updateCityName/{id}")] // /api/city/updateCityName/{id}
    public async Task<ActionResult<CityDto>> UpdateCity(long id, CityUpdateDto? cityDto)
    {
        var result = await _unitOfWork.CityRepository.GetCityAsync(id);

        if (result.IsSuccess)
        {
            _mapper.Map(cityDto, result.City);
            await _unitOfWork.SaveAsync();

            return Ok(result.City);   
        }

        return NotFound(result.Message);
    }

    [HttpPatch("update/{id}")] // /api/city/update/{id}
    public async Task<ActionResult<CityDto>> UpdateCityPatch(long id, [FromBody]JsonPatchDocument<City> cityToPatch)
    {
        var result = await _unitOfWork.CityRepository.GetCityAsync(id);

        if (result.IsSuccess)
        {
            cityToPatch.ApplyTo(result.City!, ModelState);
            await _unitOfWork.SaveAsync();

            return Ok(result.City);   
        }

        return NotFound(result.Message);
    }
        

    [HttpDelete("delete/{id}")] // /api/city/delete/{id}
    public async Task<IActionResult> DeleteCity(long id)
    {
        var result = await _unitOfWork.CityRepository.DeleteCityAsync(id);

        if (result.IsSuccess)
        {
            await _unitOfWork.SaveAsync();
            return Ok(result.Message);
        }

        return NotFound(result.Message);
    }
}
