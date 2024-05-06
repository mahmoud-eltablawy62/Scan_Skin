using ScanSkin.Core.Entites.Identity_User;

namespace ScanSkin.Api.Dtos
{
    public class PatientUpdateData
    {

        public int? Age { get; set; }
        public int? Height { get; set; }
        public int? Weight { get; set; }
        public string? gen { get; set; }
        public string? Blood { get; set; }
        public IFormFile? Profile_Pucture { get; set; }
    }
}
