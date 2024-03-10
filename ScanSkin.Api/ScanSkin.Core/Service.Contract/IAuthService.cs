using Microsoft.AspNetCore.Identity;
using ScanSkin.Core.Entites.Identity_User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanSkin.Core.Service.Contract
{
    public interface IAuthService
    {
        Task<string> CreateTokenAsync(Users userApp, UserManager<Users> userManager);

    }
}
