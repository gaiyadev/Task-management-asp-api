
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Models;
using TaskManagementAPI.Repositories.User;
using TaskManagementAPI.Responses;
using TaskManagementAPI.Services;

namespace TaskManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly JwtService _jwtService;

    public UserController(IUserRepository userRepository, JwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;

    }

    [HttpPost("signup")]
    public async Task<IActionResult> Signup(SignupDto signupDto)
    {
        var user = await _userRepository.Signup(signupDto);

        if (user != null)
        {
            var apiResponse = new ApiResponse<object>
            {
                Message = "Successfully created",
                StatusCode = 200,
                Data = new
                {
                    id = user.Id,
                    email = user.Email,
                },
            };
            return CreatedAtAction("Signup",apiResponse);
        }
        
        var errorApiResponse = new ErrorApiResponse
        {
            Error = "Email address already taken",
            StatusCode = 403,
            Title = "Forbidden",
        };
        return Conflict(errorApiResponse);
    }
    
    
    [HttpPost("signin")]
    public async Task<IActionResult> SignIn(SigninDto signinDto)
    {
    var user = await _userRepository.SignIn(signinDto);
    
    if (user == null)
    {
        var errorApiResponse = new ErrorApiResponse
        {
            Error = "Email of Password is invalid.",
            StatusCode = 403,
            Title = "Forbidden",
        };
        return Conflict(errorApiResponse);
    }
    // if Success
    // Create a JWT token.
    var token = _jwtService.CreateToken(user.Email, user.Name, user.Id);
    var apiResponse = new ApiResponse<object>
    {
        Message = "Successfully login",
        StatusCode = 200,
        Data = new
        {
            id = user.Id,
            email = user.Email,
        },
        accessToken = token,
    };
        return Ok(apiResponse);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> UserProfile()
    {
        var userEmailClaim = User.FindFirst(ClaimTypes.Email);

        if (userEmailClaim == null)
        {
            return BadRequest();
        }

        var user = await _userRepository.GetUserByEmail(userEmailClaim.Value);
        if (user == null)
        {
            return Forbid();
        }

        return Ok(user);
    }


}