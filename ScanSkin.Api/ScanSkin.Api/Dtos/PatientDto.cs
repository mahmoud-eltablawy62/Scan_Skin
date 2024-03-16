using ScanSkin.Core.Entites.Identity_User;

namespace ScanSkin.Api.Dtos
{
    public class PatientDto
    {
        public string user_name { get; set; }   
        public int? Age { get; set; }
        public int? Weight { get; set; }
        public int? Height { get; set; }
       
    }
}
