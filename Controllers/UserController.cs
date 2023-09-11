
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
        catch (InternalServerException ex)
        {
            return ApplicationExceptionResponseHelper.HandleInternalServerError(ex.Message);
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
            return SuccessResponse.HandleOk("Successfully login", apiResponse, token);
        }
        catch (ForbiddenException ex)
        {
            return ApplicationExceptionResponseHelper.HandleForbidden(ex.Message);
        }
        catch (InternalServerException ex)
        {
            return ApplicationExceptionResponseHelper.HandleInternalServerError(ex.Message);
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
        catch (InternalServerException ex)
        {
            return ApplicationExceptionResponseHelper.HandleInternalServerError(ex.Message);
        }
        catch (Exception ex)
        {
            return ApplicationExceptionResponseHelper.HandleConflictException(ex.Message);
        }
    }

    private (Guid?, bool?) GetUserClaims()
    {
        var userEmailClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userEmailClaim == null)
        {
            return (null, null); // Return null values for both Guid and bool
        }

        // Assuming you have a method to parse the email claim value into a Guid
        if (Guid.TryParse(userEmailClaim.Value, out Guid parsedGuid))
        {
            return (parsedGuid, true); // Return the parsed Guid and true for bool
        }
        else
        {
            return (null, false); // Return null for Guid and false for bool if parsing fails
        }
    }


    [HttpPut("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
    {
        // Get the user's ID claim
        var userIdClaim = User.FindFirst(ClaimTypes.Email);

        if (userIdClaim == null)
        {
            return Forbid();
        }
        
        string encodedUserId = userIdClaim.Value;
        
        var findEmail = await _userRepository.GetUserByEmail(encodedUserId);
        
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
        catch (BadRequestException ex)
        {
            return ApplicationExceptionResponseHelper.HandleBadRequest(ex.Message);
        }
        catch (InternalServerException ex)
        {
            return ApplicationExceptionResponseHelper.HandleInternalServerError(ex.Message);
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
        catch (InternalServerException ex)
        {
            return ApplicationExceptionResponseHelper.HandleInternalServerError(ex.Message);
        }
        catch (Exception ex)
        {
            return ApplicationExceptionResponseHelper.HandleInternalServerError(ex.Message);
        }
    }
    
    [HttpGet("account/{id:int}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        try
        {
            var user = await _userRepository.GetUserById(id);

            if (user == null)
            {
                return ApplicationExceptionResponseHelper.HandleNotFound("user not found");
            }

            var apiResponse = new List<object>
            {
                new { Data = user }
            };
            return SuccessResponse.HandleOk("Successfully changes", apiResponse, null);
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


    [HttpPut("account")]
    [Authorize]
    public async Task<IActionResult> Profile(ProfileDto profileDto)
    {
        try
        {
            // Get the user's ID claim
            var userIdClaim = User.FindFirst(ClaimTypes.Email);

            if (userIdClaim == null)
            {
                return Forbid();
            }
        
            string encodedUserId = userIdClaim.Value;
        
            var findEmail = await _userRepository.GetUserByEmail(encodedUserId);
            
            var user = await _userRepository.Profile(profileDto, findEmail.Id);
            if (user == null)
            {
                return NotFound();
            }
            Console.WriteLine(user);
            var apiResponse = new List<object>
            {
                // new { Data = user }
            };
            return SuccessResponse.HandleCreated("Successfully updated" , null,apiResponse);
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