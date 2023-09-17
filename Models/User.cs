using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TaskManagementAPI.Models;

[Table("users")]
    public class User
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("name",TypeName = "VARCHAR(255)")]
        public required string  Name { get; set; }
    
        [Required]
        [Column("email",TypeName = "VARCHAR(255)")]
        public required string  Email { get; set; }
    
        [Required]
        [Column("password",TypeName = "VARCHAR(255)")]
        public string  Password { get; set; }

        [Column("is_active",TypeName = "boolean")]
        [DefaultValue(false)]
        public bool IsActive { get; set; }
    
    
        [Column("reset_token",TypeName = "VARCHAR(255)")]
        public Guid? ResetToken { get; set; }
        
        public Profile Profile { get; set; }

        [JsonIgnore]
        public ICollection<Todo> Todos { get; set; }
    
        [DataType(DataType.DateTime)]
        [Column("created_at",TypeName = "timestamp with time zone")]   
        public DateTime CreatedAt { get; set; }

        [DataType(DataType.DateTime)]
        [Column("updated_at",TypeName = "timestamp with time zone")]
        public DateTime UpdatedAt { get; set; } 
    }

