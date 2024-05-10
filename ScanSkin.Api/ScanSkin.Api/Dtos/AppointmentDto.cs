using System.ComponentModel.DataAnnotations;

namespace ScanSkin.Api.Dtos
{
    public class AppointmentDto
    {
        public string Doctor_ID { get; set; }  
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public TimeSpan StartTime { get; set; }
    }
}
