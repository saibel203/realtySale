using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RealtySale.Api.Extensions;
using RealtySale.Api.Repositories.IRepository;
using RealtySale.Api.Services.IService;
using RealtySale.Shared.Data;
using RealtySale.Shared.DTOs;
using RealtySale.Shared.Errors;

namespace RealtySale.Api.Controllers;

public class AccountController : BaseController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly IPhotoService _photoService;
    private readonly IMapper _mapper;

    public AccountController(IUnitOfWork unitOfWork, ITokenService tokenService, IMapper mapper, IPhotoService photoService)
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _mapper = mapper;
        _photoService = photoService;
    }

    [HttpGet("user/{username}")] // /api/account/user/{username}
    public async Task<IActionResult> GetUser(string username)
    {
        var result = await _unitOfWork.UserRepository.GetUserDataAsync(username);
        var error = new ApiError();

        if (!result.IsSuccess)
        {
            error.ErrorCode = NotFound().StatusCode;
            error.ErrorMessage = result.Message;
            return NotFound(error);
        }

        return Ok(result.User);
    }

    [HttpPost("login")] // /api/account/login
    public async Task<IActionResult> Login(UserDto loginReq)
    {
        var result = await _unitOfWork.UserRepository.AuthenticateAsync(loginReq.Username, loginReq.Password);
        var apiError = new ApiError();

        if (!result.IsSuccess)
        {
            apiError.ErrorCode = Unauthorized().StatusCode;
            apiError.ErrorMessage = result.Message;
            apiError.ErrorDetails = "This error appear when provided username or password does not exists";
            return Unauthorized(apiError);
        }
        
        var user = result.User;
        if (user is null) return Unauthorized(result.Message);

        var response = new LoginResDto();
        
        var userInfo = await _unitOfWork.UserRepository.GetUserDataAsync(user.Username);
        
        response.Username = user.Username;
        response.Token = _tokenService.GenerateToken(user);
        response.UserImage = userInfo.User?.UserImage!;
        return Ok(response);
    }

    [HttpPost("register")] // /api/account/register
    public async Task<IActionResult> Register(UserDto registerDto)
    {
        var error = new ApiError();
        var isUserExist = await _unitOfWork.UserRepository.IsUserExistsAsync(registerDto.Username);
        if (isUserExist.IsSuccess)
        {
            error.ErrorCode = BadRequest().StatusCode;
            error.ErrorMessage = isUserExist.Message;
            return BadRequest(error);
        }
        
        if (registerDto.Username.IsEmpty() || registerDto.Password.IsEmpty())
        {
            error.ErrorCode = BadRequest().StatusCode;
            error.ErrorMessage = "Username or Password can not be blank";
            return BadRequest(error);
        }

        var result = await _unitOfWork.UserRepository.RegisterAsync(registerDto.Username, registerDto.Password, registerDto.Email);

        if (result.IsSuccess)
        {
            await _unitOfWork.SaveAsync();
            return StatusCode(201);
        }

        return BadRequest(error);
    }

    [Authorize]
    [HttpPost("ChangePassword")] // /api/account/changePassword
    public async Task<IActionResult> ChangePassword(UserDto user)
    {
        var error = new ApiError();
        if (user.NewPassword is null)
        {
            error.ErrorCode = BadRequest().StatusCode;
            error.ErrorMessage = "Need enter password";
            return BadRequest(error);
        }
        var result = await _unitOfWork.UserRepository.ChangePasswordAsync(user.Username, user.Password, user.NewPassword);
    
        if (result.IsSuccess)
        {
            await _unitOfWork.SaveAsync();
            return StatusCode(200);
        }

        error.ErrorCode = BadRequest().StatusCode;
        error.ErrorMessage = result.Message;
        return BadRequest(error);
    }

    [Authorize]
    [HttpPut("ProfileChange/{username}")] // /api/account/profileChange/{username}
    public async Task<IActionResult> ProfileDataChange(UserUpdateDto userDto, string username)
    {
        var result = await _unitOfWork.UserRepository.GetUserDataAsync(username);
        var error = new ApiError();

        if (result.IsSuccess)
        {
            _mapper.Map(userDto, result.User);
            await _unitOfWork.SaveAsync();
            return Ok(result.User);
        }
        
        error.ErrorCode = NotFound().StatusCode;
        error.ErrorMessage = result.Message;
        
        return NotFound(error);
    }

    [Authorize]
    [HttpPatch("profileChangeImage/{username}")] // /api/account/profileChangeImage/{username}
    public async Task<IActionResult> ChangeUserImage(string username, [FromBody] JsonPatchDocument<User> user)
    {
        var result = await _unitOfWork.UserRepository.GetUserDataAsync(username);
        var error = new ApiError();
        
        if (result.IsSuccess)
        {
            user.ApplyTo(result.User!, ModelState);
            await _unitOfWork.SaveAsync();
            
            return StatusCode(201, new{ Message = result.User!.UserImage });
        }

        error.ErrorCode = NotFound().StatusCode;
        error.ErrorMessage = result.Message;
        
        return NotFound(error);
    }

    [Authorize]
    [HttpPost("uploadImage")] // /api/account/uploadImage
    public async Task<IActionResult> UploadImage([FromForm] UserImage image)
    {
        var result = await _photoService.UploadUserPhotoAsync(image);
        var error = new ApiError();

        if (result.IsSuccess)
            return StatusCode(201, new { Message = result.ImagePath });
        
        error.ErrorCode = BadRequest().StatusCode;
        error.ErrorMessage = result.Message;
        return BadRequest(error);
    }
}
