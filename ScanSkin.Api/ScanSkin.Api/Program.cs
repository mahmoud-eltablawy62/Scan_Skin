using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ScanSkin.Api.Extentions;
using ScanSkin.Core.Entites.Identity_User;
using ScanSkin.Repo.Data;
using ScanSkin.Repo.IdentityUser;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ScanSkinContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddDbContext<UserContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Connection_Identity_User"));
});
builder.Services.AddIdentity<Users, IdentityRole>()
            .AddEntityFrameworkStores<UserContext>()
          .AddDefaultTokenProviders();

builder.Services.AddIdentityService(builder.Configuration);

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
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
