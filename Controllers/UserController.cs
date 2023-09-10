
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.CustomException.Exceptions;
using TaskManagementAPI.CustomException.Helper;
using TaskManagementAPI.CustomException.Responses;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Repositories.User;
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
        try
        {
            var user = await _userRepository.Signup(signupDto);
            
// Is rarely for this to happen but in case
            if (user == null)
            {
                return ApplicationExceptionResponseHelper.HandleNotFound("User not found");
            }
            
            // Sending email
            string verificationLink = $"http://localhost:5178/verify?token={user.ResetToken}";

            _emailService.SendEmail(user.Email, "Email Verification",
                _emailService.EmailVerificationTemplate(verificationLink));

            // Response
            var apiResponse = new List<object>
            {
                new { id = user.Id, email = user.Email }
            };
            return SuccessResponse.HandleCreated("Successfully created", null, apiResponse);
        }
        catch (ConflictException ex)
        {
            return ApplicationExceptionResponseHelper.HandleConflictException(ex.Message);
        }
        catch (Exception ex)
        {
            return ApplicationExceptionResponseHelper.HandleInternalServerError(ex.Message);
        }
    }
    
    
    [HttpPost("signin")]
    public async Task<IActionResult> SignIn(SigninDto signinDto)
    {
        try
        {
            var user = await _userRepository.SignIn(signinDto);
            
            if (user == null)
            {
                return ApplicationExceptionResponseHelper.HandleNotFound("User not found");
            }
            
            // Create a JWT token.
            var token = _jwtService.CreateToken(user.Email, user.Name, user.Id);
          
            var apiResponse = new List<object>
            {
                new { id = user.Id, email = user.Email }
            };
            return SuccessResponse.HandleOk("Successfully login", apiResponse,token);
        }
        catch (ForbiddenException ex)
        {
            return ApplicationExceptionResponseHelper.HandleForbidden(ex.Message);
        }
        catch (Exception ex)
        {
            return ApplicationExceptionResponseHelper.HandleInternalServerError(ex.Message);
        }
    }

    [HttpPatch("verify")]
    public async Task<IActionResult> VerifyEmail([FromQuery] Guid token)
    {
        try
        {
            var user = await _userRepository.VerifyEmail(token);
            
            if (user == null)
            {
                return ApplicationExceptionResponseHelper.HandleNotFound("User not found");
            }
            
            var apiResponse = new List<object>
            {
                new { id = user.Id, email = user.Email }
            };
            return SuccessResponse.HandleCreated("Activated", null, apiResponse);
        }
        catch (NotFoundException ex)
        {
            return ApplicationExceptionResponseHelper.HandleNotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return ApplicationExceptionResponseHelper.HandleConflictException(ex.Message);
        }
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
            return ApplicationExceptionResponseHelper.HandleNotFound("Claim User not found");
        }
        
        try
        {
            var user = await _userRepository.ChangePassword(changePasswordDto, findEmail.Id);
            
            if (user == null)
            {
                return ApplicationExceptionResponseHelper.HandleNotFound("User not found");
            }
         
            var apiResponse = new List<object>
            {
                new { id = user.Id, email = user.Email }
            };
            
            return SuccessResponse.HandleCreated("Successfully changes", null, apiResponse);
        }
        catch (NotFoundException ex)
        {
            return ApplicationExceptionResponseHelper.HandleNotFound(ex.Message);
        }
        catch (ForbiddenException ex)
        {
            return ApplicationExceptionResponseHelper.HandleForbidden(ex.Message);
        }
        catch (BadHttpRequestException ex)
        {
            return ApplicationExceptionResponseHelper.HandleBadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return ApplicationExceptionResponseHelper.HandleInternalServerError(ex.Message);
        }
    }

    [HttpGet("{email:required}")]
    public async Task<IActionResult> GetUserByEmail(string email)
    {
        try
        {
            var user =  await _userRepository.GetUserByEmail(email);
         
            var apiResponse = new List<object>
            {
                new {Data = user }
            };
            return SuccessResponse.HandleOk("Fetched Successfully", apiResponse, null);

        }
        catch (NotFoundException ex)
        {
            return ApplicationExceptionResponseHelper.HandleNotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return ApplicationExceptionResponseHelper.HandleInternalServerError(ex.Message);
        }
    }
}