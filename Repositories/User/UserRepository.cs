﻿using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Database;
using TaskManagementAPI.DTOs;
using TaskManagementAPI.Services;

namespace TaskManagementAPI.Repositories.User;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UserRepository> _logger;
    private readonly PasswordService _passwordService;

    public UserRepository(
        ApplicationDbContext context, 
        ILogger<UserRepository> logger,  
        PasswordService passwordService
        )
    {
        _context = context;
        _logger = logger;
        _passwordService = passwordService;
    }


    public async Task<Models.User?> Signup(SignupDto signupDto)
    {
        var findUser = await _context.Users.AnyAsync(user => user.Email == signupDto.Email);
        if (findUser)
        {
            return null;
        }
      
        try
        { 
            // Hash the password using bcrypt
            string hashedPassword = _passwordService.HashPassword(signupDto.Password);
            string guidHex = Guid.NewGuid().ToString("N"); // Generate a new GUID as a hexadecimal string
            Guid? verificationToken = new Guid(guidHex); // Convert the hexadecimal string to Guid
            
            // Create a new user entity
            var user = new Models.User()
            {
                Email = signupDto.Email,
                Password = hashedPassword,
                Name = signupDto.Name,
                ResetToken = verificationToken,
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
           throw;
        }
    }
    
// Get User By Email
    public async  Task<Models.User?> GetUserByEmail(string email)
    {
        try
        {
            return await _context.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }

    public async Task<Models.User?> VerifyEmail(Guid token)
    {
        try
        {
            var findUser = await _context.Users.Where(u => u.ResetToken == token).FirstOrDefaultAsync();
            if (findUser == null)
            {
                return null;
            }
            findUser.ResetToken = null;
            findUser.IsActive = true;
            _context.Update(findUser);
            await _context.SaveChangesAsync();
            return findUser;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            _logger.LogError(ex.Message);
            throw;
        }
    }

    // Signin
    public async Task<Models.User?> SignIn(SigninDto signinDto)
    {
        try
        {
          var  user = await GetUserByEmail(signinDto.Email);
          if (user == null || !_passwordService.VerifyPassword(signinDto.Password, user.Password))
          {
              return null;
          }
          
          if (user.IsActive == false)
          {
              return null;
          }
          
          return user;
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }

    public async Task<Models.User?> ChangePassword(ChangePasswordDto changePasswordDto, Guid id)
    {
        try
        {
            var findUser = await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
            if (findUser == null)
            {
                return null;
            }

            if (!_passwordService.VerifyPassword(changePasswordDto.Password,findUser.Password))
            {
                return null;
            }
            string hashedPassword = _passwordService.HashPassword(changePasswordDto.NewPassword);
            findUser.Password = hashedPassword;
            _context.Update(findUser);
            await _context.SaveChangesAsync();
            return findUser;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }
}