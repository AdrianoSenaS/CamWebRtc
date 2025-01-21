using CamWebRtc.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CamWebRtc.Infrastructure.Config
{
    public static class AppDbConfiguration
    {
        public static void AppConfigureDb(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite("Data Source=Database.db");
            });
        }
        public static void AppMigrations(this IServiceProvider scope)
        {
            try
            {
                var context = scope.GetRequiredService<AppDbContext>();
                context.Database.Migrate();
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
