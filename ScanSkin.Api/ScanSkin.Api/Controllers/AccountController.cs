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
using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Memory;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using System.Web;

namespace ScanSkin.Api.Controllers
{
    
    public class AccountController : BController
    {
        private readonly UserManager<Users> _UserManager;
        private readonly SignInManager<Users> _SignInManager;
        private readonly IAuthService _AuthService;
        private readonly IMailingService _mailingService;
        private readonly IMemoryCache _memoryCache;

        public AccountController(UserManager<Users> usermanager,
                                 SignInManager<Users> Sign_InManager,
                                 IAuthService AuthService,                                
                                 IMailingService mailingService,
                                 IMemoryCache memoryCache
            )
        {
            _UserManager = usermanager;
            _SignInManager = Sign_InManager;
            _AuthService = AuthService;
            _mailingService = mailingService;
            _memoryCache = memoryCache;
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
                Name = user.UserName,
                Email = user.Email,
                Token = await _AuthService.CreateTokenAsync(user, _UserManager)
            }
            );
        }
        [Authorize]
        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmCode(ConfirmDto Dto)
        {

            if (_memoryCache.TryGetValue("VerificationCode", out string storedCode))
            {
                
                if (Dto.ConfirmationCode == storedCode)
                {
                    var user_Email = User.FindFirstValue(ClaimTypes.Email);
                    var user = await _UserManager.FindByEmailAsync(user_Email);
                    var isCodeValid = await _UserManager.GetAuthenticationTokenAsync(user, "Confirmation", "Code");

                    if (Dto.ConfirmationCode != isCodeValid)
                    {
                        return Ok("Email confirmation failed");
                    }
                    var result = await _UserManager.ConfirmEmailAsync(user, await _UserManager.GenerateEmailConfirmationTokenAsync(user));

                    if (result.Succeeded)
                    {          
                        _memoryCache.Remove("VerificationCode");

                        return Ok("Verification code confirmed successfully.");
                    }
                    else
                    {                  
                        return BadRequest("Email confirmation failed.");
                    }
                }
            }
            return BadRequest("Click On ResendCode");
        }

        [Authorize]
        [HttpPost("ResendCode")]
        public async Task<IActionResult> ResendCode()
        {
            var user_Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _UserManager.FindByEmailAsync(user_Email);
            if (user != null)
            {
                await _UserManager.RemoveAuthenticationTokenAsync(user, "Confirmation", "Code");
                Random confirmationCode = new Random();

                string code = confirmationCode.Next(100000, 999999).ToString();

                await _mailingService.SendEmailAsync(user_Email, code);

                await _UserManager.SetAuthenticationTokenAsync(user, "Confirmation", "Code", code);

                return Ok($" Anthor Code ==> {code}");
            }
            else
            {
                return BadRequest($"User ====> {user_Email} not found in database");    
            }

        }

        
        [HttpPost("forgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgetPasswordDto Dto)
        {
            var user = await _UserManager.FindByEmailAsync(Dto.Email);
            if (user != null)
            {
                await _UserManager.RemoveAuthenticationTokenAsync(user, "Confirmation", "Code");

                Random confirmationCode = new Random();

                string code = confirmationCode.Next(100000, 999999).ToString();

                await _mailingService.SendEmailAsync(Dto.Email, code);

                await _UserManager.SetAuthenticationTokenAsync(user, "Confirmation", "Code", code);

                return Ok($" Code ==> {code}");
            }
            else
            {
                return BadRequest($"{Dto.Email} not found in database ");
            }
        }



        [Authorize]
        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPassword Dto )
        {
            var user_Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _UserManager.FindByEmailAsync(user_Email);

            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var token = await _UserManager.GeneratePasswordResetTokenAsync(user);

            // Reset the user's password
            var result = await _UserManager.ResetPasswordAsync(user,token,Dto.NewPassword);

            if (result.Succeeded)
            {
                return Ok("Password reset successfully.");
            }

            return BadRequest("Password reset failed.");
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
                Name = user.UserName,
                Email = user.Email,
                Token = await _AuthService.CreateTokenAsync(user, _UserManager)
            });
        }
    }
}