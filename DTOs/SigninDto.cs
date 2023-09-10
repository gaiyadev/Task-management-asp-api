using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.DTOs;

public class SigninDto
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    public required string Password { get; set; }
}