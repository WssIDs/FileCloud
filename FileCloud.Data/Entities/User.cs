using Microsoft.AspNetCore.Identity;

namespace FileCloud.Data.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}