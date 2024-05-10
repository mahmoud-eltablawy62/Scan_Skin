using System.ComponentModel.DataAnnotations;

namespace ScanSkin.Api.Dtos
{
    public class AppointmentReturnDtoForDoctor
    {
        public string? DoctorName { get; set; }
        public DateTime Date { get; set; }
        [Required]
        public TimeSpan StartTime { get; set; }
        public string? Doctor_Phone { get; set; }


    }
}
