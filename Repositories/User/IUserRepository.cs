using TaskManagementAPI.DTOs;

namespace TaskManagementAPI.Repositories.User;

public interface IUserRepository
{
    Task<Models.User?> Signup(SignupDto signupDto);
    
    ValueTask<Models.User?> SignIn(SigninDto signinDto);
    
    Task<Models.User?> GetUserByEmail(string email);


}