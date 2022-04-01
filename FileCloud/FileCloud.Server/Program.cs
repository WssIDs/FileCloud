using FileCloud.Data.Entities;
using FileCloud.Data.Store;
using FileCloud.Server.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

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

builder.Services.AddSingleton<IAuthManager, AuthManager>();

builder.Services.AddAuthentication(cfg =>
    {
        cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(config =>
    {
        byte[] secretBytes = Encoding.UTF8.GetBytes(TokenConstants.SecretKey);
        var key = new SymmetricSecurityKey(secretBytes);

        config.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = TokenConstants.Issuer,
            ValidAudience = TokenConstants.Audience,
            IssuerSigningKey = key
        };
    });

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