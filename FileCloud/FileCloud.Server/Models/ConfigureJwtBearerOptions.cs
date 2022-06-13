using FileCloud.Server.Models.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FileCloud.Server.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
    {
        private readonly JwtAuthSettings _jwtAuthSettings;

        public ConfigureJwtBearerOptions(IOptions<JwtAuthSettings> options)
        {
            // ConfigureJwtBearerOptionsis constructed from DI, so we can inject anything here
            _jwtAuthSettings = options.Value;
        }

        public void Configure(string name, JwtBearerOptions options)
        {
            // check that we are currently configuring the options for the correct scheme
            if (name == JwtBearerDefaults.AuthenticationScheme)
            {
                byte[] secretBytes = Encoding.UTF8.GetBytes(_jwtAuthSettings.SecretKey);
                var key = new SymmetricSecurityKey(secretBytes);

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = _jwtAuthSettings.Issuer,
                    ValidAudience = _jwtAuthSettings.Audience,
                    IssuerSigningKey = key,
                    ClockSkew = TimeSpan.Zero
                };
            }
        }

        public void Configure(JwtBearerOptions options)
        {
            // default case: no scheme name was specified
            Configure(string.Empty, options);
        }
    }
}
