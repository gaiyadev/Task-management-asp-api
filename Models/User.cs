using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TaskManagementAPI.Models;

public class User
{
    public Guid Id { get; set; }

    [Required]
    [Column(TypeName = "VARCHAR(255)")]
    public required string  Name { get; set; }
    
    [Required(ErrorMessage = "Email is required")]
    [Column(TypeName = "VARCHAR(255)")]
    public required string  Email { get; set; }
    
    [Required]
    [Column(TypeName = "VARCHAR(255)")]
    public required string  Password { get; set; }

    [Column(TypeName = "VARCHAR(255)")]
    [DefaultValue(false)]
    public bool IsActive { get; set; }
    
    
    [Column(TypeName = "VARCHAR(255)")]
    public Guid? ResetToken { get; set; }

    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
}