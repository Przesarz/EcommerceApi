using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Api.Dtos
{
    public class RegisterUserDto
    {
        [Required]
        [EmailAddress]
        [StringLength(100, MinimumLength = 5)]
        public required string Email { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public required string Password { get; set; }
    }
}
