using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.DTOs;

public class ChangePasswordDto
{
    [Required]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    public required string Password { get; set; }
    
    [Required]
    [MinLength(6, ErrorMessage = "New password must be at least 6 characters")]
    public required string NewPassword { get; set; }
    
    [Required]
    [MinLength(6, ErrorMessage = "New password must be at least 6 characters")]
    public required string ConfirmPassword { get; set; }
}