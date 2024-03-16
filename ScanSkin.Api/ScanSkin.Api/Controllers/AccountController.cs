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

namespace ScanSkin.Api.Controllers
{
    
    public class AccountController : BController
    {
        private readonly UserManager<Users> _UserManager;
        private readonly SignInManager<Users> _SignInManager;
        private readonly RoleManager<IdentityRole> _RoleManager;
        private readonly IAuthService _AuthService;



        public AccountController(UserManager<Users> usermanager,
                                 SignInManager<Users> Sign_InManager,
                                 IAuthService AuthService,
                                 RoleManager<IdentityRole> roleManager


            )
        {
            _UserManager = usermanager;
            _SignInManager = Sign_InManager;
            _AuthService = AuthService;
            _RoleManager = roleManager;

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
            if (Result.Succeeded is false) { return BadRequest(new ApiResponse(400)); }
            return Ok(new UserDto()
            {
                Name = user.User_Name,
                Email = user.Email,
                Token = await _AuthService.CreateTokenAsync(user, _UserManager)
            }
            );
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


            var claims = new List<Claim>
               {
             new Claim(ClaimTypes.Name, "username"),
                   };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = Login.RemeberMe,
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return Ok(new UserDto()
            {
                Name = user.User_Name,
                Email = user.Email,
                Token = await _AuthService.CreateTokenAsync(user, _UserManager)
            });
        }


        [Authorize]
        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await _SignInManager.SignOutAsync();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok("Sign-out successful");
        }


    }
}