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
using System.Text;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

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
            string fileName;
            do
            {
                fileName = GenerateFileNameWithoutNumbers(Model.Profile_Picture.FileName);
            } while (System.IO.File.Exists(Path.Combine("wwwroot/Doctors/", fileName)));

            var filePath = Path.Combine("wwwroot/Doctors/", fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await Model.Profile_Picture.CopyToAsync(fileStream);
            }

            var user_Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _UserManager.FindByEmailAsync(user_Email);
            if (Model.Profile_Picture != null)
            {
                user.Profile_Picture = Encoding.UTF8.GetBytes($"Doctors/{fileName}");
            }
            user.Experience = Model.Experience;
            user.Price = Model.Price;
            user.AddressDescription = Model.AddressDescription;
            user.AddressLocation = Model.AddressLocation;
            user.Speciality = Model.Speciality;            
            user.StartWork = Model.StartHour; 
            user.EndWork = Model.EndHour;
            user.DurationTime = user.EndWork - user.StartWork;

            _Context.SaveChanges();

            return Ok(new DoctorDto()
            {
                Doctor_Name = user.UserName,
                Location_Description = user.AddressDescription,
            }
            );
        }

        [HttpGet()]
        private string GenerateFileNameWithoutNumbers(string originalFileName)
        {
            var guid = Guid.NewGuid().ToString("N");
            var extension = Path.GetExtension(originalFileName);
            var fileNameWithoutNumbers = Regex.Replace(guid.Substring(0, 6), @"\d", "a");
            return $"{fileNameWithoutNumbers}{extension}";
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
