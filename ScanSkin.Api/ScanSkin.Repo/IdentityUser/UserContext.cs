using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ScanSkin.Core.Entites.Identity_User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanSkin.Repo.IdentityUser
{
    public class UserContext : IdentityDbContext<Users>
    {
        public UserContext(DbContextOptions<UserContext> options)
              : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);

        }
    }

}
