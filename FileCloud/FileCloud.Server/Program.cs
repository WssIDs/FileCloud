using FileCloud.Data.Entities;
using FileCloud.Data.Store;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<FileCloudDbContext>(config =>
{
    config.UseInMemoryDatabase("Memory");
})
    .AddIdentity<User, Role>(config =>
    {
        config.Password.RequireDigit = true;
        config.Password.RequireLowercase = false;
        config.Password.RequireNonAlphanumeric = false;
        config.Password.RequireUppercase = false;
        config.Password.RequiredLength = 1;
    })
    .AddEntityFrameworkStores<FileCloudDbContext>();

builder.Services.AddControllersWithViews();

builder.Services.AddAuthorization();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var serviceScope = app.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;

    using var userManager = services.GetRequiredService<UserManager<User>>();

    var user = new User
    {
        UserName = "admin",
        FirstName = "Default",
        LastName = "Default Last"
    };

    var task = userManager.CreateAsync(user, "1");
    var result = task.GetAwaiter().GetResult();

    if(!result.Succeeded)
    {
        await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "Administrator"));
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();