namespace TaskManagementAPI.DTOs.Responses;

public class UserWithProfileResponse
{
    public class UserWithProfileDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public ProfileDto Profile { get; set; }
    }

    public class ProfileDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Occupation { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}