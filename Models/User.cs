using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TaskManagementAPI.Models;

    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(255)")]
        public required string  Name { get; set; }
    
        [Required(ErrorMessage = "Email is required")]
        [Column(TypeName = "VARCHAR(255)")]
        public required string  Email { get; set; }
    
        [Required]
        [Column(TypeName = "VARCHAR(255)")]
        public string  Password { get; set; }

        [Column(TypeName = "VARCHAR(255)")]
        [DefaultValue(false)]
        public bool IsActive { get; set; }
    
    
        [Column(TypeName = "VARCHAR(255)")]
        public Guid? ResetToken { get; set; }
        
        public Profile Profile { get; set; }

        [JsonIgnore]
        public ICollection<Todo> Todos { get; set; }
    
        [DataType(DataType.DateTime)]
        [Column(TypeName = "timestamp with time zone")]   
        public DateTime CreatedAt { get; set; }

        [DataType(DataType.DateTime)]
        [Column(TypeName = "timestamp with time zone")]
        public DateTime UpdatedAt { get; set; } 
    }

