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

    [DataType(DataType.DateTime)]
    [Column(TypeName = "timestamp with time zone")]
    public DateTime CreatedAt { get; set; }

    [DataType(DataType.DateTime)]
    [Column(TypeName = "timestamp with time zone")]
    public DateTime UpdatedAt { get; set; } 
}