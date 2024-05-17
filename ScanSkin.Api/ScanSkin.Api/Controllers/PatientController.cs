using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using MimeKit.Text;
using ScanSkin.Api.Dtos;
using ScanSkin.Api.Errors;
using ScanSkin.Api.Helpers;
using ScanSkin.Core.Entites.Identity_User;
using ScanSkin.Core.Service.Contract;
using ScanSkin.Core.Spacifications;
using ScanSkin.Services;
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
        private readonly IAppointementService _appointementService;
        public PatientController(UserManager<Users> UserManager, IMapper mapper, IAppointementService appointementService)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://api-4-89sc.onrender.com/");
            _UserManager = UserManager;
            _mapper = mapper;
            _appointementService = appointementService;

        }
        [HttpPost("RespondingToillness")]
        public async Task<IActionResult> RespondingToillness([FromForm] illnessDto Dto)
        {

            var image = Dto.Illness_Pucture;

            var content = new MultipartFormDataContent();

            var photoContent = new StreamContent(image.OpenReadStream());
            content.Add(photoContent, "image", image.FileName);

            HttpResponseMessage response = await _httpClient.PostAsync("predict", content);

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
        public async Task<ActionResult<PatientUpdateData>> UpdateDataForPatient([FromForm] PatientDto Dto)
        {


            var user_email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _UserManager.FindByEmailAsync(user_email);

            if (user == null)
            {
                return BadRequest(new ApiValidation()
                { Errors = new string[] { "This User Is not Exist" } });
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
            });
        }

        [HttpGet("{Id}")]
        public async Task <ActionResult<PhoneforDoctorDto>> GetPhoneforDoctors(string Id)
        { 
            var doctor = await _UserManager.FindByIdAsync(Id);

            if (doctor != null)
            {
                return Ok(new PhoneforDoctorDto
                {
                    PhoneNumber = doctor.PhoneNumber,
                });

            }
            return BadRequest(new ApiValidation()
            { Errors = new string[] { "This Doctor Is not Exist" } });    
        }

        [Authorize]
        [HttpPost("BookAppointment")]
        public async Task<ActionResult<AppointmentReturnDtoForDoctor>> BookAppointment(AppointmentDto dto)
        {

            DateTime datetimetoday = DateTime.Today;

            if(dto.Date < datetimetoday) { 
            return BadRequest("Day And Time Not Valid");
            }

            var DaysIsValid = await _appointementService.GetAppointmentsByData(dto.Date);

            int Appoints = DaysIsValid.Count();

            if(Appoints == 16)
            {
                return BadRequest("Full Day");
            }

            var user_email = User.FindFirstValue(ClaimTypes.Email);

            var user = await _UserManager.FindByEmailAsync(user_email);

            var Doctor = await _UserManager.FindByIdAsync(dto.Doctor_ID);

            var AppiontmentsWithSameDoctor = await _appointementService.GetAppoiontments(dto.Doctor_ID);

            foreach(var appoint in AppiontmentsWithSameDoctor)
            {
                if(appoint.StartTime == dto.StartTime && appoint.Date == dto.Date)
                {
                    return BadRequest("Appointment not valid");
                }
            }

            TimeSpan endtime = dto.StartTime.Add(TimeSpan.FromMinutes(30));

            var appointment = await _appointementService.Appointment( Doctor.Email , user.UserName , Doctor.PhoneNumber , dto.Date , dto.StartTime , endtime, Doctor.Id );

            return Ok(appointment);
        }
    }
}

