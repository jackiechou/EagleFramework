using Eagle.Core.Common;
using Eagle.EntityFramework;
using Eagle.EntityFramework.Repositories;
using Eagle.Repositories.Business;
using Eagle.Repositories.Business.Employees;
using Eagle.Repositories.Business.Roster;
using Eagle.Repositories.Business.Vendor;
using Eagle.Repositories.Cache;
using Eagle.Repositories.Contents;
using Eagle.Repositories.Services.Booking;
using Eagle.Repositories.Services.Chat;
using Eagle.Repositories.Services.Event;
using Eagle.Repositories.Services.Messaging;
using Eagle.Repositories.Skins;
using Eagle.Repositories.SystemManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using IsolationLevel = System.Data.IsolationLevel;


namespace Eagle.Repositories
{
    public sealed class UnitOfWork : DisposableObject, IUnitOfWork
    {
        #region Initialization
        private IDbFactory _dbFactory;
        private IDataContext _dbContext;
        private Guid InstanceId { get; set; }
        private DbTransaction _transaction;
        private ObjectContext _objectContext;

        private HashSet<IDataContext> _dataContexts = new HashSet<IDataContext>();

        public UnitOfWork()
        {
            _dbFactory = CreateObject(() => new DbFactory());
            _dbContext = _dbFactory.Init();
            InstanceId = Guid.NewGuid();
        }
        public UnitOfWork(IDataContext context)
        {
            _dbContext = context;
            InstanceId = Guid.NewGuid();
        }

        public UnitOfWork(IDataContext context, IsolationLevel isolationLevel)
        {
            Contract.Requires(context != null);

            _dbContext = context;
            BeginTransaction(isolationLevel);
        }

        private int SaveChanges()
        {
            try
            {
                return _dbContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                string errorMessages = string.Join("; ", ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => $"Property: {x.PropertyName} - Error: {x.ErrorMessage}"));
                throw new DbEntityValidationException(errorMessages);
            }
        }

        public int SaveChanges(TransactionScopeOption scopeOption)
        {
            int result = 0;
            try
            {
                using (var transactionScope = new TransactionScope(scopeOption))
                {

                    foreach (var platformItem in _dataContexts)
                    {
                        IDataContext dataContext = platformItem;
                        result += dataContext.SaveChanges();
                        dataContext.CommitTransaction();
                    }
                    transactionScope.Complete();
                }
            }
            catch
            {
                foreach (IDataContext platformContext in _dataContexts)
                {
                    platformContext.AbortTransaction();
                }

                throw;
            }


            return result;
        }
        private async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        private async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        private void RegisterRepositories<TRepository>(IEnumerable<TRepository> repositories) where TRepository : IRepository
        {
            foreach (var repositoryBase in repositories.OfType<BaseRepository>())
            {
                _dataContexts.Add(repositoryBase.DataContext);
            }
        }

        void IUnitOfWork.RegisterRepositories<TRepository>(IEnumerable<IRepository> repositories)
        {
            RegisterRepositories(repositories);
        }

        Guid IUnitOfWork.InstanceId => InstanceId;

        int IUnitOfWork.SaveChanges()
        {
            return SaveChanges();
        }

        Task<int> IUnitOfWork.SaveChangesAsync()
        {
            return SaveChangesAsync();
        }

        Task<int> IUnitOfWork.SaveChangesAsync(CancellationToken cancellationToken)
        {
            return SaveChangesAsync(cancellationToken);
        }
        #endregion

        #region Unit of Work Transactions

        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            _objectContext = ((IObjectContextAdapter)_dbContext).ObjectContext;
            if (_objectContext.Connection.State != ConnectionState.Open)
            {
                _objectContext.Connection.Open();
            }

