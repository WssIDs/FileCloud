using System.Security.Claims;

namespace FileCloud.Server.Abstractions
{
    public interface IJwtTokenManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="expires">expires time in seconds</param>
        /// <returns></returns>
        void GenerateJwtToken(IEnumerable<Claim> claims, int expires);

        void UpdateToken();
    }
}
