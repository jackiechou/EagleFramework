using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using Eagle.Entities.Skins;
using Eagle.Entities.SystemManagement;
using Eagle.Entities.SystemManagement.FileStorage;

namespace Eagle.EntityFramework.EntityMapping
{
    public static class SystemManagementMap
    {
        public static void Configure(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationEntity>().ToTable("Application");
            modelBuilder.Entity<ApplicationSetting>().ToTable("ApplicationSetting");
            modelBuilder.Entity<ApplicationLanguage>().ToTable("ApplicationLanguage");

            modelBuilder.Entity<Country>().ToTable("Country");
            modelBuilder.Entity<Province>().ToTable("Province");
            modelBuilder.Entity<Region>().ToTable("Region");

            modelBuilder.Entity<Address>().ToTable("Address");
            modelBuilder.Entity<Contact>().ToTable("Contact");
            modelBuilder.Entity<Language>().ToTable("Language");

            modelBuilder.Entity<AppClaim>().ToTable("AppClaim");

            modelBuilder.Entity<Role>().ToTable("Role");
            modelBuilder.Entity<Group>().ToTable("Group").HasKey(s => new { s.GroupId }).Property(c => c.GroupId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<RoleGroup>().ToTable("RoleGroup").HasKey(s => new { s.RoleId, s.GroupId });
            modelBuilder.Entity<User>().ToTable("User", "dbo").HasKey(s => new { s.UserId }).Property(c => c.SeqNo).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);         
            //modelBuilder.Entity<User>().HasRequired(x => x.Profile).WithRequiredDependent(x => x.User);
            //modelBuilder.Entity<User>().HasMany(u => u.Roles).WithMany(r => r.Users)
            //.Map(m =>
            //{
            //    m.ToTable("UserRoles");ProfileId
            //    m.MapLeftKey("UserId");
            //    m.MapRightKey("RoleId");
            //});
            modelBuilder.Entity<UserAddress>().ToTable("UserAddress").HasKey(s => new { s.UserId, s.AddressId });
            modelBuilder.Entity<UserProfile>().ToTable("UserProfile").HasKey(s => new { s.ProfileId });
            modelBuilder.Entity<UserRole>().ToTable("UserRole").HasKey(s => new { s.UserId, s.RoleId });
            modelBuilder.Entity<UserRoleGroup>().ToTable("UserRoleGroup");
            modelBuilder.Entity<UserVendor>().ToTable("UserVendor").HasKey(s => new { s.UserId, s.VendorId });
            //modelBuilder.Entity<UsersOnline>().ToTable("UsersOnline");

            modelBuilder.Entity<Menu>().ToTable("Menu");
            modelBuilder.Entity<MenuType>().ToTable("MenuType");
            modelBuilder.Entity<MenuPosition>().ToTable("MenuPosition");
            modelBuilder.Entity<MenuPermissionLevel>().ToTable("MenuPermissionLevel");
            modelBuilder.Entity<MenuPermission>().ToTable("MenuPermission");
            modelBuilder.Entity<SiteMap>().ToTable("SiteMap");

            modelBuilder.Entity<Module>().ToTable("Module");
            modelBuilder.Entity<ModuleCapability>().ToTable("ModuleCapability");
            modelBuilder.Entity<ModuleSetting>().ToTable("ModuleSetting");
            modelBuilder.Entity<ModulePermission>().ToTable("ModulePermission");

            modelBuilder.Entity<Page>().ToTable("Page").HasKey(s => new { s.PageId }).Property(c => c.PageId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<PageModule>().ToTable("PageModule").HasKey(s => new {s.PageId, s.ModuleId});
            modelBuilder.Entity<PageSetting>().ToTable("PageSetting");
            modelBuilder.Entity<PagePermission>().ToTable("PagePermission").HasKey(p => new { p.RoleId, p.PageId });
            
            modelBuilder.Entity<DocumentFile>().ToTable("DocumentFile");            
            modelBuilder.Entity<DocumentFolder>().ToTable("DocumentFolder");
            modelBuilder.Entity<DownloadTracking>().ToTable("DownloadTracking");

            modelBuilder.Entity<ContentType>().ToTable("ContentType");
            modelBuilder.Entity<ContentItem>().ToTable("ContentItem");


            modelBuilder.Entity<SkinPackage>().ToTable("SkinPackage");
            modelBuilder.Entity<SkinPackageBackground>().ToTable("SkinPackageBackground");
            modelBuilder.Entity<SkinPackageTemplate>().ToTable("SkinPackageTemplate");
            modelBuilder.Entity<SkinPackageType>().ToTable("SkinPackageType");

            //modelBuilder.Configurations.Add(new MenuPermissionMap());
        }


        //#region Common

        //private class RoleGroupReferenceMap : EntityTypeConfiguration<RoleGroupReference>
        //{
        //    public RoleGroupReferenceMap()
        //    {
        //        ToTable("RoleGroupReference");
        //        // Primary Key
        //        HasKey(t => new { t.RoleId, t.RoleGroupId });

        //        Property(t => t.RoleId).HasColumnName("RoleId").IsRequired();
        //        Property(t => t.RoleGroupId).HasColumnName("RoleGroupId").IsRequired();

        //        //HasRequired(t => t.Role)
        //        //.WithMany(t => t.UserRoles)
        //        //    .HasForeignKey(d => d.RoleId)
        //        //        .WillCascadeOnDelete(false);

        //        //HasRequired(t => t.RoleGroup)
        //        //.WithMany(t => t.Users)
        //        //    .HasForeignKey(d => d.RoleGroupId)
        //        //        .WillCascadeOnDelete(false);
        //    }
        //}

        //#endregion
    }
}
