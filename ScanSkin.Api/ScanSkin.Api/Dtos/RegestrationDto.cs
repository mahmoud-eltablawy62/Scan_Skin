using System.ComponentModel.DataAnnotations;

namespace ScanSkin.Api.Dtos
{
    public class RegestrationDto
    {
        [Required]
        public string DisplayName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        [RegularExpression("^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{8,32}$", ErrorMessage = "You must match regex At least one digit [0-9]\r\nAt least one lowercase character [a-z]\r\nAt least one uppercase character [A-Z] At least 8 characters in length, but no more than 32.")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Password must match confirm password")]
        public string ConfirmPassword { get; set; }
    }
}
