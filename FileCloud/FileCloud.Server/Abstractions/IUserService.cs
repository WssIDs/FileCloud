using FileCloud.Server.Models.Auth;

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
        Task<IEnumerable<object>> GetAllAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<object> GetByIdAsync(Guid id);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        AuthenticateTokenResponseModel GetToken();
    }
}