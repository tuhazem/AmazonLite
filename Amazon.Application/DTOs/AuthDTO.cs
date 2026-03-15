using System.ComponentModel.DataAnnotations;

namespace Amazon.Application.DTOs
{
    public class RegisterDTO
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class LoginDTO
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class AuthResponseDTO
    {
        public string Token { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime Expiration { get; set; }
    }
}
