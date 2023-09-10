using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagementAPI.Models;

public class Todo
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [Column(TypeName = "VARCHAR(255)")]
    public required string Title { get; set; }
    
    [Required]
    public required string Duration { get; set; }
    
    [Required]
    public required bool IsCompleted { get; set; }


}