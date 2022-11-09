using Microsoft.AspNetCore.Mvc;
using RealtySale.Api.Extensions;
using RealtySale.Api.Repositories.IRepository;
using RealtySale.Api.Services.IService;
using RealtySale.Shared.DTOs;
using RealtySale.Shared.Errors;

namespace RealtySale.Api.Controllers;

public class AccountController : BaseController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;

    public AccountController(IUnitOfWork unitOfWork, ITokenService tokenService)
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
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
        response.Username = user.Username;
        response.Token = _tokenService.GenerateToken(user);
        return Ok(response);
    }

    [HttpPost("register")] // /api/account/register
    public async Task<IActionResult> Register(UserDto loginReq)
    {
        var error = new ApiError();
        var isUserExist = await _unitOfWork.UserRepository.IsUserExistsAsync(loginReq.Username);
        if (isUserExist.IsSuccess)
        {
            error.ErrorCode = BadRequest().StatusCode;
            error.ErrorMessage = isUserExist.Message;
            return BadRequest(error);
        }
        
        if (loginReq.Username.IsEmpty() || loginReq.Password.IsEmpty())
        {
            error.ErrorCode = BadRequest().StatusCode;
            error.ErrorMessage = "Username or Password can not be blank";
            return BadRequest(error);
        }

        var result = await _unitOfWork.UserRepository.RegisterAsync(loginReq.Username, loginReq.Password);

        if (result.IsSuccess)
        {
            await _unitOfWork.SaveAsync();
            return StatusCode(201);
        }

        return BadRequest(error);
    }
}
