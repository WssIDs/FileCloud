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
        Task<AuthenticateResponseModel> AuthAsync(AuthenticateRequestModel authenticateRequest);

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
    }
}