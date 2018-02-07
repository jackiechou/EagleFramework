using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Eagle.EntityFramework
{
    public class AppIdentityDbContext : IdentityDbContext, IAppIdentityDbContext
    {
        public AppIdentityDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            Database.SetInitializer<AppIdentityDbContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ConfigureMapping(modelBuilder);
        }

        internal static void ConfigureMapping(DbModelBuilder modelBuilder)
        {
        }
    }
}
