using FileCloud.Server.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FileCloud.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IAuthManager _authManager;

        public UsersController(IAuthManager authManager)
        {
            _authManager = authManager;
        }

        [Authorize()]
        [HttpGet("GetToken")]
        public string GetToken()
        {
            return "";
        }

        [HttpGet("Authenticate")]
        public async Task<ActionResult<string>> Authenticate(string userName, string password)
        {
            try
            {
                return await _authManager.AuthAsync(userName, password);
            }
            catch(Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}