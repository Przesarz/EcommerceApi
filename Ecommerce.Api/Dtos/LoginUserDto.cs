using Ecommerce.Api.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Api.Dtos
{
    public class LoginUserDto
    {
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public required string Email { get; set; }
        [Required]
        [StringLength(100)]
        public required string Password { get; set; }

        void Test()
        {
            var hasher = new PasswordHasher<User>();
            var newUser = new User { Email = "1@mail@.com", PasswordHash = "" };
            var hashedPassword = hasher.HashPassword(newUser, "nowe haslo");
        }
    }
}
