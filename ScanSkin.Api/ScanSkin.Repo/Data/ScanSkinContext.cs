using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ScanSkin.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ScanSkin.Repo.Data
{
    public class ScanSkinContext : DbContext
    {
        public ScanSkinContext(DbContextOptions<ScanSkinContext> options)
                    : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

       

    }
}

