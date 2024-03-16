using ScanSkin.Core.Entites.Identity_User;

namespace ScanSkin.Api.Dtos
{
    public class DataPatientDto
    {
        public int Age { get; set; }
        public int Weight { get; set; }
        public int Height { get; set; }
        public string Gender { get; set; }
        public string BloodType { get; set; }
    }
}
