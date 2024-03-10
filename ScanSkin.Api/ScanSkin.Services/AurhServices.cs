using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ScanSkin.Core.Entites.Identity_User;
using ScanSkin.Core.Service.Contract;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ScanSkin.Services
{
    public class AurhServices : IAuthService
    {
        private readonly IConfiguration _Config;
        public AurhServices(IConfiguration Config)
        {
            _Config = Config;
        }
        public async Task<string> CreateTokenAsync(Users userApp, UserManager<Users> userManager)
        {

            var _Claims = new List<Claim>()
            {

            new Claim (ClaimTypes.GivenName , userApp.UserName),

            new Claim (ClaimTypes.Email , userApp.Email)

            };

            var User_Role = await userManager.GetRolesAsync(userApp);

            foreach (var role in User_Role)
            {
                _Claims.Add(new Claim(ClaimTypes.Role, role));
            }



            var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Config["JWT:SecretKey"]));
            var token = new JwtSecurityToken(
                audience: _Config["JWT:ValidAudience"],
                issuer: _Config["Jwt:ValidIssuer"],
                expires: DateTime.UtcNow.AddDays(double.Parse(_Config["JWT:expireDay"])),
                claims: _Claims,
                signingCredentials: new SigningCredentials(AuthKey, SecurityAlgorithms.HmacSha256Signature)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
