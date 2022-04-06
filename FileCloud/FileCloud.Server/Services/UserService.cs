using FileCloud.Data.Entities;
using FileCloud.Server.Abstractions;
using FileCloud.Server.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FileCloud.Server.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IJwtTokenManager _jwtTokenManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public UserService(
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IJwtTokenManager jwtTokenManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenManager = jwtTokenManager;
        }

        public async Task<AuthenticateResponseModel> AuthAsync(AuthenticateRequestModel authenticateRequest)
        {
            var user = await _userManager.FindByNameAsync(authenticateRequest.UserName);

            if(user == null) throw new NullReferenceException(nameof(user));

            var result = await _userManager.CheckPasswordAsync(user, authenticateRequest.Password);

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

                var jwtToken = _jwtTokenManager.GenerateJwtToken(claims, DateTime.UtcNow.AddMinutes(1));

                return new AuthenticateResponseModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Username = user.UserName,
                    JwtToken = jwtToken
                };
            }
            else
            {
                throw new Exception("Имя пользователя или пароль неверный");
            }
        }

        public Task<IEnumerable<object>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<object> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public AuthenticateTokenResponseModel GetToken() => new() { Token = _jwtTokenManager.UpdateToken() };
    }
}
