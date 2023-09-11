using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace TaskManagementAPI.Models;

public class Profile
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Username is required")]
    [Column(TypeName = "VARCHAR(255)")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Occupation is required")]
    [Column(TypeName = "VARCHAR(255)")]
    public string Occupation { get; set; }

    public int UserId { get; set; }
    [Key, ForeignKey("UserId")]

    [JsonIgnore] // Add this attribute to prevent circular reference
    public User User { get; set; }

    [Column(TypeName = "timestamp")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "timestamp")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}