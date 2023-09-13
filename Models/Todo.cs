using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
    
    public int UserId { get; set; }

    public User User { get; set; }
    
    [DataType(DataType.DateTime)]
    [Column(TypeName = "timestamp with time zone")]
    public DateTime CreatedAt { get; set; }

    [DataType(DataType.DateTime)]
    [Column(TypeName = "timestamp with time zone")]
    public DateTime UpdatedAt { get; set; } 
}