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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<PathData>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
        }

        public DbSet<PathData> Paths { get; set; }

        public DbSet<FileInfoData> Files { get; set; }
    }
}