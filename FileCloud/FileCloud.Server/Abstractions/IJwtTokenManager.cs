using System.Security.Claims;

namespace FileCloud.Server.Abstractions
{
    public interface IJwtTokenManager
    {
        string GenerateJwtToken(IEnumerable<Claim> claims, DateTime expires);

        string UpdateToken();
    }
}
