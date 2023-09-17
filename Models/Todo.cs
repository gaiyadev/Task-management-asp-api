using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementAPI.Models;

[Table("todos")]
public class Todo
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("title",TypeName = "VARCHAR(255)")]
    public required string Title { get; set; }
    
    [Required]
    [Column("duration", TypeName = "VARCHAR(255)")]
    public required string Duration { get; set; }
    
    [Required]
    [Column("Is_completed", TypeName = "boolean" )]
    public required bool IsCompleted { get; set; }
    
    [Column("user_id", TypeName = "int" )]
    public int UserId { get; set; }

    public User User { get; set; }
    
    [DataType(DataType.DateTime)]
    [Column("created_at",TypeName = "timestamp with time zone")]   
    public DateTime CreatedAt { get; set; }

    [DataType(DataType.DateTime)]
    [Column(TypeName = "timestamp with time zone")]
    public DateTime UpdatedAt { get; set; } 
}