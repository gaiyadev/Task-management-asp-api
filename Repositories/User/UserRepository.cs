using Microsoft.EntityFrameworkCore;
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
            // Create a new user entity
            var user = new Models.User()
            {
                Email = signupDto.Email,
                Password = hashedPassword,
                Name = signupDto.Name,
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
    
    // Signin
    public async ValueTask<Models.User?> SignIn(SigninDto signinDto)
    {
        try
        {
          var  user = await GetUserByEmail(signinDto.Email);
          if (user == null || !_passwordService.VerifyPassword(signinDto.Password, user.Password))
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
}