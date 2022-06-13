using FileCloud.Data.Abstractions;
using FileCloud.Data.Entities;
using FileCloud.Data.Repositories;
using FileCloud.Data.Store;
using FileCloud.Server.Abstractions;
using FileCloud.Server.Models;
using FileCloud.Server.Models.Auth;
using FileCloud.Server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<JwtAuthSettings>(options => builder.Configuration.GetSection("JwtAuth").Bind(options));

builder.Services.AddDbContext<FileCloudDbContext>(config =>
{
    //config.UseInMemoryDatabase("Memory");
    config.UseSqlServer(builder.Configuration.GetConnectionString("FileCloudConnection"));
})
    .AddIdentity<User, Role>(config =>
    {
        config.Password.RequireDigit = true;
        config.Password.RequireLowercase = false;
        config.Password.RequireNonAlphanumeric = false;
        config.Password.RequireUppercase = false;
        config.Password.RequiredLength = 3;


        config.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
        config.Lockout.MaxFailedAccessAttempts = 5;
        config.Lockout.AllowedForNewUsers = true;
    })
    .AddEntityFrameworkStores<FileCloudDbContext>();

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IJwtTokenManager, JwtTokenManager>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddTransient<IRepository<PathInfo>, PathInfoRepository>();

builder.Services.AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();

builder.Services.AddAuthentication(cfg =>
    {
        cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, configureOptions: null);

builder.Services.AddAuthorization();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(cfg =>
{
    cfg.SwaggerDoc("v1", new OpenApiInfo { Title = "FileCloud Api", Version = "v1" });
    cfg.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    cfg.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              }
            },
            new List<string>()
          }
        });
});

builder.Services.Configure<FormOptions>(x =>
{
    x.ValueLengthLimit = int.MaxValue;
    x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
});


var app = builder.Build();

using (var serviceScope = app.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;

    using var userManager = services.GetRequiredService<UserManager<User>>();
    using var roleManager = services.GetRequiredService<RoleManager<Role>>();

    var user = await userManager.FindByNameAsync("admin");

    if (user == null)
    {
        user = new User
        {
            UserName = "admin",
            FirstName = "Default",
            Email = "admin@site.local",
            LastName = "Default Last"
        };

        var result = await userManager.CreateAsync(user, "111");

        if (result.Succeeded)
        {
            await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "Administrator"));

            var role = await roleManager.Roles.FirstOrDefaultAsync(role => role.Name == "Administrator");

            if(role == null)
            {
                var newRole = new Role
                {
                    Name = "Administrator",
                    NormalizedName = "Администратор",
                };

                var roleResult = await roleManager.CreateAsync(newRole);

                if(roleResult.Succeeded)
                {
                    if (!await userManager.IsInRoleAsync(user, "Administrator"))
                    {
                        var addToRoleResult = await userManager.AddToRoleAsync(user, "Administrator");

                        if (addToRoleResult.Succeeded)
                        {
                            var current = await userManager.FindByNameAsync(user.UserName);

                            var rootPath = @"C:\FileCloud";

                            if (Directory.Exists(rootPath))
                            {
                                var path = Path.Combine(rootPath, $"{current.UserName}_{current.Id}");

                                Directory.CreateDirectory(path);
                            }
                        }
                    }
                }
            }
        }
    }

    var userole = new Role
    {
        Name = "User",
        NormalizedName = "USER",
    };

    await roleManager.CreateAsync(userole);
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