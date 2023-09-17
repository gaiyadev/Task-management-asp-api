using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace TaskManagementAPI.Models;

[Table("profiles")]
public class Profile
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("username",TypeName = "VARCHAR(255)")]
    public string Username { get; set; }

    [Required]
    [Column("occupation",TypeName = "VARCHAR(255)")]
    public string Occupation { get; set; }

    [Column("user_id")]
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