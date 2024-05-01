using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ScanSkin.Api.Extentions;
using ScanSkin.Api.Setting;
using ScanSkin.Core.Entites.Identity_User;
using ScanSkin.Core.Service.Contract;
using ScanSkin.Repo.Data;
using ScanSkin.Repo.IdentityUser;
using ScanSkin.Services;


namespace ScanSkin.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {



            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "SkanSkinApi",
                    Description = "The idea of ​​the project is based on the fact that the user sends a picture of the skin and responds with the disease," +
                    " and we help him communicate with the appropriate doctor.",
                    
                    Contact = new OpenApiContact
                    {
                        Name = "Mahmoud_eltablawy/LinkedIn",
                        Url = new Uri("https://www.linkedin.com/in/mahmoud-eltablawy-282900265/")
                    },
                   
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\""
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
            });
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.Configure<MailSetting>(builder.Configuration.GetSection("MailSetting"));
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
           builder.Services.AddMemoryCache();

           builder.Services.AddScoped(typeof(IMailingService), typeof(MailingService));

           builder.Services.AddDbContext<ScanSkinContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddDbContext<UserContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Connection_Identity_User"));
            });


            builder.Services.AddIdentityService(builder.Configuration);

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromDays(30);

                    options.SlidingExpiration = true;
                });


            var app = builder.Build();

            using var scope = app.Services.CreateScope();

            var services = scope.ServiceProvider;

            var _dbContext = services.GetRequiredService<ScanSkinContext>();

            var _IdentityContext = services.GetRequiredService<UserContext>();

            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

            try
            {
                await _dbContext.Database.MigrateAsync();

                await _IdentityContext.Database.MigrateAsync();

                var _user_manager = services.GetRequiredService<UserManager<Users>>();
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();

                logger.LogError(ex, "an error has been occured during apply the migration ");
            }
          
            app.UseSwagger();

            app.UseSwaggerUI();
            
            app.UseHttpsRedirection();

            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
