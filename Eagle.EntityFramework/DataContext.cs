using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Eagle.Core.Common;
using Eagle.Entities;
using Eagle.Entities.Business.Brand;
using Eagle.Entities.Business.Customers;
using Eagle.Entities.Business.Employees;
using Eagle.Entities.Business.Manufacturers;
using Eagle.Entities.Business.Orders;
using Eagle.Entities.Business.Products;
using Eagle.Entities.Business.Roster;
using Eagle.Entities.Business.Shipping;
using Eagle.Entities.Business.Transactions;
using Eagle.Entities.Business.Vendors;
using Eagle.Entities.Common;
using Eagle.Entities.Contents.Articles;
using Eagle.Entities.Contents.Banners;
using Eagle.Entities.Contents.Documents;
using Eagle.Entities.Contents.Feedbacks;
using Eagle.Entities.Contents.Galleries;
using Eagle.Entities.Contents.Media;
using Eagle.Entities.Services.Booking;
using Eagle.Entities.Services.Chat;
using Eagle.Entities.Services.Events;
using Eagle.Entities.Services.Messaging;
using Eagle.Entities.Skins;
using Eagle.Entities.SystemManagement;
using Eagle.Entities.SystemManagement.FileStorage;
using Eagle.EntityFramework.EntityMapping;

namespace Eagle.EntityFramework
{
    public class DataContext : DbContext, IDataContext
    {

        public DataContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            Database.SetInitializer<DataContext>(null);
            Configuration.ProxyCreationEnabled = true;
            Configuration.LazyLoadingEnabled = true;

            InstanceId = Guid.NewGuid();

            // Get the ObjectContext related to this DbContext
            //Database.CommandTimeout = 5 * 180; // value in seconds
            Database.CommandTimeout = 0;

            // Sets DateTimeKinds on DateTimes of Entities, so that Dates are automatically set to be UTC.
            // Currently only processes CleanEntityBase entities. All EntityBase entities remain unchanged.
            // http://stackoverflow.com/questions/4648540/entity-framework-datetime-and-utc
            ((IObjectContextAdapter)this).ObjectContext.ObjectMaterialized += (sender, e) => DateTimeKindAttribute.Apply(e.Entity);
        }

        public Guid InstanceId { get; }

        #region DECLARE TABLES