            _transaction = _objectContext.Connection.BeginTransaction(isolationLevel);
        }

        public bool Commit()
        {
            _transaction.Commit();
            return true;
        }

        public void Rollback()
        {
            _transaction.Rollback();
            _dbContext.SyncObjectsStatePostCommit();
        }

        #endregion

        #region IUnitOfWork Members
        public ICommonRepository _commonRepository;
        ICommonRepository IUnitOfWork.CommonRepository => CommonRepository;
        public ICommonRepository CommonRepository => _commonRepository ?? (_commonRepository = new CommonRepository(_dbContext));

        public ICacheManager _cacheManager;
        ICacheManager IUnitOfWork.CacheManager => CacheManager;
        public ICacheManager CacheManager => _cacheManager ?? (_cacheManager = new CacheManager());

        public IApplicationRepository _applicationRepository;
        IApplicationRepository IUnitOfWork.ApplicationRepository => ApplicationRepository;
        public IApplicationRepository ApplicationRepository => _applicationRepository ?? (_applicationRepository = new ApplicationRepository(_dbContext));


        public IApplicationSettingRepository _applicationSettingRepository;
        IApplicationSettingRepository IUnitOfWork.ApplicationSettingRepository => ApplicationSettingRepository;
        public IApplicationSettingRepository ApplicationSettingRepository => _applicationSettingRepository ?? (_applicationSettingRepository = new ApplicationSettingRepository(_dbContext));


        public IApplicationLanguageRepository _applicationLanguageRepository;
        IApplicationLanguageRepository IUnitOfWork.ApplicationLanguageRepository => ApplicationLanguageRepository;
        public IApplicationLanguageRepository ApplicationLanguageRepository => _applicationLanguageRepository ?? (_applicationLanguageRepository = new ApplicationLanguageRepository(_dbContext));


        public IAddressRepository _addressRepository;
        IAddressRepository IUnitOfWork.AddressRepository => AddressRepository;
        public IAddressRepository AddressRepository => _addressRepository ?? (_addressRepository = new AddressRepository(_dbContext));


        public ICountryRepository _countryRepository;
        ICountryRepository IUnitOfWork.CountryRepository => CountryRepository;
        public ICountryRepository CountryRepository => _countryRepository ?? (_countryRepository = new CountryRepository(_dbContext));

        public IProvinceRepository _provinceRepository;
        IProvinceRepository IUnitOfWork.ProvinceRepository => ProvinceRepository;
        public IProvinceRepository ProvinceRepository => _provinceRepository ?? (_provinceRepository = new ProvinceRepository(_dbContext));

        public IRegionRepository _regionRepository;
        IRegionRepository IUnitOfWork.RegionRepository => RegionRepository;
        public IRegionRepository RegionRepository => _regionRepository ?? (_regionRepository = new RegionRepository(_dbContext));

        public IContactRepository _contactRepository;
        IContactRepository IUnitOfWork.ContactRepository => ContactRepository;
        public IContactRepository ContactRepository => _contactRepository ?? (_contactRepository = new ContactRepository(_dbContext));


        public ICompanyRepository _companyRepository;
        ICompanyRepository IUnitOfWork.CompanyRepository => CompanyRepository;
        public ICompanyRepository CompanyRepository => _companyRepository ?? (_companyRepository = new CompanyRepository(_dbContext));


        public ILanguageRepository _languageRepository;
        ILanguageRepository IUnitOfWork.LanguageRepository => LanguageRepository;
        public ILanguageRepository LanguageRepository => _languageRepository ?? (_languageRepository = new LanguageRepository(_dbContext));


        public IUserRepository _userRepository;
        IUserRepository IUnitOfWork.UserRepository => UserRepository;
        public IUserRepository UserRepository => _userRepository ?? (_userRepository = new UserRepository(_dbContext));


        public IUserClaimRepository _userClaimRepository;
        IUserClaimRepository IUnitOfWork.UserClaimRepository => UserClaimRepository;
        public IUserClaimRepository UserClaimRepository => _userClaimRepository ?? (_userClaimRepository = new UserClaimRepository(_dbContext));


        public IUserProfileRepository _userProfileRepository;
        IUserProfileRepository IUnitOfWork.UserProfileRepository => UserProfileRepository;
        public IUserProfileRepository UserProfileRepository => _userProfileRepository ?? (_userProfileRepository = new UserProfileRepository(_dbContext));


        public IUserAddressRepository _userAddressRepository;
        IUserAddressRepository IUnitOfWork.UserAddressRepository => UserAddressRepository;
        public IUserAddressRepository UserAddressRepository => _userAddressRepository ?? (_userAddressRepository = new UserAddressRepository(_dbContext));


        public IUserRoleRepository _userRoleRepository;
        IUserRoleRepository IUnitOfWork.UserRoleRepository => UserRoleRepository;
        public IUserRoleRepository UserRoleRepository => _userRoleRepository ?? (_userRoleRepository = new UserRoleRepository(_dbContext));


        public IUserRoleGroupRepository _userRoleGroupRepository;
        IUserRoleGroupRepository IUnitOfWork.UserRoleGroupRepository => UserRoleGroupRepository;
        public IUserRoleGroupRepository UserRoleGroupRepository => _userRoleGroupRepository ?? (_userRoleGroupRepository = new UserRoleGroupRepository(_dbContext));


        public IRoleRepository _roleRepository;
        IRoleRepository IUnitOfWork.RoleRepository => RoleRepository;
        public IRoleRepository RoleRepository => _roleRepository ?? (_roleRepository = new RoleRepository(_dbContext));


        public IGroupRepository _groupRepository;
        IGroupRepository IUnitOfWork.GroupRepository => GroupRepository;
        public IGroupRepository GroupRepository => _groupRepository ?? (_groupRepository = new GroupRepository(_dbContext));


        public IRoleGroupRepository _roleGroupRepository;
        IRoleGroupRepository IUnitOfWork.RoleGroupRepository => RoleGroupRepository;
        public IRoleGroupRepository RoleGroupRepository => _roleGroupRepository ?? (_roleGroupRepository = new RoleGroupRepository(_dbContext));


        public IPageRepository _pageRepository;
        IPageRepository IUnitOfWork.PageRepository => PageRepository;
        public IPageRepository PageRepository => _pageRepository ?? (_pageRepository = new PageRepository(_dbContext));


        public IModuleRepository _moduleRepository;
        IModuleRepository IUnitOfWork.ModuleRepository => ModuleRepository;
        public IModuleRepository ModuleRepository => _moduleRepository ?? (_moduleRepository = new ModuleRepository(_dbContext));


        public IPageModuleRepository _pageModuleRepository;
        IPageModuleRepository IUnitOfWork.PageModuleRepository => PageModuleRepository;
        public IPageModuleRepository PageModuleRepository => _pageModuleRepository ?? (_pageModuleRepository = new PageModuleRepository(_dbContext));


        public IMenuRepository _menuRepository;
        IMenuRepository IUnitOfWork.MenuRepository => MenuRepository;
        public IMenuRepository MenuRepository => _menuRepository ?? (_menuRepository = new MenuRepository(_dbContext));


        public IMenuTypeRepository _menuTypeRepository;
        IMenuTypeRepository IUnitOfWork.MenuTypeRepository => MenuTypeRepository;
        public IMenuTypeRepository MenuTypeRepository => _menuTypeRepository ?? (_menuTypeRepository = new MenuTypeRepository(_dbContext));


        public IMenuPositionRepository _menuPositionRepository;
        IMenuPositionRepository IUnitOfWork.MenuPositionRepository => MenuPositionRepository;
        public IMenuPositionRepository MenuPositionRepository => _menuPositionRepository ?? (_menuPositionRepository = new MenuPositionRepository(_dbContext));


        public ISiteMapRepository _siteMapRepository;
        ISiteMapRepository IUnitOfWork.SiteMapRepository => SiteMapRepository;
        public ISiteMapRepository SiteMapRepository => _siteMapRepository ?? (_siteMapRepository = new SiteMapRepository(_dbContext));


        IPermissionRepository _permissionRepository;
        IPermissionRepository IUnitOfWork.PermissionRepository => PermissionRepository;
        public IPermissionRepository PermissionRepository => _permissionRepository ?? (_permissionRepository = new PermissionRepository(_dbContext));


        IMenuPermissionRepository _menuPermissionRepository;
        IMenuPermissionRepository IUnitOfWork.MenuPermissionRepository => MenuPermissionRepository;
        public IMenuPermissionRepository MenuPermissionRepository => _menuPermissionRepository ?? (_menuPermissionRepository = new MenuPermissionRepository(_dbContext));


        IMenuPermissionLevelRepository _menuPermissionLevelRepository;
        IMenuPermissionLevelRepository IUnitOfWork.MenuPermissionLevelRepository => MenuPermissionLevelRepository;
        public IMenuPermissionLevelRepository MenuPermissionLevelRepository => _menuPermissionLevelRepository ?? (_menuPermissionLevelRepository = new MenuPermissionLevelRepository(_dbContext));


        IModulePermissionRepository _modulePermissionRepository;
        IModulePermissionRepository IUnitOfWork.ModulePermissionRepository => ModulePermissionRepository;
        public IModulePermissionRepository ModulePermissionRepository => _modulePermissionRepository ?? (_modulePermissionRepository = new ModulePermissionRepository(_dbContext));

        IModuleCapabilityRepository _moduleCapabilityRespository;
        IModuleCapabilityRepository IUnitOfWork.ModuleCapabilityRepository => ModuleCapabilityRepository;
        public IModuleCapabilityRepository ModuleCapabilityRepository => _moduleCapabilityRespository ?? (_moduleCapabilityRespository = new ModuleCapabilityRepository(_dbContext));


        IPagePermissionRepository _pagePermissionRepository;
        IPagePermissionRepository IUnitOfWork.PagePermissionRepository => PagePermissionRepository;
        public IPagePermissionRepository PagePermissionRepository => _pagePermissionRepository ?? (_pagePermissionRepository = new PagePermissionRepository(_dbContext));


        public IContentTypeRepository _contentTypeRepository;
        IContentTypeRepository IUnitOfWork.ContentTypeRepository => ContentTypeRepository;
        public IContentTypeRepository ContentTypeRepository => _contentTypeRepository ?? (_contentTypeRepository = new ContentTypeRepository(_dbContext));

        public IContentItemRepository _contentItemRepository;
        IContentItemRepository IUnitOfWork.ContentItemRepository => ContentItemRepository;
        public IContentItemRepository ContentItemRepository => _contentItemRepository ?? (_contentItemRepository = new ContentItemRepository(_dbContext));


        public IChatPrivateMessageMasterRepository _chatPrivateMessageMasterRepository;
        IChatPrivateMessageMasterRepository IUnitOfWork.ChatPrivateMessageMasterRepository => ChatPrivateMessageMasterRepository;
        public IChatPrivateMessageMasterRepository ChatPrivateMessageMasterRepository => _chatPrivateMessageMasterRepository ?? (_chatPrivateMessageMasterRepository = new ChatPrivateMessageMasterRepository(_dbContext));


        public IChatPrivateMessageRepository _chatPrivateMessageRepository;
        IChatPrivateMessageRepository IUnitOfWork.ChatPrivateMessageRepository => ChatPrivateMessageRepository;
        public IChatPrivateMessageRepository ChatPrivateMessageRepository => _chatPrivateMessageRepository ?? (_chatPrivateMessageRepository = new ChatPrivateMessageRepository(_dbContext));


        public IChatUserRepository _chatUserRepository;
        IChatUserRepository IUnitOfWork.ChatUserRepository => ChatUserRepository;
        public IChatUserRepository ChatUserRepository => _chatUserRepository ?? (_chatUserRepository = new ChatUserRepository(_dbContext));


        public IChatMessageRepository _chatMessageRepository;
        IChatMessageRepository IUnitOfWork.ChatMessageRepository => ChatMessageRepository;
        public IChatMessageRepository ChatMessageRepository => _chatMessageRepository ?? (_chatMessageRepository = new ChatMessageRepository(_dbContext));




        public INotificationSenderRepository _notificationSenderRepository;
        INotificationSenderRepository IUnitOfWork.NotificationSenderRepository => NotificationSenderRepository;
        public INotificationSenderRepository NotificationSenderRepository => _notificationSenderRepository ?? (_notificationSenderRepository = new NotificationSenderRepository(_dbContext));


        public IMailServerProviderRepository _mailServerProviderRepository;
        IMailServerProviderRepository IUnitOfWork.MailServerProviderRepository => MailServerProviderRepository;
        public IMailServerProviderRepository MailServerProviderRepository => _mailServerProviderRepository ?? (_mailServerProviderRepository = new MailServerProviderRepository(_dbContext));


        public IMessageTypeRepository _messageTypeRepository;
        IMessageTypeRepository IUnitOfWork.MessageTypeRepository => MessageTypeRepository;
        public IMessageTypeRepository MessageTypeRepository => _messageTypeRepository ?? (_messageTypeRepository = new MessageTypeRepository(_dbContext));

        public IMessageTemplateRepository _messageTemplateRepository;
        IMessageTemplateRepository IUnitOfWork.MessageTemplateRepository => MessageTemplateRepository;
        public IMessageTemplateRepository MessageTemplateRepository => _messageTemplateRepository ?? (_messageTemplateRepository = new MessageTemplateRepository(_dbContext));


        public IMessageQueueRepository _messageQueueRepository;
        IMessageQueueRepository IUnitOfWork.MessageQueueRepository => MessageQueueRepository;
        public IMessageQueueRepository MessageQueueRepository => _messageQueueRepository ?? (_messageQueueRepository = new MessageQueueRepository(_dbContext));


        public INotificationTypeRepository _notificationTypeRepository;
        INotificationTypeRepository IUnitOfWork.NotificationTypeRepository => NotificationTypeRepository;
        public INotificationTypeRepository NotificationTypeRepository => _notificationTypeRepository ?? (_notificationTypeRepository = new NotificationTypeRepository(_dbContext));


        public INotificationTargetRepository _notificationTargetRepository;
        INotificationTargetRepository IUnitOfWork.NotificationTargetRepository => NotificationTargetRepository;
        public INotificationTargetRepository NotificationTargetRepository => _notificationTargetRepository ?? (_notificationTargetRepository = new NotificationTargetRepository(_dbContext));


        public INotificationTargetDefaultRepository _notificationTargetDefaultRepository;
        INotificationTargetDefaultRepository IUnitOfWork.NotificationTargetDefaultRepository => NotificationTargetDefaultRepository;
        public INotificationTargetDefaultRepository NotificationTargetDefaultRepository => _notificationTargetDefaultRepository ?? (_notificationTargetDefaultRepository = new NotificationTargetDefaultRepository(_dbContext));


        public INotificationMessageRepository _notificationMessageRepository;
        INotificationMessageRepository IUnitOfWork.NotificationMessageRepository => NotificationMessageRepository;
        public INotificationMessageRepository NotificationMessageRepository => _notificationMessageRepository ?? (_notificationMessageRepository = new NotificationMessageRepository(_dbContext));


        public INotificationMessageTypeRepository _notificationMessageTypeRepository;
        INotificationMessageTypeRepository IUnitOfWork.NotificationMessageTypeRepository => NotificationMessageTypeRepository;
        public INotificationMessageTypeRepository NotificationMessageTypeRepository => _notificationMessageTypeRepository ?? (_notificationMessageTypeRepository = new NotificationMessageTypeRepository(_dbContext));


        public ISkinPackageTemplateRepository _skinPackageTemplateRepository;
        ISkinPackageTemplateRepository IUnitOfWork.SkinPackageTemplateRepository => SkinPackageTemplateRepository;
        public ISkinPackageTemplateRepository SkinPackageTemplateRepository => _skinPackageTemplateRepository ?? (_skinPackageTemplateRepository = new SkinPackageTemplateRepository(_dbContext));


        public ISkinPackageBackgroundRepository _skinPackageBackgroundRepository;
        ISkinPackageBackgroundRepository IUnitOfWork.SkinPackageBackgroundRepository => SkinPackageBackgroundRepository;
        public ISkinPackageBackgroundRepository SkinPackageBackgroundRepository => _skinPackageBackgroundRepository ?? (_skinPackageBackgroundRepository = new SkinPackageBackgroundRepository(_dbContext));


        public ISkinPackageTypeRepository _skinPackageTypeRepository;
        ISkinPackageTypeRepository IUnitOfWork.SkinPackageTypeRepository => SkinPackageTypeRepository;
        public ISkinPackageTypeRepository SkinPackageTypeRepository => _skinPackageTypeRepository ?? (_skinPackageTypeRepository = new SkinPackageTypeRepository(_dbContext));


        public ISkinPackageRepository _skinPackageRepository;
        ISkinPackageRepository IUnitOfWork.SkinPackageRepository => SkinPackageRepository;
        public ISkinPackageRepository SkinPackageRepository => _skinPackageRepository ?? (_skinPackageRepository = new SkinPackageRepository(_dbContext));


        public IServicePackOptionRepository _servicePackOptionRepository;
        IServicePackOptionRepository IUnitOfWork.ServicePackOptionRepository => ServicePackOptionRepository;
        public IServicePackOptionRepository ServicePackOptionRepository => _servicePackOptionRepository ?? (_servicePackOptionRepository = new ServicePackOptionRepository(_dbContext));


        public IServicePackTypeRepository _servicePackTypeRepository;
        IServicePackTypeRepository IUnitOfWork.ServicePackTypeRepository => ServicePackTypeRepository;
        public IServicePackTypeRepository ServicePackTypeRepository => _servicePackTypeRepository ?? (_servicePackTypeRepository = new ServicePackTypeRepository(_dbContext));


        public IDocumentFolderRepository _documentFolderRespository;
        IDocumentFolderRepository IUnitOfWork.DocumentFolderRepository => DocumentFolderRepository;
        public IDocumentFolderRepository DocumentFolderRepository => _documentFolderRespository ?? (_documentFolderRespository = new DocumentFolderRepository(_dbContext));

        
        public IDocumentationRepository _documentationRepository;
        IDocumentationRepository IUnitOfWork.DocumentationRepository => DocumentationRepository;
        public IDocumentationRepository DocumentationRepository => _documentationRepository ?? (_documentationRepository = new DocumentationRepository(_dbContext));

        
        public IDocumentFileRepository _documentFileRepository;
        IDocumentFileRepository IUnitOfWork.DocumentFileRepository => DocumentFileRepository;
        public IDocumentFileRepository DocumentFileRepository => _documentFileRepository ?? (_documentFileRepository = new DocumentFileRepository(_dbContext));


        public IDocumentPermissionRepository _documentPermissionRepository;
        IDocumentPermissionRepository IUnitOfWork.DocumentPermissionRepository => DocumentPermissionRepository;
        public IDocumentPermissionRepository DocumentPermissionRepository => _documentPermissionRepository ?? (_documentPermissionRepository = new DocumentPermissionRepository());


        public INewsRepository _newsRepository;
        INewsRepository IUnitOfWork.NewsRepository => NewsRepository;
        public INewsRepository NewsRepository => _newsRepository ?? (_newsRepository = new NewsRepository(_dbContext));

        public INewsCategoryRepository _newsCategoryRepository;
        INewsCategoryRepository IUnitOfWork.NewsCategoryRepository => NewsCategoryRepository;
        public INewsCategoryRepository NewsCategoryRepository => _newsCategoryRepository ?? (_newsCategoryRepository = new NewsCategoryRepository(_dbContext));

        public INewsCommentRepository _newsCommentRepository;
        INewsCommentRepository IUnitOfWork.NewsCommentRepository => NewsCommentRepository;
        public INewsCommentRepository NewsCommentRepository => _newsCommentRepository ?? (_newsCommentRepository = new NewsCommentRepository(_dbContext));


        public INewsRatingRepository _newsRatingRepository;
        INewsRatingRepository IUnitOfWork.NewsRatingRepository => NewsRatingRepository;
        public INewsRatingRepository NewsRatingRepository => _newsRatingRepository ?? (_newsRatingRepository = new NewsRatingRepository(_dbContext));


        public IEventRepository _eventRepository;
        IEventRepository IUnitOfWork.EventRepository => EventRepository;
        public IEventRepository EventRepository => _eventRepository ?? (_eventRepository = new EventRepository(_dbContext));

        public IEventTypeRepository _eventTypeRepository;
        IEventTypeRepository IUnitOfWork.EventTypeRepository => EventTypeRepository;
        public IEventTypeRepository EventTypeRepository => _eventTypeRepository ?? (_eventTypeRepository = new EventTypeRepository(_dbContext));


        public IFeedbackRepository _feedbackRepository;
        IFeedbackRepository IUnitOfWork.FeedbackRepository => FeedbackRepository;
        public IFeedbackRepository FeedbackRepository => _feedbackRepository ?? (_feedbackRepository = new FeedbackRepository(_dbContext));


        public ILogRepository _logRepository;
        ILogRepository IUnitOfWork.LogRepository => LogRepository;
        public ILogRepository LogRepository => _logRepository ?? (_logRepository = new LogRepository(_dbContext));

        public ITagRepository _tagRepository;
        ITagRepository IUnitOfWork.TagRepository => TagRepository;
        public ITagRepository TagRepository => _tagRepository ?? (_tagRepository = new TagRepository(_dbContext));


        public ITagIntegrationRepository _tagIntegrationRepository;
        ITagIntegrationRepository IUnitOfWork.TagIntegrationRepository => TagIntegrationRepository;
        public ITagIntegrationRepository TagIntegrationRepository => _tagIntegrationRepository ?? (_tagIntegrationRepository = new TagIntegrationRepository(_dbContext));


        public ITransactionMethodRepository _transactionMethodRepository;
        ITransactionMethodRepository IUnitOfWork.TransactionMethodRepository => TransactionMethodRepository;
        public ITransactionMethodRepository TransactionMethodRepository => _transactionMethodRepository ?? (_transactionMethodRepository = new TransactionMethodRepository(_dbContext));


        public IBannerRepository _bannerRepository;
        IBannerRepository IUnitOfWork.BannerRepository => BannerRepository;
        public IBannerRepository BannerRepository => _bannerRepository ?? (_bannerRepository = new BannerRepository(_dbContext));


        public IBannerPositionRepository _bannerPositionRepository;
        IBannerPositionRepository IUnitOfWork.BannerPositionRepository => BannerPositionRepository;
        public IBannerPositionRepository BannerPositionRepository => _bannerPositionRepository ?? (_bannerPositionRepository = new BannerPositionRepository(_dbContext));

        public IBannerTypeRepository _bannerTypeRepository;
        IBannerTypeRepository IUnitOfWork.BannerTypeRepository => BannerTypeRepository;
        public IBannerTypeRepository BannerTypeRepository => _bannerTypeRepository ?? (_bannerTypeRepository = new BannerTypeRepository(_dbContext));

        public IBannerScopeRepository _bannerScopeRepository;
        IBannerScopeRepository IUnitOfWork.BannerScopeRepository => BannerScopeRepository;
        public IBannerScopeRepository BannerScopeRepository => _bannerScopeRepository ?? (_bannerScopeRepository = new BannerScopeRepository(_dbContext));


        public IBannerPageRepository _bannerPageRepository;
        IBannerPageRepository IUnitOfWork.BannerPageRepository => BannerPageRepository;
        public IBannerPageRepository BannerPageRepository => _bannerPageRepository ?? (_bannerPageRepository = new BannerPageRepository(_dbContext));

        public IBannerZoneRepository _bannerZoneRepository;
        IBannerZoneRepository IUnitOfWork.BannerZoneRepository => BannerZoneRepository;
        public IBannerZoneRepository BannerZoneRepository => _bannerZoneRepository ?? (_bannerZoneRepository = new BannerZoneRepository(_dbContext));


        public IGalleryCollectionRepository _galleryCollectionRepository;
        IGalleryCollectionRepository IUnitOfWork.GalleryCollectionRepository => GalleryCollectionRepository;
        public IGalleryCollectionRepository GalleryCollectionRepository => _galleryCollectionRepository ?? (_galleryCollectionRepository = new GalleryCollectionRepository(_dbContext));


        public IGalleryTopicRepository _galleryTopicRepository;
        IGalleryTopicRepository IUnitOfWork.GalleryTopicRepository => GalleryTopicRepository;
        public IGalleryTopicRepository GalleryTopicRepository => _galleryTopicRepository ?? (_galleryTopicRepository = new GalleryTopicRepository(_dbContext));


        public IGalleryFileRepository _galleryFileRepository;
        IGalleryFileRepository IUnitOfWork.GalleryFileRepository => GalleryFileRepository;
        public IGalleryFileRepository GalleryFileRepository => _galleryFileRepository ?? (_galleryFileRepository = new GalleryFileRepository(_dbContext));


        public IMediaAlbumRepository _mediaAlbumRepository;
        IMediaAlbumRepository IUnitOfWork.MediaAlbumRepository => MediaAlbumRepository;
        public IMediaAlbumRepository MediaAlbumRepository => _mediaAlbumRepository ?? (_mediaAlbumRepository = new MediaAlbumRepository(_dbContext));

        public IMediaArtistRepository _mediaArtistRepository;
        IMediaArtistRepository IUnitOfWork.MediaArtistRepository => MediaArtistRepository;
        public IMediaArtistRepository MediaArtistRepository => _mediaArtistRepository ?? (_mediaArtistRepository = new MediaArtistRepository(_dbContext));


        public IMediaComposerRepository _mediaComposerRepository;
        IMediaComposerRepository IUnitOfWork.MediaComposerRepository => MediaComposerRepository;
        public IMediaComposerRepository MediaComposerRepository => _mediaComposerRepository ?? (_mediaComposerRepository = new MediaComposerRepository(_dbContext));


        public IMediaFileRepository _mediaFileRepository;
        IMediaFileRepository IUnitOfWork.MediaFileRepository => MediaFileRepository;
        public IMediaFileRepository MediaFileRepository => _mediaFileRepository ?? (_mediaFileRepository = new MediaFileRepository(_dbContext));


        public IMediaPlayListRepository _mediaPlayListRepository;
        IMediaPlayListRepository IUnitOfWork.MediaPlayListRepository => MediaPlayListRepository;
        public IMediaPlayListRepository MediaPlayListRepository => _mediaPlayListRepository ?? (_mediaPlayListRepository = new MediaPlayListRepository(_dbContext));


        public IMediaTopicRepository _mediaTopicRepository;
        IMediaTopicRepository IUnitOfWork.MediaTopicRepository => MediaTopicRepository;
        public IMediaTopicRepository MediaTopicRepository => _mediaTopicRepository ?? (_mediaTopicRepository = new MediaTopicRepository(_dbContext));


        public IMediaTypeRepository _mediaTypeRepository;
        IMediaTypeRepository IUnitOfWork.MediaTypeRepository => MediaTypeRepository;
        public IMediaTypeRepository MediaTypeRepository => _mediaTypeRepository ?? (_mediaTypeRepository = new MediaTypeRepository(_dbContext));


        public IMediaAlbumFileRepository _mediaAlbumFileRepository;
        IMediaAlbumFileRepository IUnitOfWork.MediaAlbumFileRepository => MediaAlbumFileRepository;
        public IMediaAlbumFileRepository MediaAlbumFileRepository => _mediaAlbumFileRepository ?? (_mediaAlbumFileRepository = new MediaAlbumFileRepository(_dbContext));


        public IMediaPlayListFileRepository _mediaPlayListFileRepository;
        IMediaPlayListFileRepository IUnitOfWork.MediaPlayListFileRepository => MediaPlayListFileRepository;
        public IMediaPlayListFileRepository MediaPlayListFileRepository => _mediaPlayListFileRepository ?? (_mediaPlayListFileRepository = new MediaPlayListFileRepository(_dbContext));


        public ICurrencyRepository _currencyRepository;
        ICurrencyRepository IUnitOfWork.CurrencyRepository => CurrencyRepository;
        public ICurrencyRepository CurrencyRepository => _currencyRepository ?? (_currencyRepository = new CurrencyRepository(_dbContext));


        public ICurrencyRateRepository _currencyRateRepository;
        ICurrencyRateRepository IUnitOfWork.CurrencyRateRepository => CurrencyRateRepository;
        public ICurrencyRateRepository CurrencyRateRepository => _currencyRateRepository ?? (_currencyRateRepository = new CurrencyRateRepository(_dbContext));


        public ICustomerRepository _customerRepository;
        ICustomerRepository IUnitOfWork.CustomerRepository => CustomerRepository;
        public ICustomerRepository CustomerRepository => _customerRepository ?? (_customerRepository = new CustomerRepository(_dbContext));

        public ICustomerTypeRepository _customerTypeRepository;
        ICustomerTypeRepository IUnitOfWork.CustomerTypeRepository => CustomerTypeRepository;
        public ICustomerTypeRepository CustomerTypeRepository => _customerTypeRepository ?? (_customerTypeRepository = new CustomerTypeRepository(_dbContext));

        public IProductRepository _productRepository;
        IProductRepository IUnitOfWork.ProductRepository => ProductRepository;
        public IProductRepository ProductRepository => _productRepository ?? (_productRepository = new ProductRepository(_dbContext));


        public IProductAlbumRepository _productAlbumRepository;
        IProductAlbumRepository IUnitOfWork.ProductAlbumRepository => ProductAlbumRepository;
        public IProductAlbumRepository ProductAlbumRepository => _productAlbumRepository ?? (_productAlbumRepository = new ProductAlbumRepository(_dbContext));


        public IProductCategoryRepository _productCategoryRepository;
        IProductCategoryRepository IUnitOfWork.ProductCategoryRepository => ProductCategoryRepository;
        public IProductCategoryRepository ProductCategoryRepository => _productCategoryRepository ?? (_productCategoryRepository = new ProductCategoryRepository(_dbContext));

        public IProductTypeRepository _productTypeRepository;
        IProductTypeRepository IUnitOfWork.ProductTypeRepository => ProductTypeRepository;
        public IProductTypeRepository ProductTypeRepository => _productTypeRepository ?? (_productTypeRepository = new ProductTypeRepository(_dbContext));


        public IProductTaxRateRepository _productTaxRateRepository;
        IProductTaxRateRepository IUnitOfWork.ProductTaxRateRepository => ProductTaxRateRepository;
        public IProductTaxRateRepository ProductTaxRateRepository => _productTaxRateRepository ?? (_productTaxRateRepository = new ProductTaxRateRepository(_dbContext));


        public IProductAttributeRepository _productAttributeRepository;
        IProductAttributeRepository IUnitOfWork.ProductAttributeRepository => ProductAttributeRepository;
        public IProductAttributeRepository ProductAttributeRepository => _productAttributeRepository ?? (_productAttributeRepository = new ProductAttributeRepository(_dbContext));


        public IProductAttributeOptionRepository _productAttributeOptionRepository;
        IProductAttributeOptionRepository IUnitOfWork.ProductAttributeOptionRepository => ProductAttributeOptionRepository;
        public IProductAttributeOptionRepository ProductAttributeOptionRepository => _productAttributeOptionRepository ?? (_productAttributeOptionRepository = new ProductAttributeOptionRepository(_dbContext));


        public IProductDiscountRepository _productDiscountRepository;
        IProductDiscountRepository IUnitOfWork.ProductDiscountRepository => ProductDiscountRepository;
        public IProductDiscountRepository ProductDiscountRepository => _productDiscountRepository ?? (_productDiscountRepository = new ProductDiscountRepository(_dbContext));

        public IProductFileRepository _productFileRepository;
        IProductFileRepository IUnitOfWork.ProductFileRepository => ProductFileRepository;
        public IProductFileRepository ProductFileRepository => _productFileRepository ?? (_productFileRepository = new ProductFileRepository(_dbContext));


        public IProductCommentRepository _productCommentRepository;
        IProductCommentRepository IUnitOfWork.ProductCommentRepository => ProductCommentRepository;
        public IProductCommentRepository ProductCommentRepository => _productCommentRepository ?? (_productCommentRepository = new ProductCommentRepository(_dbContext));


        public IProductRatingRepository _productRatingRepository;
        IProductRatingRepository IUnitOfWork.ProductRatingRepository => ProductRatingRepository;
        public IProductRatingRepository ProductRatingRepository => _productRatingRepository ?? (_productRatingRepository = new ProductRatingRepository(_dbContext));


        public IOrderRepository _orderRepository;
        IOrderRepository IUnitOfWork.OrderRepository => OrderRepository;
        public IOrderRepository OrderRepository => _orderRepository ?? (_orderRepository = new OrderRepository(_dbContext));


        public IOrderTempRepository _orderTempRepository;
        IOrderTempRepository IUnitOfWork.OrderTempRepository => OrderTempRepository;
        public IOrderTempRepository OrderTempRepository => _orderTempRepository ?? (_orderTempRepository = new OrderTempRepository(_dbContext));


        public IOrderProductRepository _orderProductRepository;
        IOrderProductRepository IUnitOfWork.OrderProductRepository => OrderProductRepository;
        public IOrderProductRepository OrderProductRepository => _orderProductRepository ?? (_orderProductRepository = new OrderProductRepository(_dbContext));


        public IOrderProductTempRepository _orderProductTempRepository;
        IOrderProductTempRepository IUnitOfWork.OrderProductTempRepository => OrderProductTempRepository;
        public IOrderProductTempRepository OrderProductTempRepository => _orderProductTempRepository ?? (_orderProductTempRepository = new OrderProductTempRepository(_dbContext));


        public IOrderProductOptionRepository _orderProductOptionRepository;
        IOrderProductOptionRepository IUnitOfWork.OrderProductOptionRepository => OrderProductOptionRepository;
        public IOrderProductOptionRepository OrderProductOptionRepository => _orderProductOptionRepository ?? (_orderProductOptionRepository = new OrderProductOptionRepository(_dbContext));

        public IOrderPaymentRepository _orderPaymentRepository;
        IOrderPaymentRepository IUnitOfWork.OrderPaymentRepository => OrderPaymentRepository;
        public IOrderPaymentRepository OrderPaymentRepository => _orderPaymentRepository ?? (_orderPaymentRepository = new OrderPaymentRepository(_dbContext));


        public IOrderShipmentRepository _orderShipmentRepository;
        IOrderShipmentRepository IUnitOfWork.OrderShipmentRepository => OrderShipmentRepository;
        public IOrderShipmentRepository OrderShipmentRepository => _orderShipmentRepository ?? (_orderShipmentRepository = new OrderShipmentRepository(_dbContext));


        public IPaymentMethodRepository _paymentMethodRepository;
        IPaymentMethodRepository IUnitOfWork.PaymentMethodRepository => PaymentMethodRepository;
        public IPaymentMethodRepository PaymentMethodRepository => _paymentMethodRepository ?? (_paymentMethodRepository = new PaymentMethodRepository(_dbContext));


        public IManufacturerCategoryRepository _manufacturerCategoryRepository;
        IManufacturerCategoryRepository IUnitOfWork.ManufacturerCategoryRepository => ManufacturerCategoryRepository;
        public IManufacturerCategoryRepository ManufacturerCategoryRepository => _manufacturerCategoryRepository ?? (_manufacturerCategoryRepository = new ManufacturerCategoryRepository(_dbContext));


        public IManufacturerRepository _manufacturerRepository;
        IManufacturerRepository IUnitOfWork.ManufacturerRepository => ManufacturerRepository;
        public IManufacturerRepository ManufacturerRepository => _manufacturerRepository ?? (_manufacturerRepository = new ManufacturerRepository(_dbContext));


        public IVendorRepository _vendorRepository;
        IVendorRepository IUnitOfWork.VendorRepository => VendorRepository;
        public IVendorRepository VendorRepository => _vendorRepository ?? (_vendorRepository = new VendorRepository(_dbContext));


        public IVendorCurrencyRepository _vendorCurrencyRepository;
        IVendorCurrencyRepository IUnitOfWork.VendorCurrencyRepository => VendorCurrencyRepository;
        public IVendorCurrencyRepository VendorCurrencyRepository => _vendorCurrencyRepository ?? (_vendorCurrencyRepository = new VendorCurrencyRepository(_dbContext));


        public IVendorAddressRepository _vendorAddressRepository;
        IVendorAddressRepository IUnitOfWork.VendorAddressRepository => VendorAddressRepository;
        public IVendorAddressRepository VendorAddressRepository => _vendorAddressRepository ?? (_vendorAddressRepository = new VendorAddressRepository(_dbContext));


        public IVendorPartnerRepository _vendorPartnerRepository;
        IVendorPartnerRepository IUnitOfWork.VendorPartnerRepository => VendorPartnerRepository;
        public IVendorPartnerRepository VendorPartnerRepository => _vendorPartnerRepository ?? (_vendorPartnerRepository = new VendorPartnerRepository(_dbContext));


        public IVendorCreditCardRepository _vendorCreditCardRepository;
        IVendorCreditCardRepository IUnitOfWork.VendorCreditCardRepository => VendorCreditCardRepository;
        public IVendorCreditCardRepository VendorCreditCardRepository => _vendorCreditCardRepository ?? (_vendorCreditCardRepository = new VendorCreditCardRepository(_dbContext));


        public IVendorShippingCarrierRepository _vendorShippingCarrierRepository;
        IVendorShippingCarrierRepository IUnitOfWork.VendorShippingCarrierRepository => VendorShippingCarrierRepository;
        public IVendorShippingCarrierRepository VendorShippingCarrierRepository => _vendorShippingCarrierRepository ?? (_vendorShippingCarrierRepository = new VendorShippingCarrierRepository(_dbContext));


        public IVendorShippingMethodRepository _vendorShippingMethodRepository;
        IVendorShippingMethodRepository IUnitOfWork.VendorShippingMethodRepository => VendorShippingMethodRepository;
        public IVendorShippingMethodRepository VendorShippingMethodRepository => _vendorShippingMethodRepository ?? (_vendorShippingMethodRepository = new VendorShippingMethodRepository(_dbContext));


        public IVendorPaymentMethodRepository _vendorPaymentMethodRepository;
        IVendorPaymentMethodRepository IUnitOfWork.VendorPaymentMethodRepository => VendorPaymentMethodRepository;
        public IVendorPaymentMethodRepository VendorPaymentMethodRepository => _vendorPaymentMethodRepository ?? (_vendorPaymentMethodRepository = new VendorPaymentMethodRepository(_dbContext));


        public IVendorTransactionMethodRepository _vendorTransactionMethodRepository;
        IVendorTransactionMethodRepository IUnitOfWork.VendorTransactionMethodRepository => VendorTransactionMethodRepository;
        public IVendorTransactionMethodRepository VendorTransactionMethodRepository => _vendorTransactionMethodRepository ?? (_vendorTransactionMethodRepository = new VendorTransactionMethodRepository(_dbContext));


        public IUserVendorRepository _userVendorRepository;
        IUserVendorRepository IUnitOfWork.UserVendorRepository => UserVendorRepository;
        public IUserVendorRepository UserVendorRepository => _userVendorRepository ?? (_userVendorRepository = new UserVendorRepository(_dbContext));

        public IPromotionRepository _promotionRepository;
        IPromotionRepository IUnitOfWork.PromotionRepository => PromotionRepository;
        public IPromotionRepository PromotionRepository => _promotionRepository ?? (_promotionRepository = new PromotionRepository(_dbContext));


        public IShippingCarrierRepository _shippingCarrierRepository;
        IShippingCarrierRepository IUnitOfWork.ShippingCarrierRepository => ShippingCarrierRepository;
        public IShippingCarrierRepository ShippingCarrierRepository => _shippingCarrierRepository ?? (_shippingCarrierRepository = new ShippingCarrierRepository(_dbContext));


        public IShippingMethodRepository _shippingMethodRepository;
        IShippingMethodRepository IUnitOfWork.ShippingMethodRepository => ShippingMethodRepository;
        public IShippingMethodRepository ShippingMethodRepository => _shippingMethodRepository ?? (_shippingMethodRepository = new ShippingMethodRepository(_dbContext));


        public IShippingFeeRepository _shippingFeeRepository;
        IShippingFeeRepository IUnitOfWork.ShippingFeeRepository => ShippingFeeRepository;
        public IShippingFeeRepository ShippingFeeRepository => _shippingFeeRepository ?? (_shippingFeeRepository = new ShippingFeeRepository(_dbContext));

        public IServiceCategoryRepository _serviceCategoryRepository;
        IServiceCategoryRepository IUnitOfWork.ServiceCategoryRepository => ServiceCategoryRepository;
        public IServiceCategoryRepository ServiceCategoryRepository => _serviceCategoryRepository ?? (_serviceCategoryRepository = new ServiceCategoryRepository(_dbContext));


        public IServiceDiscountRepository _serviceDiscountRepository;
        IServiceDiscountRepository IUnitOfWork.ServiceDiscountRepository => ServiceDiscountRepository;
        public IServiceDiscountRepository ServiceDiscountRepository => _serviceDiscountRepository ?? (_serviceDiscountRepository = new ServiceDiscountRepository(_dbContext));


        public IServicePackRepository _servicePackRepository;
        IServicePackRepository IUnitOfWork.ServicePackRepository => ServicePackRepository;
        public IServicePackRepository ServicePackRepository => _servicePackRepository ?? (_servicePackRepository = new ServicePackRepository(_dbContext));


        public IServicePackDurationRepository _servicePackDurationRepository;
        IServicePackDurationRepository IUnitOfWork.ServicePackDurationRepository => ServicePackDurationRepository;
        public IServicePackDurationRepository ServicePackDurationRepository => _servicePackDurationRepository ?? (_servicePackDurationRepository = new ServicePackDurationRepository(_dbContext));


        public IServicePackProviderRepository _servicePackProviderRepository;
        IServicePackProviderRepository IUnitOfWork.ServicePackProviderRepository => ServicePackProviderRepository;
        public IServicePackProviderRepository ServicePackProviderRepository => _servicePackProviderRepository ?? (_servicePackProviderRepository = new ServicePackProviderRepository(_dbContext));


        public IServicePackRatingRepository _servicePackRatingRepository;
        IServicePackRatingRepository IUnitOfWork.ServicePackRatingRepository => ServicePackRatingRepository;
        public IServicePackRatingRepository ServicePackRatingRepository => _servicePackRatingRepository ?? (_servicePackRatingRepository = new ServicePackRatingRepository(_dbContext));


        public IServicePeriodRepository _servicePeriodRepository;
        IServicePeriodRepository IUnitOfWork.ServicePeriodRepository => ServicePeriodRepository;
        public IServicePeriodRepository ServicePeriodRepository => _servicePeriodRepository ?? (_servicePeriodRepository = new ServicePeriodRepository(_dbContext));


        public IServiceTaxRateRepository _serviceTaxRateRepository;
        IServiceTaxRateRepository IUnitOfWork.ServiceTaxRateRepository => ServiceTaxRateRepository;
        public IServiceTaxRateRepository ServiceTaxRateRepository => _serviceTaxRateRepository ?? (_serviceTaxRateRepository = new ServiceTaxRateRepository(_dbContext));


        public IContractRepository _contractRepository;
        IContractRepository IUnitOfWork.ContractRepository => ContractRepository;
        public IContractRepository ContractRepository => _contractRepository ?? (_contractRepository = new ContractRepository(_dbContext));


        public IEmployeeRepository _employeeRepository;
        IEmployeeRepository IUnitOfWork.EmployeeRepository => EmployeeRepository;
        public IEmployeeRepository EmployeeRepository => _employeeRepository ?? (_employeeRepository = new EmployeeRepository(_dbContext));


        public IEmployeeAvailabilityRepository _employeeAvailabilityRepository;
        IEmployeeAvailabilityRepository IUnitOfWork.EmployeeAvailabilityRepository => EmployeeAvailabilityRepository;
        public IEmployeeAvailabilityRepository EmployeeAvailabilityRepository => _employeeAvailabilityRepository ?? (_employeeAvailabilityRepository = new EmployeeAvailabilityRepository(_dbContext));


        public IEmployeePositionRepository _employeePositionRepository;
        IEmployeePositionRepository IUnitOfWork.EmployeePositionRepository => EmployeePositionRepository;
        public IEmployeePositionRepository EmployeePositionRepository => _employeePositionRepository ?? (_employeePositionRepository = new EmployeePositionRepository(_dbContext));


        public IEmployeeSkillRepository _employeeSkillRepository;
        IEmployeeSkillRepository IUnitOfWork.EmployeeSkillRepository => EmployeeSkillRepository;
        public IEmployeeSkillRepository EmployeeSkillRepository => _employeeSkillRepository ?? (_employeeSkillRepository = new EmployeeSkillRepository(_dbContext));


        public IEmployeeTimeOffRepository _employeeTimeOffRepository;
        IEmployeeTimeOffRepository IUnitOfWork.EmployeeTimeOffRepository => EmployeeTimeOffRepository;
        public IEmployeeTimeOffRepository EmployeeTimeOffRepository => _employeeTimeOffRepository ?? (_employeeTimeOffRepository = new EmployeeTimeOffRepository(_dbContext));


        public IJobPositionRepository _jobPositionRepository;
        IJobPositionRepository IUnitOfWork.JobPositionRepository => JobPositionRepository;
        public IJobPositionRepository JobPositionRepository => _jobPositionRepository ?? (_jobPositionRepository = new JobPositionRepository(_dbContext));


        public IJobPositionSkillRepository _jobPositionSkillRepository;
        IJobPositionSkillRepository IUnitOfWork.JobPositionSkillRepository => JobPositionSkillRepository;
        public IJobPositionSkillRepository JobPositionSkillRepository => _jobPositionSkillRepository ?? (_jobPositionSkillRepository = new JobPositionSkillRepository(_dbContext));


        public IQualificationRepository _qualificationRepository;
        IQualificationRepository IUnitOfWork.QualificationRepository => QualificationRepository;
        public IQualificationRepository QualificationRepository => _qualificationRepository ?? (_qualificationRepository = new QualificationRepository(_dbContext));


        public IRewardDisciplineRepository _rewardDisciplineRepository;
        IRewardDisciplineRepository IUnitOfWork.RewardDisciplineRepository => RewardDisciplineRepository;
        public IRewardDisciplineRepository RewardDisciplineRepository => _rewardDisciplineRepository ?? (_rewardDisciplineRepository = new RewardDisciplineRepository(_dbContext));


        public ISalaryRepository _salaryRepository;
        ISalaryRepository IUnitOfWork.SalaryRepository => SalaryRepository;
        public ISalaryRepository SalaryRepository => _salaryRepository ?? (_salaryRepository = new SalaryRepository(_dbContext));


        public ISkillRepository _skillRepository;
        ISkillRepository IUnitOfWork.SkillRepository => SkillRepository;
        public ISkillRepository SkillRepository => _skillRepository ?? (_skillRepository = new SkillRepository(_dbContext));


        public ITerminationRepository _terminationRepository;
        ITerminationRepository IUnitOfWork.TerminationRepository => TerminationRepository;
        public ITerminationRepository TerminationRepository => _terminationRepository ?? (_terminationRepository = new TerminationRepository(_dbContext));


        public IWorkingHistoryRepository _workingHistoryRepository;
        IWorkingHistoryRepository IUnitOfWork.WorkingHistoryRepository => WorkingHistoryRepository;
        public IWorkingHistoryRepository WorkingHistoryRepository => _workingHistoryRepository ?? (_workingHistoryRepository = new WorkingHistoryRepository(_dbContext));


        public IPublicHolidaySetRepository _publicHolidaySetRepository;
        IPublicHolidaySetRepository IUnitOfWork.PublicHolidaySetRepository => PublicHolidaySetRepository;
        public IPublicHolidaySetRepository PublicHolidaySetRepository => _publicHolidaySetRepository ?? (_publicHolidaySetRepository = new PublicHolidaySetRepository(_dbContext));

        public IPublicHolidayRepository _publicHolidayRepository;
        IPublicHolidayRepository IUnitOfWork.PublicHolidayRepository => PublicHolidayRepository;
        public IPublicHolidayRepository PublicHolidayRepository => _publicHolidayRepository ?? (_publicHolidayRepository = new PublicHolidayRepository(_dbContext));


        public IShiftRepository _shiftRepository;
        IShiftRepository IUnitOfWork.ShiftRepository => ShiftRepository;
        public IShiftRepository ShiftRepository => _shiftRepository ?? (_shiftRepository = new ShiftRepository(_dbContext));


        public IShiftPositionRepository _shiftPositionRepository;
        IShiftPositionRepository IUnitOfWork.ShiftPositionRepository => ShiftPositionRepository;
        public IShiftPositionRepository ShiftPositionRepository => _shiftPositionRepository ?? (_shiftPositionRepository = new ShiftPositionRepository(_dbContext));


        public IShiftSwapRepository _shiftSwapRepository;
        IShiftSwapRepository IUnitOfWork.ShiftSwapRepository => ShiftSwapRepository;
        public IShiftSwapRepository ShiftSwapRepository => _shiftSwapRepository ?? (_shiftSwapRepository = new ShiftSwapRepository(_dbContext));


        public IShiftTypeRepository _shiftTypeRepository;
        IShiftTypeRepository IUnitOfWork.ShiftTypeRepository => ShiftTypeRepository;
        public IShiftTypeRepository ShiftTypeRepository => _shiftTypeRepository ?? (_shiftTypeRepository = new ShiftTypeRepository(_dbContext));


        public ITimeOffTypeRepository _timeOffTypeRepository;
        ITimeOffTypeRepository IUnitOfWork.TimeOffTypeRepository => TimeOffTypeRepository;
        public ITimeOffTypeRepository TimeOffTypeRepository => _timeOffTypeRepository ?? (_timeOffTypeRepository = new TimeOffTypeRepository(_dbContext));


        public ITimesheetRepository _timesheetRepository;
        ITimesheetRepository IUnitOfWork.TimesheetRepository => TimesheetRepository;
        public ITimesheetRepository TimesheetRepository => _timesheetRepository ?? (_timesheetRepository = new TimesheetRepository(_dbContext));

        public IBrandRepository _brandRepository;
        IBrandRepository IUnitOfWork.BrandRepository => BrandRepository;
        public IBrandRepository BrandRepository => _brandRepository ?? (_brandRepository = new BrandRepository(_dbContext));
        public IAttributeRepository _attributeRepository;
        public IAttributeRepository AttributeRepository => _attributeRepository ?? (_attributeRepository = new AttributeRepository(_dbContext));
        public IAttributeOptionRepository _attributeOptionRepository;
        public IAttributeOptionRepository AttributeOptionRepository => _attributeOptionRepository ?? (_attributeOptionRepository = new AttributeOptionRepository(_dbContext));


        #endregion

        private bool _disposed;

        protected override void Dispose(bool isDisposing)
        {
            if (!_disposed)
            {
                if (isDisposing)
                {
                    _dataContexts = null;
                    _dbFactory = null;
                    _dbContext = null;
                    _commonRepository = null;


                    _applicationRepository = null;
                    _applicationSettingRepository = null;
                    _applicationLanguageRepository = null;

                    _addressRepository = null;
                    _countryRepository = null;
                    _provinceRepository = null;
                    _regionRepository = null;

                    _companyRepository = null;
                    _contactRepository = null;
                    _languageRepository = null;


                    _roleRepository = null;
                    _groupRepository = null;

                    _userRepository = null;
                    _userClaimRepository = null;
                    _roleGroupRepository = null;
                    _userRoleRepository = null;
                    _userRoleGroupRepository = null;
                    _userAddressRepository = null;
                    _userProfileRepository = null;


                    _pageRepository = null;
                    _moduleRepository = null;
                    _pageModuleRepository = null;


                    _permissionRepository = null;
                    _menuRepository = null;
                    _menuPositionRepository = null;
                    _menuTypeRepository = null;
                    _menuPermissionLevelRepository = null;

                    _modulePermissionRepository = null;
                    _moduleCapabilityRespository = null;
                    _pagePermissionRepository = null;

                    _contentTypeRepository = null;
                    _contentItemRepository = null;

                    _chatPrivateMessageMasterRepository = null;
                    _chatPrivateMessageRepository = null;
                    _chatUserRepository = null;
                    _chatMessageRepository = null;

                    _notificationSenderRepository = null;
                    _mailServerProviderRepository = null;

                    _notificationTypeRepository = null;
                    _messageTemplateRepository = null;
                    _messageQueueRepository = null;
                    _messageTypeRepository = null;

                    _skinPackageTemplateRepository = null;
                    _skinPackageBackgroundRepository = null;
                    _skinPackageRepository = null;

                    _documentationRepository = null;
                    _documentFolderRespository = null;
                    _documentFileRepository = null;
                    _documentPermissionRepository = null;

                    _newsRepository = null;
                    _newsCategoryRepository = null;
                    _newsCommentRepository = null;
                    _newsRatingRepository = null;

                    _logRepository = null;

                    _tagRepository = null;
                    _tagIntegrationRepository = null;
                    _transactionMethodRepository = null;

                    _bannerRepository = null;
                    _bannerPositionRepository = null;
                    _bannerTypeRepository = null;
                    _bannerScopeRepository = null;
                    _bannerPageRepository = null;

                    _galleryCollectionRepository = null;
                    _galleryTopicRepository = null;
                    _galleryFileRepository = null;

                    _mediaAlbumRepository = null;
                    _mediaArtistRepository = null;
                    _mediaComposerRepository = null;
                    _mediaFileRepository = null;
                    _mediaPlayListRepository = null;
                    _mediaTopicRepository = null;
                    _mediaTypeRepository = null;
                    _mediaAlbumFileRepository = null;
                    _mediaPlayListFileRepository = null;

                    _currencyRepository = null;
                    _currencyRateRepository = null;

                    _customerRepository = null;
                    _customerTypeRepository = null;

                    _productRepository = null;
                    _productCategoryRepository = null;
                    _productTypeRepository = null;
                    _productTaxRateRepository = null;
                    _productAttributeRepository = null;
                    _productAttributeOptionRepository = null;
                    _productDiscountRepository = null;
                    _productFileRepository = null;
                    _productCommentRepository = null;
                    _productRatingRepository = null;

                    _orderRepository = null;
                    _orderProductRepository = null;
                    _orderProductOptionRepository = null;
                    _orderPaymentRepository = null;
                    _orderShipmentRepository = null;

                    _paymentMethodRepository = null;

                    _manufacturerCategoryRepository = null;
                    _manufacturerRepository = null;

                    _vendorRepository = null;
                    _vendorCurrencyRepository = null;
                    _vendorAddressRepository = null;
                    _vendorCreditCardRepository = null;
                    _vendorPartnerRepository = null;
                    _userVendorRepository = null;

                    _promotionRepository = null;

                    _shippingCarrierRepository = null;
                    _shippingMethodRepository = null;
                    _shippingFeeRepository = null;

                    _serviceDiscountRepository = null;
                    _servicePackRepository = null;
                    _servicePackDurationRepository = null;
                    _servicePackProviderRepository = null;
                    _servicePackRatingRepository = null;
                    _servicePeriodRepository = null;
                    _serviceTaxRateRepository = null;

                    _employeeRepository = null;
                    _jobPositionRepository = null;
                    _contractRepository = null;

                    _brandRepository = null;
                }

                _disposed = true;
            }
            base.Dispose(isDisposing);
        }
    }
}