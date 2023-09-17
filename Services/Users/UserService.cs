using TaskManagementAPI.DTOs;
using TaskManagementAPI.Models;
using TaskManagementAPI.Repositories.User;

namespace TaskManagementAPI.Services.Users;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<User> Signup(SignupDto signupDto)
    {
        return await _userRepository.Signup(signupDto);
    }

    public async Task<User> SignIn(SigninDto signinDto)
    {
        return await _userRepository.SignIn(signinDto);
    }

    public async Task<User> GetUserByEmail(string email)
    {
        return await _userRepository.GetUserByEmail(email);
    }

    public async Task<User> VerifyEmail(Guid token)
    {
        return await _userRepository.VerifyEmail(token);
    }

    public async Task<User> ChangePassword(ChangePasswordDto changePasswordDto, int id)
    {
        return await _userRepository.ChangePassword(changePasswordDto, id);
    }

    public async Task<User> GetUserById(int id)
    {
        return await _userRepository.GetUserById(id);
    }

    public async Task<User> Profile(ProfileDto profileDto, int id)
    {
        return await _userRepository.Profile(profileDto, id);
    }

    public async Task<List<User>> FindAll()
    {
        return await _userRepository.FindAll();
    }
}