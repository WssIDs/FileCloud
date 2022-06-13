using System.Security.Claims;

namespace FileCloud.Server.Abstractions
{
    public interface IJwtTokenManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        void GenerateJwtToken(IEnumerable<Claim> claims);

        void UpdateToken();
    }
}
