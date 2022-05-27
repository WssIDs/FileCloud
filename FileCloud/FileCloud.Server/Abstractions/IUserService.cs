using FileCloud.Shared.Models;
using FileCloud.Shared.Models.Auth;

namespace FileCloud.Server.Abstractions
{
    public interface IUserService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="authenticateRequest"></param>
        /// <returns></returns>
        Task<UserModel> AuthAsync(AuthenticateRequestModel authenticateRequest);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<UserModel>> GetAllAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UserModel> GetByIdAsync(Guid id);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        void UpdateToken();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        Task<bool> CheckLoginAsync(string login);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<bool> RegisterAsync(CreateUserModel user);
    }
}