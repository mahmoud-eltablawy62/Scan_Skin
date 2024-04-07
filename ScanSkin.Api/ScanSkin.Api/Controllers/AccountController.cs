using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ScanSkin.Api.Dtos;
using ScanSkin.Api.Errors;
using ScanSkin.Core.Entites.Identity_User;
using ScanSkin.Core.Service.Contract;
using ScanSkin.Repo.IdentityUser;
using System.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Cors;
using MailKit;
using ScanSkin.Services;
using System.Reflection.Emit;

namespace ScanSkin.Api.Controllers
{
    
    public class AccountController : BController
    {
        private readonly UserManager<Users> _UserManager;
        private readonly SignInManager<Users> _SignInManager;
        private readonly IAuthService _AuthService;
        private readonly IMailingService _mailingService;

        /// tcld lllo blll jpee


        public AccountController(UserManager<Users> usermanager,
                                 SignInManager<Users> Sign_InManager,
                                 IAuthService AuthService,                                
                                 IMailingService mailingService

            )
        {
            _UserManager = usermanager;
            _SignInManager = Sign_InManager;
            _AuthService = AuthService;
            _mailingService = mailingService;

        }

        [HttpGet("CkeckEmail")]
        public async Task<ActionResult<bool>> CheckedEmail(string Email)
        {
            return await _UserManager.FindByEmailAsync(Email) is not null;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Registration(RegestrationDto Model)
        {
            if (CheckedEmail(Model.Email).Result.Value)
            {
                return BadRequest(new ApiValidation()
                { Errors = new string[] { "this email is exist" } });
            }
            var user = new Users()
            {
                User_Name = Model.DisplayName,
                Email = Model.Email,
                UserName = Model.Email.Split('@')[0],
                PhoneNumber = Model.PhoneNumber
            };
            var Result = await _UserManager.CreateAsync(user, Model.Password);

            Random confirmationCode = new Random();

            string code = confirmationCode.Next(100000, 999999).ToString();

           await _mailingService.SendEmailAsync(Model.Email, code);

            await _UserManager.SetAuthenticationTokenAsync(user, "Confirmation", "Code", code);

            if (Result.Succeeded is false) { return BadRequest(new ApiResponse(400)); }
            return Ok(new UserDto()
            {
                Name = user.User_Name,
                Email = user.Email,
                Token = await _AuthService.CreateTokenAsync(user, _UserManager)
            }
            );
        }
        [Authorize]
        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmCode([FromBody]string code)
        {
            // Retrieve the user based on the provided email
            var user_Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _UserManager.FindByEmailAsync(user_Email);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            // Check if the provided confirmation code matches the one stored in the user record
            var isCodeValid =  await _UserManager.GetAuthenticationTokenAsync(user, "Confirmation", "Code");

            if (code != isCodeValid)
            {
                return Ok("Email confirmation failed");
                        
            }
            var result = await _UserManager.ConfirmEmailAsync(user, await _UserManager.GenerateEmailConfirmationTokenAsync(user));

            if (!result.Succeeded)
            {
                return BadRequest("Email confirmation failed");
            }
            return Ok("mail Confirmed");

        }


        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto Login)
        {
            var user = await _UserManager.FindByEmailAsync(Login.Email);
            if (user == null)
            {
                return Unauthorized(new ApiResponse(401));
            }
            var pass = await _SignInManager.CheckPasswordSignInAsync(user, Login.Password, false);
            if (pass.Succeeded is false) { return Unauthorized(new ApiResponse(401)); }

            if (!await _UserManager.IsEmailConfirmedAsync(user))
            {
                return BadRequest("Email not confirmed");
            }


            return Ok(new UserDto()
            {
                Name = user.User_Name,
                Email = user.Email,
                Token = await _AuthService.CreateTokenAsync(user, _UserManager)
            });
        }

       

    }
}