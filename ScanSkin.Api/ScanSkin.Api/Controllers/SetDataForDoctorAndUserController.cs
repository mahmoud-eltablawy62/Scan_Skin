using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        public async Task<ActionResult<string>> SetRole(RoleDto dto)
        {
            var user_Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _UserManager.FindByEmailAsync(user_Email);
            if (user == null)
            {
                return BadRequest("not found ");
            }
            await _UserManager.AddToRoleAsync(user, dto.Role);
            return Ok(new RoleReturnDto()
            {
                Email = user_Email, 
                Role = dto.Role 
            }
            );
            
        }

        [Authorize]
        [HttpPost("SetData/Doctor")]
        public async Task<ActionResult<DoctorDto>> SetDataForDoctor([FromForm] DataDoctorDto Model)
        {
            using var dataStream = new MemoryStream();
            await Model.Profile_Picture.CopyToAsync(dataStream);
            var user_Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _UserManager.FindByEmailAsync(user_Email);
            user.Experience = Model.Experience;
            user.Price = Model.Price;
            user.AddressDescription = Model.AddressDescription;
            user.AddressLocation = Model.AddressLocation;
            user.Speciality = Model.Speciality;
            user.StartDay = Model.StartDay;
            user.EndDay = Model.EndDay;
            user.Profile_Picture = dataStream.ToArray();

            _Context.SaveChanges();

            return Ok(new DoctorDto()
            {
                Doctor_Name = user.User_Name,
                Location_Description = user.AddressDescription,
            }
            );
        }

        [Authorize]
        [HttpPost("SetData")]
        public async Task<ActionResult<PatientDto>> SetDataForPatient([FromForm] PatientDto dto)
        {
            using var dataStream = new MemoryStream();
            await dto.Profile_Pucture.CopyToAsync(dataStream);
            var user_Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _UserManager.FindByEmailAsync(user_Email);
            user.Age = dto.Age;
            user.Weight =dto.Weight;
            user.Height = dto.Height;
            user.BloodType = (BloodType?)Enum.Parse<BloodType>(dto.Blood);
            user.Gender = (Gender?)Enum.Parse<Gender>(dto.gen);
            user.Profile_Picture = dataStream.ToArray() ;    
           
            _Context.SaveChanges();

            return Ok(new PatientDto()
            {
                Age = user.Age,
                Height = user.Height,
                Weight = user.Weight,
                gen = Enum.GetName(typeof(Gender), user.Gender),
                Blood =  Enum.GetName(typeof(BloodType), user.BloodType),
                Profile_Pucture = dto.Profile_Pucture
            }
            );
        }
        
    }
}
