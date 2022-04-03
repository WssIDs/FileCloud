using FileCloud.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FileCloud.Server.Auth
{
    public class AuthManager : IAuthManager
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public AuthManager(
            UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<string> AuthAsync(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if(user == null) throw new NullReferenceException(nameof(user));

            var result = await _userManager.CheckPasswordAsync(user, password);

            if (result)
            {
                var roles = await _userManager.GetRolesAsync(user);

                var claims = new List<Claim>()
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                };

                foreach (var userRole in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole));
                    var role = await _roleManager.FindByNameAsync(userRole);
                    if (role != null)
                    {
                        var roleClaims = await _roleManager.GetClaimsAsync(role);
                        foreach (Claim roleClaim in roleClaims)
                        {
                            claims.Add(roleClaim);
                        }
                    }
                }

                byte[] secretBytes = Encoding.UTF8.GetBytes(TokenConstants.SecretKey);
                var key = new SymmetricSecurityKey(secretBytes);

                var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    TokenConstants.Issuer,
                    TokenConstants.Audience,
                    claims,
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.AddMinutes(15),
                    signingCredentials: signingCredentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            else
            {
                throw new Exception("Имя пользователя или пароль неверный");
            }
        }
    }
}
