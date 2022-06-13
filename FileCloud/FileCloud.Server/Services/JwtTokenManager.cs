﻿using FileCloud.Server.Abstractions;
using FileCloud.Server.Models.Auth;
using Microsoft.Extensions.Options;
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

        private readonly JwtAuthSettings _jwtAuthSettings;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public JwtTokenManager(IHttpContextAccessor httpContextAccessor, IOptions<JwtAuthSettings> options)
        {
            _httpContextAccessor = httpContextAccessor;
            _jwtAuthSettings = options.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public void GenerateJwtToken(IEnumerable<Claim> claims)
        {
            byte[] secretBytes = Encoding.UTF8.GetBytes(_jwtAuthSettings.SecretKey);
            var key = new SymmetricSecurityKey(secretBytes);

            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var currenttime = DateTime.UtcNow;

            var token = new JwtSecurityToken(
                _jwtAuthSettings.Issuer,
                _jwtAuthSettings.Audience,
                claims,
                notBefore: currenttime,
                expires: currenttime.AddSeconds(_jwtAuthSettings.TokenLifeTime),
                signingCredentials: signingCredentials);

            _httpContextAccessor.HttpContext.Response.Headers.Add("JwtTokenExpires", _jwtAuthSettings.TokenLifeTime.ToString());
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
            //var expires = _jwtAuthSettings.TokenLifeTime;

            GenerateJwtToken(jwtToken.Claims);
        }
    }
}
