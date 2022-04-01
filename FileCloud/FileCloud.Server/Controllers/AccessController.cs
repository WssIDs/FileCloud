using FileCloud.Server.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FileCloud.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccessController : ControllerBase
    {
        private readonly IAuthManager _authManager;

        public AccessController(IAuthManager authManager)
        {
            _authManager = authManager;
        }

        [Authorize()]
        [HttpGet("GetToken")]
        public string GetToken()
        {
            return "";
        }

        [AllowAnonymous]
        [HttpGet("Authenticate")]
        public string Authenticate()
        {
            return _authManager.Auth();
        }
    }
}