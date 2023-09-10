using System.ComponentModel.DataAnnotations;

namespace TaskManagementAPI.DTOs;

public class SignupDto
{
    
    [Required]
    [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", 
        ErrorMessage = "Name field allows only characters.")]
    [StringLength(3, ErrorMessage = "Name length can't be less than 3.")]
    public required string Name { get; set; }
    
    
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    public required string Password { get; set; }
}