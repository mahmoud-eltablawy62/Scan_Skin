using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace ScanSkin.Api.Dtos
{
    public class ResetPasswordModel
    {
        public string Email { get; set; }
        public string Token { get; set; }   
        public string ResetPassword { get; set; }
    }
}
