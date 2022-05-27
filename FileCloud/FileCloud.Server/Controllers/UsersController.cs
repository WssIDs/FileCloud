using FileCloud.Server.Abstractions;
using FileCloud.Shared.Models;
using FileCloud.Shared.Models.Auth;
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
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<IEnumerable<UserModel>> GetAsync() => await _userService.GetAllAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        [Route("{id}")]
        public async Task<UserModel> GetAsync(Guid id) => await _userService.GetByIdAsync(id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authenticateRequest"></param>
        /// <returns></returns>
        [HttpPost("Authenticate")]
        public async Task<ActionResult<UserModel>> AuthenticateAsync([FromBody] AuthenticateRequestModel authenticateRequest)
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
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpGet("CheckLogin/{login}")]
        public async Task<ActionResult<bool>> CheckLoginAsync(string login) => await _userService.CheckLoginAsync(login);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("UpdateToken")]
        public void UpdateToken() => _userService.UpdateToken();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        public async Task<ActionResult<bool>> RegisterAsync(CreateUserModel user)
        {
            try
            {
                return await _userService.RegisterAsync(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}