        public virtual DbSet<ApplicationEntity> Applications { get; set; }
        public virtual DbSet<ApplicationSetting> ApplicationSettings { get; set; }
        public virtual DbSet<AppClaim> AppClaims { get; set; }
        public virtual DbSet<CurrencyGroup> Currencies { get; set; }
        public virtual DbSet<CurrencyRate> CurrencyRates { get; set; }
        public virtual DbSet<CreditCard> CreditCards { get; set; }

        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Province> Provinces { get; set; }
        public virtual DbSet<Region> Regions { get; set; }

        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<ApplicationLanguage> ApplicationLanguages { get; set; }


        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<GroupClaim> GroupClaims { get; set; }
        public virtual DbSet<RoleGroup> RoleGroups { get; set; }


        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserProfile> Profiles { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<UserRoleGroup> UserRoleGroups { get; set; }



        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<PagePermission> PagePermissions { get; set; }
        public virtual DbSet<ModulePermission> ModulePermissions { get; set; }



        public virtual DbSet<ContentType> ContentTypes { get; set; }
        public virtual DbSet<ContentItem> ContentItems { get; set; }

        public virtual DbSet<Page> Pages { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<ModuleSetting> ModuleSettings { get; set; }
        public virtual DbSet<ModuleGroup> ModuleGroups { get; set; }
        public virtual DbSet<ModuleGroupReference> ModuleGroupReferences { get; set; }
        public virtual DbSet<ModuleCapability> ModuleCapabilities { get; set; }
        public virtual DbSet<PageModule> PageModules { get; set; }

        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<MenuType> MenuTypes { get; set; }
        public virtual DbSet<MenuPosition> MenuPositions { get; set; }
        public virtual DbSet<MenuPermissionLevel> MenuPermissionLevels { get; set; }
        public virtual DbSet<MenuPermission> MenuPermissions { get; set; }
        public virtual DbSet<SiteMap> SiteMaps { get; set; }

        public virtual DbSet<SkinPackage> SkinPackages { get; set; }
        public virtual DbSet<SkinPackageTemplate> SkinPackageTemplates { get; set; }
        public virtual DbSet<SkinPackageBackground> SkinPackageBackgrounds { get; set; }
        public virtual DbSet<SkinPackageType> SkinPackageTypes { get; set; }


        public virtual DbSet<Documentation> Documentation { get; set; }
        public virtual DbSet<DocumentFile> DocumentFile { get; set; }
        public virtual DbSet<DocumentFolder> DocumentFolder { get; set; }
        public virtual DbSet<DownloadTracking> DownloadTracking { get; set; }


        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<NewsCategory> NewsCategories { get; set; }
        public virtual DbSet<NewsComment> NewsComment { get; set; }
        public virtual DbSet<NewsRating> NewsRatings { get; set; }

        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<EventType> EventTypes { get; set; }


        public virtual DbSet<Feedback> Feedbacks { get; set; }


        public virtual DbSet<Banner> Banners { get; set; }
        public virtual DbSet<BannerType> BannerTypes { get; set; }
        public virtual DbSet<BannerPosition> BannerPositions { get; set; }
        public virtual DbSet<BannerScope> BannerScopes { get; set; }
        public virtual DbSet<BannerPage> BannerPages { get; set; }
        public virtual DbSet<BannerZone> BannerZones { get; set; }


        public virtual DbSet<GalleryCollection> GalleryCollections { get; set; }
        public virtual DbSet<GalleryTopic> GalleryTopics { get; set; }
        public virtual DbSet<GalleryFile> GalleryFiles { get; set; }

        public virtual DbSet<MediaAlbum> MediaAlbums { get; set; }
        public virtual DbSet<MediaAlbumFile> MediaAlbumFiles { get; set; }
        public virtual DbSet<MediaArtist> MediaArtists { get; set; }
        public virtual DbSet<MediaComposer> MediaComposers { get; set; }
        public virtual DbSet<MediaFile> MediaFiles { get; set; }
        public virtual DbSet<MediaPlayList> MediaPlayLists { get; set; }
        public virtual DbSet<MediaPlayListFile> MediaPlayListFiles { get; set; }
        public virtual DbSet<MediaTopic> MediaTopics { get; set; }
        public virtual DbSet<MediaType> MediaTypes { get; set; }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductAttribute> ProductAttributes { get; set; }
        public virtual DbSet<ProductAttributeOption> ProductAttributeOptions { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        public virtual DbSet<ProductDiscount> ProductDiscounts { get; set; }
        public virtual DbSet<ProductTaxRate> ProductTaxRates { get; set; }
        public virtual DbSet<ProductType> ProductTypes { get; set; }
        public virtual DbSet<ProductVote> ProductVotes { get; set; }
        public virtual DbSet<ProductFile> ProductFiles { get; set; }
        public virtual DbSet<ProductAlbum> ProductAlbums { get; set; }
        public virtual DbSet<ProductRating> ProductRatings { get; set; }
        public virtual DbSet<Entities.Business.Products.Attribute> Attributes { get; set; }
        public virtual DbSet<AttributeOption> AttributeOptions { get; set; }
        public virtual DbSet<Brand> Brand { get; set; }

        public virtual DbSet<Manufacturer> Manufacturers { get; set; }
        public virtual DbSet<ManufacturerCategory> ManufacturerCategorys { get; set; }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<CustomerType> CustomerTypes { get; set; }


        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderPayment> OrderPayments { get; set; }
        public virtual DbSet<OrderProduct> OrderProducts { get; set; }
        public virtual DbSet<OrderProductOption> OrderProductOptions { get; set; }
        public virtual DbSet<OrderShipment> OrderShipments { get; set; }
        public virtual DbSet<OrderTemp> OrderTemps { get; set; }
        public virtual DbSet<OrderProductTemp> OrderTempOrderProductTemps { get; set; }


        public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }
        public virtual DbSet<Promotion> Promotions { get; set; }
        public virtual DbSet<ShippingCarrier> ShippingCarriers { get; set; }
        public virtual DbSet<ShippingMethod> ShippingMethods { get; set; }
        public virtual DbSet<ShippingFee> ShippingFees { get; set; }
        public virtual DbSet<TransactionMethod> TransactionMethods { get; set; }


        public virtual DbSet<Vendor> Vendors { get; set; }
        public virtual DbSet<VendorAddress> VendorAddresses { get; set; }
        public virtual DbSet<VendorCurrency> VendorCurrencies { get; set; }
        public virtual DbSet<VendorPartner> VendorPartners { get; set; }
        public virtual DbSet<VendorShippingCarrier> VendorShippingCarriers { get; set; }
        public virtual DbSet<VendorShippingMethod> VendorShippingMethods { get; set; }
        public virtual DbSet<VendorPaymentMethod> VendorPaymentMethods { get; set; }
        public virtual DbSet<VendorTransactionMethod> VendorTransactionMethods { get; set; }

        public virtual DbSet<ServicePack> ServicePacks { get; set; }
        public virtual DbSet<ServicePackOption> ServicePackOptions { get; set; }
        public virtual DbSet<ServicePackDuration> ServicePackDurations { get; set; }
        public virtual DbSet<ServicePackProvider> ServicePackProviders { get; set; }
        public virtual DbSet<ServicePackRating> ServicePackRatings { get; set; }
        public virtual DbSet<ServicePackType> ServicePackTypes { get; set; }
        public virtual DbSet<ServiceDiscount> ServiceDiscounts { get; set; }
        public virtual DbSet<ServiceCategory> ServiceCategorys { get; set; }
        public virtual DbSet<ServicePeriod> ServicePeriods { get; set; }
        public virtual DbSet<ServiceTaxRate> ServiceTaxRates { get; set; }
      
        public virtual DbSet<ChatMessage> ChatMessages { get; set; }
        public virtual DbSet<ChatPrivateMessage> ChatPrivateMessages { get; set; }
        public virtual DbSet<ChatPrivateMessageMaster> ChatPrivateMessageMasters { get; set; }
        public virtual DbSet<ChatUser> ChatUsers { get; set; }

        public virtual DbSet<MailServerProvider> MailServerProviders { get; set; }
        public virtual DbSet<MessageQueue> MessageQueues { get; set; }
        public virtual DbSet<MessageTemplate> MessageTemplates { get; set; }
        public virtual DbSet<MessageType> MessageTypes { get; set; }
        public virtual DbSet<NotificationType> NotificationTypes { get; set; }
        public virtual DbSet<NotificationSender> NotificationSenders { get; set; }
        public virtual DbSet<NotificationTarget> NotificationTargets { get; set; }
        public virtual DbSet<NotificationTargetDefault> NotificationTargetDefaults { get; set; }
        public virtual DbSet<NotificationMessageType> NotificationMessageTypes { get; set; }
        public virtual DbSet<NotificationMessage> NotificationMessages { get; set; }

        public virtual DbSet<Contract> Contracts { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EmployeeAvailability> EmployeeAvailabilities { get; set; }
        public virtual DbSet<EmployeePosition> EmployeePositions { get; set; }
        public virtual DbSet<EmployeeSkill> EmployeeSkills { get; set; }
        public virtual DbSet<EmployeeTimeOff> EmployeeTimeOffs { get; set; }
        public virtual DbSet<JobPosition> JobPositions { get; set; }
        public virtual DbSet<JobPositionSkill> JobPositionSkills { get; set; }
        public virtual DbSet<Qualification> Qualifications { get; set; }
        public virtual DbSet<RewardDiscipline> RewardDisciplines { get; set; }
        public virtual DbSet<Salary> Salaries { get; set; }
        public virtual DbSet<Skill> Skills { get; set; }
        public virtual DbSet<Termination> Terminations { get; set; }
        public virtual DbSet<WorkingHistory> WorkingHistory { get; set; }
       

        public virtual DbSet<PublicHolidaySet> PublicHolidaySets { get; set; }
        public virtual DbSet<PublicHoliday> PublicHolidays { get; set; }
        public virtual DbSet<Shift> Shifts { get; set; }
        public virtual DbSet<ShiftPosition> ShiftPositions { get; set; }
        public virtual DbSet<ShiftSwap> ShiftSwaps { get; set; }
        public virtual DbSet<ShiftType> ShiftTypes { get; set; }
        public virtual DbSet<TimeOffType> TimeOffTypes { get; set; }
        public virtual DbSet<Timesheet> Timesheets { get; set; }

        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));
            base.OnModelCreating(modelBuilder);

#if DEBUG
            Database.Log = s => Debug.Write(s);
#endif

            BaseMap.ConfigureMapping(modelBuilder);
        }

