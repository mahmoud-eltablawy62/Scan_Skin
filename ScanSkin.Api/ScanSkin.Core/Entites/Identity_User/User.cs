using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ScanSkin.Core.Entites.Identity_User
{
    public class Users : IdentityUser
    {
        public string User_Name { get; set; }
    }
}
