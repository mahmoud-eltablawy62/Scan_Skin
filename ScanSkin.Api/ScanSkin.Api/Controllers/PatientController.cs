using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit.Text;
using ScanSkin.Api.Dtos;
using ScanSkin.Api.Errors;
using ScanSkin.Api.Helpers;
using ScanSkin.Core.Entites.Identity_User;
using ScanSkin.Core.Spacifications;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ScanSkin.Api.Controllers
{
    public class PatientController : BController
    {
        private readonly HttpClient _httpClient;
        private readonly UserManager<Users> _UserManager;
        private readonly IMapper _mapper; 
        public PatientController(UserManager<Users> UserManager , IMapper mapper)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://api-4-89sc.onrender.com/");
            _UserManager = UserManager;
            _mapper = mapper;   

        }
        [HttpPost("RespondingToillness")]
        public async Task<IActionResult> RespondingToillness([FromForm] illnessDto Dto)
        {

            var image = Dto.Illness_Pucture;
      
            var content = new MultipartFormDataContent();

            var photoContent = new StreamContent(image.OpenReadStream());
            content.Add(photoContent, "image", image.FileName);

            HttpResponseMessage response = await _httpClient.PostAsync("predict",content);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();

                int indexOfPTag = result.IndexOf("<p>") + 3;

                int indexOfTage2 = result.IndexOf("</p>");

                int Len = indexOfTage2 - indexOfPTag;

                string extractedSubstring = result.Substring(indexOfPTag, Len);

                return Ok(extractedSubstring);
            }
            else
            {
                return StatusCode((int)response.StatusCode);
            }
        }

        [HttpGet("GetAllDoctors")]
        public async Task<ActionResult<DoctorToReturnDto>> GetAllDoctors()
        {
            var Doctors = await _UserManager.GetUsersInRoleAsync("Doctor");
            return Ok(_mapper.Map<IReadOnlyList<DoctorToReturnDto>>(Doctors));
        }

        [Authorize]
        [HttpPut("UpdateDataForPatient")]
        public async Task<ActionResult<PatientUpdateData>> UpdateDataForPatient([FromForm] PatientDto Dto )
        {
            

            var user_email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _UserManager.FindByEmailAsync(user_email);

            if(user == null)
            {
                return BadRequest(new ApiValidation()
                { Errors = new string[] { "This User Is  Exist" } });
            }

            using var dataStream = new MemoryStream();
            await Dto?.Profile_Pucture.CopyToAsync(dataStream);
            user.Age = Dto.Age;
            user.Gender = (Gender?)Enum.Parse<Gender>(Dto.gen);
            user.BloodType = (BloodType?)Enum.Parse<BloodType>(Dto.Blood);
            user.Weight = Dto.Weight;
            user.Profile_Picture = dataStream.ToArray();
            user.Height = Dto.Height;

            await _UserManager.UpdateAsync(user);

            return Ok(new PatientUpdateData()
            {
                Age = Dto.Age,
                Height = Dto.Height,
                Weight = Dto.Weight,
                gen = Dto.gen,
                Blood = Dto.Blood,
                Profile_Pucture = Dto.Profile_Pucture
            }) ;

        }
    }
}

