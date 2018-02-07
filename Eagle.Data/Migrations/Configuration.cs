using System.Data.Entity.Migrations;
using Eagle.Data.DefaultData;

namespace Eagle.Data.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            CommonData.Set(context);
            context.SaveChanges();
        }
    }
}