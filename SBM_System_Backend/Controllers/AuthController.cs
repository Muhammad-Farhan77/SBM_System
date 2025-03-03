using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SBM_System_Backend.AppDbContext;
using SBM_System_Backend.DTOs;
using SBM_System_Backend.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using SBM_System_Backend.Migrations;

namespace SBM_System_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AuthController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;
        public AuthController(DataContext dataContext, IConfiguration configuration)
        {
          this._dataContext = dataContext;
            this._configuration = configuration;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            if (user.Id < 0)
            {
                return BadRequest("Invalid user id");
            }

            if(user == null)
            {
                return BadRequest("Invalid user data.");
            }

            if (_dataContext.Users.Any(u => u.Email == user.Email))
            {
                return Conflict(new { Message = "Email already exists!" });
            }

            string hashedPassword = HashPasswordHMACSHA256(user.Password, "MySecretKey123");


            var newuUser = new User
            {
                Name = user.Name,
                Email = user.Email,
                Password = hashedPassword,
                Role = user.IsAdmin ? eRole.Admin : eRole.Customer,
                BookingDate = DateTime.UtcNow,
                IsAdmin = user.IsAdmin
            };

            _dataContext.Users.Add(newuUser);
            _dataContext.SaveChanges();
            return Ok(new { Message = "User registered successfully!" });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            var existingUser = _dataContext.Users.FirstOrDefault(u => u.Email == loginDto.Email);

            if (existingUser == null || !VerifyPasswordHMACSHA256(loginDto.Password, existingUser.Password, "MySecretKey123"))
            {
                return Unauthorized(new { Message = "Invalid email or password." });
            }

            var token = GenerateJwtToken(existingUser);

            return Ok(new
            {
                Token = token,
                Role = existingUser.Role.ToString(),
                UserId = existingUser.Id,
                Email = existingUser.Email,
                Message = "Login successful"
            });
        }


        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.Role, user.Role.ToString()),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
    };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        private string HashPasswordHMACSHA256(string password, string secretKey)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey)))
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = hmac.ComputeHash(passwordBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower(); // Convert to hex string
            }
        }

        private bool VerifyPasswordHMACSHA256(string inputPassword, string storedHashedPassword, string secretKey)
        {
            string hashedInputPassword = HashPasswordHMACSHA256(inputPassword, secretKey);
            return hashedInputPassword == storedHashedPassword;
        }


    }
}
    

