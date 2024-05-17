using ScanSkin.Core.Entites.Identity_User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanSkin.Core.Entites
{
    public  class Appointment : BaseClass
    {

        public Appointment()
        {
            
        }
        public Appointment(string ?doctorEmail, string ?patient_name, string ?doctor_Phone, DateTime date, TimeSpan startTime, TimeSpan endTime, string doctor_Id)
        {
            DoctorName = doctorEmail;
            Patient_Name = patient_name;
            Doctor_Phone = doctor_Phone;
            Date = date;
            StartTime = startTime;
            EndTime = endTime;
            D_Id = doctor_Id;
        }

        public string ? DoctorName  { get; set; }
        public string ? Patient_Name  { get; set; }
        public string ? Doctor_Phone { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public TimeSpan StartTime { get; set; }
        [Required]
        public TimeSpan EndTime { get; set; }

        public string  D_Id { get; set; }

       public bool IsValed { get; set; } =true;
         
    }
}
