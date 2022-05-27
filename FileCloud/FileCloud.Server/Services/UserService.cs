using FileCloud.Data.Entities;
using FileCloud.Server.Abstractions;
using FileCloud.Server.Models.Auth;
using FileCloud.Shared.Models;
using FileCloud.Shared.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FileCloud.Server.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class UserService : IUserService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtTokenManager _jwtTokenManager;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public UserService(
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IJwtTokenManager jwtTokenManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenManager = jwtTokenManager;
            _signInManager = signInManager;
        }

        public async Task<UserModel> AuthAsync(AuthenticateRequestModel authenticateRequest)
        {
            var user = await _userManager.FindByNameAsync(authenticateRequest.UserName);

            if (user == null) throw new NullReferenceException($"Пользователь '{nameof(user)}' не зарегистирован в системе");

            var signResult = await _signInManager.CheckPasswordSignInAsync(user, authenticateRequest.Password, true);

            if (!signResult.Succeeded)
            {
                if (signResult.IsLockedOut)
                {
                    throw new Exception($"Пользователь заблокирован");
                }
            }

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

                _jwtTokenManager.GenerateJwtToken(claims, TokenConstants.TokenLifeTime);

                return new UserModel
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    AccessFailedCount = user.AccessFailedCount,
                    FirstName = user.FirstName,
                    Id = user.Id,
                    LastName = user.LastName,
                    LockoutEnabled = user.LockoutEnabled,
                    PhoneNumber = user.PhoneNumber,
                    TwoFactorEnabled = user.TwoFactorEnabled,
                    IsLocked = user.LockoutEnd != null && user.LockoutEnd >= DateTime.UtcNow,
                    Roles = await _userManager.GetRolesAsync(user)
                };
            }
            else
            {
                throw new Exception("Имя пользователя или пароль неверный");
            }
        }

        public async Task<bool> CheckLoginAsync(string login)
        {
            var user = await _userManager.FindByNameAsync(login);

            if (user != null)
            {
                if (login.ToLower().Equals(user.UserName.ToLower(), StringComparison.InvariantCultureIgnoreCase))
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<IEnumerable<UserModel>> GetAllAsync()
        {
            return await _userManager.Users.Select(user => new UserModel
            {
                UserName = user.UserName,
                Email = user.Email,
                AccessFailedCount = user.AccessFailedCount,
                FirstName = user.FirstName,
                Id = user.Id,
                LastName = user.LastName,
                LockoutEnabled = user.LockoutEnabled,
                PhoneNumber = user.PhoneNumber,
                TwoFactorEnabled = user.TwoFactorEnabled,
                IsLocked = user.LockoutEnd != null && user.LockoutEnd >= DateTime.UtcNow
            }).ToListAsync();
        }

        public async Task<UserModel> GetByIdAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            return new UserModel
            {
                UserName = user.UserName,
                Email = user.Email,
                AccessFailedCount = user.AccessFailedCount,
                FirstName = user.FirstName,
                Id = user.Id,
                LastName = user.LastName,
                LockoutEnabled = user.LockoutEnabled,
                PhoneNumber = user.PhoneNumber,
                TwoFactorEnabled = user.TwoFactorEnabled,
                IsLocked = user.LockoutEnd != null && user.LockoutEnd >= DateTime.UtcNow,
                Roles = await _userManager.GetRolesAsync(user)
            };
        }

        public async Task<bool> RegisterAsync(CreateUserModel user)
        {
            var oldUser = await _userManager.FindByNameAsync(user.UserName);

            if (oldUser != null) throw new Exception($"Пользователь с логином '{user.UserName}' существует");

            var newUser = new User
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                Email = user.Email,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber
            };

            var result = await _userManager.CreateAsync(newUser, user.Password);

            if (result.Succeeded)
            {
                await _userManager.AddClaimAsync(newUser, new Claim(ClaimTypes.Role, "User"));

                var role = await _roleManager.Roles.FirstOrDefaultAsync(role => role.Name == "User");

                if (role != null)
                {
                    if (!await _userManager.IsInRoleAsync(newUser, role.Name))
                    {
                        await _userManager.AddToRoleAsync(newUser, role.Name);
                    }
                }
                else
                {
                    throw new Exception($"Роль '{role.Name}' не найдена в системе");
                }
            }
            else
            {
                throw new Exception(string.Join("\n", result.Errors.Select(error => $"{error.Code} {error.Description}")));
            }

            return true;
        }

        public void UpdateToken() => _jwtTokenManager.UpdateToken();
    }
}