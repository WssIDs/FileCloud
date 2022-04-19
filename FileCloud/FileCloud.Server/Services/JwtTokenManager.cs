using FileCloud.Server.Abstractions;
using FileCloud.Server.Models.Auth;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FileCloud.Server.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class JwtTokenManager : IJwtTokenManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public JwtTokenManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="expires"></param>
        /// <returns></returns>
        public string GenerateJwtToken(IEnumerable<Claim> claims, DateTime expires)
        {
            byte[] secretBytes = Encoding.UTF8.GetBytes(TokenConstants.SecretKey);
            var key = new SymmetricSecurityKey(secretBytes);

            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                TokenConstants.Issuer,
                TokenConstants.Audience,
                claims,
                notBefore: DateTime.UtcNow,
                expires: expires,
                signingCredentials: signingCredentials);

            _httpContextAccessor.HttpContext.Response.Headers.Add("JwtTokenExpires", expires.ToString());

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string UpdateToken()
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", string.Empty);

            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

            var expires = DateTime.UtcNow.AddMinutes(1);

            return GenerateJwtToken(jwtToken.Claims, expires);
        }
    }
}
