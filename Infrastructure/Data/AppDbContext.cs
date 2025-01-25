using CamWebRtc.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CamWebRtc.Infrastructure.Data
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) { }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<CamModel> Cams { get; set; }
        public DbSet<StreamModel> Streams { get; set; }
        public DbSet<IceServersModel> IceServers { get; set; }
        public DbSet<StunServersUrls> StunServers { get; set; }
        public DbSet<TurnServersUrls> TurnServers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>().ToTable("Users");
            modelBuilder.Entity<CamModel>().ToTable("Cams");
            modelBuilder.Entity<StreamModel>().ToTable("Stream");
            modelBuilder.Entity<IceServersModel>()
                .HasMany(e => e.UrlsStun)
                .WithOne()
                .HasForeignKey("IceServerId");
            modelBuilder.Entity<IceServersModel>()
              .HasMany(e => e.urlsTurn)
              .WithOne()
              .HasForeignKey("IceServerId");
        }

    }
}
