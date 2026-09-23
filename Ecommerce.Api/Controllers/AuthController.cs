using Ecommerce.Api.Data;
using Ecommerce.Api.Dtos;
using Ecommerce.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterUserDto userDto)
        {
            var doesUserExist = await _context.Users.AnyAsync(u => u.Email == userDto.Email);
            if (doesUserExist)
            {
                return BadRequest();
            }

            var newUser = new User
            {
                Email = userDto.Email,
                PasswordHash = ""
            };

            var passwordHasher = new PasswordHasher<User>();
            var hashedPassword = passwordHasher.HashPassword(newUser, userDto.Password);

            newUser.PasswordHash = hashedPassword;

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(LoginUserDto userDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email);
            if (user == null)
            {
                return BadRequest();
            }

            var passwordHasher = new PasswordHasher<User>();

            var passwordVerificationResult = passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                userDto.Password);
            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
