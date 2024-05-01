namespace ScanSkin.Api.Dtos
{
    public class DataDoctorDto
    {
        
        public int Experience { get; set; }
        public int Price { get; set; }
        public string AddressLocation { get; set; }

        public string AddressDescription { get; set; }

        public string Speciality { get; set; }

        public DateTime StartDay { get; set; }

        public DateTime EndDay { get; set; }

        public IFormFile? Profile_Picture { get; set; }
      
    }
}
