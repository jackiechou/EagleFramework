using System.Data.Entity;
using Eagle.Core.Configuration;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework.EntityMapping;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Eagle.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
        : base(Settings.ConnectionString, throwIfV1Schema: false)
        {

        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
       
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            BaseMap.ConfigureMapping(modelBuilder);
            base.OnModelCreating(modelBuilder);
            modelBuilder.Ignore<User>();
        }
    }
}
