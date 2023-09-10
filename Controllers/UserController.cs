
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.DTOs;
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
    private readonly EmailService _emailService;

    public UserController(IUserRepository userRepository, JwtService jwtService, EmailService emailService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _emailService = emailService;

    }

    [HttpPost("signup")]
    public async Task<IActionResult> Signup(SignupDto signupDto)
    {
        var user = await _userRepository.Signup(signupDto);

        if (user != null)
        {
            // Sending email
            string verificationLink = $"http://localhost:5178/verify?token={user.ResetToken}";
            Console.WriteLine(verificationLink);
            _emailService.SendEmail(user.Email,"Email Verification", _emailService.EmailVerificationTemplate(verificationLink));
            
            // Response
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

    [HttpPatch("verify")]
    public async Task<IActionResult> VerifyEmail([FromQuery] Guid token)
    {
        var user = await _userRepository.VerifyEmail(token);
        if (user == null)
        {
            var errorApiResponse = new ErrorApiResponse
            {
                Error = "Invalid token.",
                StatusCode = 403,
                Title = "Forbidden",
            };
            return BadRequest(errorApiResponse);
        }
        var apiResponse = new ApiResponse<object>
        {
            Message = "Successfully activated",
            StatusCode = 200,
            Data = new
            {
                id = user.Id,
                email = user.Email,
            },
        };
        return CreatedAtAction("VerifyEmail",apiResponse);
    }

    private (string?, bool?) GetUserClaims()
    {
        var userEmailClaim = User.FindFirst(ClaimTypes.Email);

        if (userEmailClaim == null)
        {
            return (null, null); // Return null values for both string and bool
        }
        return (userEmailClaim.Value, true); // Return the email claim value and true for bool
    }

    [HttpPut("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
    {
        var (emailClaim, isClaimFound) = GetUserClaims();
        if (emailClaim == null || isClaimFound == false)
        {
            return Forbid();
        }

        var findEmail = await _userRepository.GetUserByEmail(emailClaim);

        if (findEmail == null)
        {
            return Forbid();
        }
        var user = await _userRepository.ChangePassword(changePasswordDto, findEmail.Id);
        if (user == null)
        {
            var errorApiResponse = new ErrorApiResponse
            {
                Error = "User not found.",
                StatusCode = 404,
                Title = "NotFound",
            };
            return NotFound(errorApiResponse);
        }
        var apiResponse = new ApiResponse<object>
        {
            Message = "Successfully changes",
            StatusCode = 200,
            Data = new
            {
                id = user.Id,
                email = user.Email,
            },
        };
        return Ok(apiResponse);
    }
}