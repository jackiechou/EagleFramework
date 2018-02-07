using System.Data.Entity;

namespace Eagle.EntityFramework.EntityMapping
{
    public static class BaseMap
    {
        //Modification - Register Table Entites 
        public static void ConfigureMapping(DbModelBuilder modelBuilder)
        {
            SystemManagementMap.Configure(modelBuilder);
            ContentMap.Configure(modelBuilder);
            BusinessMap.Configure(modelBuilder);
            ServiceMap.Configure(modelBuilder);

            // ignore a type that is not mapped to a database table
            //modelBuilder.Ignore<MailTemplateExt>();
        }
    }
}