        #region Initialization

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }
        public TEntity FindById<TEntity>(params object[] ids) where TEntity : class
        {
            return Set<TEntity>().Find(ids);
        }

        public IQueryable<TEntity> Get<TEntity>() where TEntity : class
        {
            return Set<TEntity>();
        }

        public IQueryable<TEntity> Get<TEntity>(string storedProcedureName, params object[] args) where TEntity : class
        {
            IQueryable<TEntity> result;

            if (args.Any(a => a is SqlParameter &&
                                (((SqlParameter)a).Direction == ParameterDirection.Output || ((SqlParameter)a).Direction == ParameterDirection.Output)))
            {
                result = GetWithOutParameters<TEntity>(storedProcedureName, args);
            }
            else
            {
                var query = Database.SqlQuery<TEntity>(storedProcedureName, args).ToList();
                foreach (var entity in query)
                {
                    DateTimeKindAttribute.Apply(entity);
                }
                result = query.AsQueryable();
            }

            return result;
        }

        public void BulkInsert<TEntity>(IList<TEntity> insertList, string tableName, IList<SqlBulkCopyColumnMapping> mapping, DataTable table) where TEntity : class
        {
            using (var connection = new SqlConnection(Database.Connection.ConnectionString))
            {
                connection.Open();

                using (var bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.BatchSize = 100;
                    bulkCopy.DestinationTableName = tableName;

                    foreach (var columnMapping in mapping)
                    {
                        bulkCopy.ColumnMappings.Add(columnMapping);
                    }

                    bulkCopy.WriteToServer(table);
                }

                connection.Close();
            }
        }

        public IQueryable<TEntity> SelectQuery<TEntity>(string query, params object[] args) where TEntity : class
        {
            return Database.SqlQuery<TEntity>(query, args).AsQueryable();
        }
        IQueryable<TEntity> IDataContext.Get<TEntity>(string storedProcedureName, params object[] args)
        {
            return Get<TEntity>(storedProcedureName, args);
        }
        private IQueryable<TEntity> GetWithOutParameters<TEntity>(string storedProcedureName, params object[] args)
        {
            var isAlreadyOpen = Database.Connection.State == ConnectionState.Open;

            if (!isAlreadyOpen)
            {
                Database.Connection.Open();
            }

            var cmd = Database.Connection.CreateCommand();
            cmd.CommandText = storedProcedureName;
            cmd.CommandType = CommandType.Text;

            foreach (SqlParameter arg in args)
            {
                cmd.Parameters.Add(arg);
            }

            var reader = cmd.ExecuteReader();

            var objectResults = ((IObjectContextAdapter)Database).ObjectContext.Translate<TEntity>(reader, typeof(TEntity).Name, MergeOption.AppendOnly);

            var nextResultSet = reader.NextResult();
            foreach (SqlParameter arg in args)
            {
                if (arg.Direction != ParameterDirection.InputOutput && arg.Direction != ParameterDirection.Output) continue;
                var xml = nextResultSet.ToXml();
            }

            var result = objectResults.AsQueryable();

            if (!isAlreadyOpen)
            {
                Database.Connection.Close();
            }

            return result;
        }
        public TEntity Insert<TEntity>(TEntity entity) where TEntity : class
        {
            var result = base.Set<TEntity>().Add(entity);

            var creationTrackingEntity = entity as IEntityTrackingCreation;
            if (creationTrackingEntity != null)
            {
                creationTrackingEntity.DateCreated = DateTime.UtcNow;
            }

          ((IObjectState)entity).State = ObjectState.Added;
            return result;
        }

        public void Update<TEntity>(TEntity entity) where TEntity : class
        {
            base.Set<TEntity>().Attach(entity);

            var modifyTrackingEntity = entity as IEntityTrackingModified;
            if (modifyTrackingEntity != null)
            {
                modifyTrackingEntity.DateModified = DateTime.UtcNow;
            }

            ((IObjectState)entity).State = ObjectState.Modified;
        }
        public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            ((IObjectState)entity).State = ObjectState.Deleted;
            base.Set<TEntity>().Attach(entity);
            base.Set<TEntity>().Remove(entity);
        }
        public void Delete<TEntity>(params object[] ids) where TEntity : class
        {
            var entity = FindById<TEntity>(ids);
            ((IObjectState)entity).State = ObjectState.Deleted;
            base.Set<TEntity>().Attach(entity);
            base.Set<TEntity>().Remove(entity);
        }

        //public int SaveChanges<TEntity>() where TEntity : class
        //{
        //    var result = DbContext.SaveChanges();
        //    return result;
        //}

        public int Execute(string sqlCommand)
        {
            return Database.ExecuteSqlCommand(sqlCommand);
        }

        public int Execute(string sqlCommand, params object[] args)
        {
            var result = Database.ExecuteSqlCommand(sqlCommand, args);
            return result;
        }

        /// <summary>
        ///     Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <exception cref="System.Data.Entity.Infrastructure.DbUpdateException">
        ///     An error occurred sending updates to the database.</exception>
        /// <exception cref="System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
        ///     A database command did not affect the expected number of rows. This usually
        ///     indicates an optimistic concurrency violation; that is, a row has been changed
        ///     in the database since it was queried.</exception>
        /// <exception cref="System.Data.Entity.Validation.DbEntityValidationException">
        ///     The save was aborted because validation of entity property values failed.</exception>
        /// <exception cref="System.NotSupportedException">
        ///     An attempt was made to use unsupported behavior such as executing multiple
        ///     asynchronous commands concurrently on the same context instance.</exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     The context or connection have been disposed.</exception>
        /// <exception cref="System.InvalidOperationException">
        ///     Some error occurred attempting to process entities in the context either
        ///     before or after sending commands to the database.</exception>
        /// <seealso cref="DbContext.SaveChanges"/>
        /// <returns>The number of objects written to the underlying database.</returns>
        public override int SaveChanges()
        {
            SyncObjectsStatePreCommit();
            var changes = base.SaveChanges();
            SyncObjectsStatePostCommit();
            return changes;
        }

        public override Task<int> SaveChangesAsync()
        {
            SyncObjectsStatePreCommit();
            var changesAsync = base.SaveChangesAsync();
            SyncObjectsStatePostCommit();
            return changesAsync;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            SyncObjectsStatePreCommit();
            var changesAsync = base.SaveChangesAsync(cancellationToken);
            SyncObjectsStatePostCommit();
            return changesAsync;
        }
        public void SyncObjectState(object entity)
        {
            Entry(entity).State = StateHelper.ConvertState(((IObjectState)entity).State);
        }
        private void SyncObjectsStatePreCommit()
        {
            foreach (var dbEntityEntry in ChangeTracker.Entries())
                dbEntityEntry.State = StateHelper.ConvertState(((IObjectState)dbEntityEntry.Entity).State);
        }

        public void SyncObjectsStatePostCommit()
        {
            foreach (var dbEntityEntry in ChangeTracker.Entries())
                ((IObjectState)dbEntityEntry.Entity).State = StateHelper.ConvertState(dbEntityEntry.State);
        }

        public void CommitTransaction()
        {
            throw new NotImplementedException();
            // Nothing to do here, 2 phase commit will handle it.
        }

        public void AbortTransaction()
        {
            throw new NotImplementedException();
            // Nothing to do here, 2 phase commit will handle it.
        }

        #endregion

        #region Dispose
        private bool _disposed;

        protected override void Dispose(bool isDisposing)
        {
            if (!_disposed)
            {
                if (isDisposing)
                {

                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }
        #endregion


    }
}
