﻿using TaskManagementAPI.DTOs;

namespace TaskManagementAPI.Repositories.User;

public interface IUserRepository
{
    Task<Models.User?> Signup(SignupDto signupDto);
    
    Task<Models.User?> SignIn(SigninDto signinDto);
    
    Task<Models.User?> GetUserByEmail(string email);
    Task<Models.User?> VerifyEmail(Guid token);

    Task<Models.User?> ChangePassword(ChangePasswordDto changePasswordDto, Guid id);

}