using CamWebRtc.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CamWebRtc.Infrastructure.Data
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) { }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<CamModel> Cams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>().ToTable("Users");
            modelBuilder.Entity<CamModel>().ToTable("Cams");
        }

    }
}
