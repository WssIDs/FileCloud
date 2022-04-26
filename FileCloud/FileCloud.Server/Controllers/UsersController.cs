using FileCloud.Server.Abstractions;
using FileCloud.Server.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FileCloud.Server.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userService"></param>
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<object>> GetAsync() => await _userService.GetAllAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("{id}")]
        public async Task<object> GetAsync(Guid id) => await _userService.GetByIdAsync(id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authenticateRequest"></param>
        /// <returns></returns>
        [HttpPost("Authenticate")]
        public async Task<ActionResult<AuthenticateResponseModel>> AuthenticateAsync([FromBody] AuthenticateRequestModel authenticateRequest)
        {
            try
            {
                return await _userService.AuthAsync(authenticateRequest);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("GetToken")]
        public void UpdateToken() => _userService.UpdateToken();
    }
}