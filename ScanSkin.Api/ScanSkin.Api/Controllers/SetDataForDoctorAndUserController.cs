using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ScanSkin.Api.Dtos;
using ScanSkin.Api.Errors;
using ScanSkin.Core.Entites.Identity_User;
using ScanSkin.Core.Service.Contract;
using ScanSkin.Repo.IdentityUser;
using System.Security.Claims;

namespace ScanSkin.Api.Controllers
{
   
    public class SetDataForDoctorAndUserController : BController
    {
        private readonly UserManager<Users> _UserManager;
        private readonly UserContext _Context;


        public SetDataForDoctorAndUserController(UserManager<Users> usermanager,
               UserContext Context

            )
        {
            _UserManager = usermanager;  
            _Context = Context; 
        }

        [Authorize]
        [HttpPost("SetRole")]
        public async Task<ActionResult<string>> SetRole([FromBody]string Role)
        {
            var user_Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _UserManager.FindByEmailAsync(user_Email);
            if (user == null)
            {
                return BadRequest("not found ");
            }
            await _UserManager.AddToRoleAsync(user, Role);
            return Ok("Okay");
            
        }

        [Authorize(Roles = "Doctor")]
        [HttpPost("SetData/Doctor")]
        public async Task<ActionResult<DoctorDto>> SetDataForDoctor(DataDoctorDto Model)
        {
            var user_Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _UserManager.FindByEmailAsync(user_Email);
            user.Experience = Model.Experience;
            user.Price = Model.Price;
            user.AddressDescription = Model.AddressDescription;
            user.AddressLocation = Model.AddressLocation;
            user.Speciality = Model.Speciality;
            user.StartDay = Model.StartDay;
            user.EndDay = Model.EndDay;

            _Context.SaveChanges();

            return Ok(new DoctorDto()
            {
                Doctor_Name = user.User_Name,
                Location_Description = user.AddressDescription,
            }
            );
        }

        [Authorize(Roles = "User")]
        [HttpPost("SetData")]
        public async Task<ActionResult<PatientDto>> SetDataForPatient([FromBody]int Age , int Height,
            int Weight ,Gender gen , BloodType blood)
        {
            var user_Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _UserManager.FindByEmailAsync(user_Email);
            user.Age = Age;
            user.Weight =Weight;
            user.Height = Height;
            user.Gender = gen;
            user.BloodType = blood;
           
            _Context.SaveChanges();

            return Ok(new PatientDto()
            {
                user_name = user.User_Name,
                Age = user.Age,
                Height = user.Height,
                Weight = user.Weight,

            }

            );
        }
        
    }
}
