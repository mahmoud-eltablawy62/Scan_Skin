using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ScanSkin.Core.Entites.Identity_User
{
    public class Users : IdentityUser
    {

        

        //<<<<<<<<<<<<< Doctor >>>>>>>>>>>>>>>> //

        public int ? Experience  { get; set; }
        public int ? Price {  get; set; }   
        public string? AddressLocation { get; set; }
        public string? AddressDescription { get; set; }
        public string? Speciality { get; set;}
        public TimeSpan ? StartWork { get; set; }
        public TimeSpan? EndWork { get; set; }
        public TimeSpan ? DurationTime { get; set; }    
        public DateTime? StartDate { get; set; }  
        public DateTime? EndDate { get; set; }  

        //<<<<<<<<<<<<<<<<< Patient >>>>>>>>>>>>> 


        public int ? Age { get; set; }        
        public int ? Weight { get; set; }   
        public int ? Height { get; set; }   
        public Gender? Gender { get; set; }
        public BloodType ? BloodType { get; set; }

        ///////////////////// Both picture to doctor and paitent //////////////// 
        public byte[] ? Profile_Picture { get; set; } = null;

       
    }
}
