using System.ComponentModel.DataAnnotations;
namespace TaskManagementAPI.DTOs;

public class ProfileDto
{
    [Required] 
    public required string Username { get; set; }
    
    [Required] 
    public required string Occupation { get; set; }
    


}