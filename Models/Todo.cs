using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementAPI.Models;

public class Todo
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [Column(TypeName = "VARCHAR(255)")]
    public required string Title { get; set; }
    
    [Required]
    public required string Duration { get; set; }
    
    [Required]
    public required bool IsCompleted { get; set; }
    
      
    [Column(TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "timestamp")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}