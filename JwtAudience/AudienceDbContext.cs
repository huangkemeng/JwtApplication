using Microsoft.EntityFrameworkCore;

namespace JwtAudience
{
    public class AudienceDbContext : DbContext
    {
        public AudienceDbContext(DbContextOptions options) : base(options) { }

        /// <summary>
        /// 失效 Token 黑名单
        /// </summary>
        public DbSet<BlackRecord> BlackRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<BlackRecord>()
                .HasKey(r => r.Jti);
        }
    }

    public class BlackRecord
    {
        public string Jti { get; set; }
        public string UserId { get; set; }
    }
}