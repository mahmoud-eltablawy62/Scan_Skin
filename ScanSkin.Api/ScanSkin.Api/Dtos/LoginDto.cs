using System.ComponentModel.DataAnnotations;

namespace ScanSkin.Api.Dtos
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RemeberMe { get; set; } = false;
    }
}
