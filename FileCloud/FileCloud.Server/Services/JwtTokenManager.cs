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
        public void GenerateJwtToken(IEnumerable<Claim> claims, int expires)
        {
            byte[] secretBytes = Encoding.UTF8.GetBytes(TokenConstants.SecretKey);
            var key = new SymmetricSecurityKey(secretBytes);

            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var currenttime = DateTime.UtcNow;

            var token = new JwtSecurityToken(
                TokenConstants.Issuer,
                TokenConstants.Audience,
                claims,
                notBefore: currenttime,
                expires: currenttime.AddSeconds(expires),
                signingCredentials: signingCredentials);

            _httpContextAccessor.HttpContext.Response.Headers.Add("JwtTokenExpires", expires.ToString());
            _httpContextAccessor.HttpContext.Response.Headers.Add("JwtToken", new JwtSecurityTokenHandler().WriteToken(token));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public void UpdateToken()
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", string.Empty);

            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

            // seconds
            var expires = 60;

            GenerateJwtToken(jwtToken.Claims, expires);
        }
    }
}
