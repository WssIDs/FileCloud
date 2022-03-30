using FileCloud.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FileCloud.Data.Store
{
    /// <summary>
    /// 
    /// </summary>
    public class FileCloudDbContext : IdentityDbContext<User, Role, Guid>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public FileCloudDbContext(DbContextOptions<FileCloudDbContext> options)
            : base(options)
        {
        }
    }
}