using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
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
using IsolationLevel = System.Data.IsolationLevel;

namespace Eagle.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        void RegisterRepositories<TRepository>(IEnumerable<IRepository> repositories) where TRepository : IRepository;

        Guid InstanceId { get; }
        int SaveChanges();
        int SaveChanges(TransactionScopeOption scopeOption);
        Task<int> SaveChangesAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified);
        bool Commit();
        void Rollback();

        ICommonRepository CommonRepository { get; }
        ICacheManager CacheManager { get; }

        IApplicationRepository ApplicationRepository { get; }
        IApplicationSettingRepository ApplicationSettingRepository { get; }
        IApplicationLanguageRepository ApplicationLanguageRepository { get; }

        ICountryRepository CountryRepository { get; }
        IProvinceRepository ProvinceRepository { get; }
        IRegionRepository RegionRepository { get; }
        IAddressRepository AddressRepository { get; }
        IContactRepository ContactRepository { get; }
        
        ICompanyRepository CompanyRepository { get; }

        IUserRepository UserRepository { get; }
        IUserClaimRepository UserClaimRepository { get; }
        IUserProfileRepository UserProfileRepository { get; }
        IUserAddressRepository UserAddressRepository { get; }
        IUserRoleRepository UserRoleRepository { get; }
        IUserRoleGroupRepository UserRoleGroupRepository { get; }
        IUserVendorRepository UserVendorRepository { get; }

        ILanguageRepository LanguageRepository { get; }
        IContentTypeRepository ContentTypeRepository { get; }
        IContentItemRepository ContentItemRepository { get; }
        IRoleRepository RoleRepository { get; }
        IGroupRepository GroupRepository { get; }
        IRoleGroupRepository RoleGroupRepository { get; }

        IPermissionRepository PermissionRepository { get; }
        IMenuPermissionRepository MenuPermissionRepository { get; }
        IModulePermissionRepository ModulePermissionRepository { get; }
        IModuleCapabilityRepository ModuleCapabilityRepository { get; }
        IPagePermissionRepository PagePermissionRepository { get; }

        IPageRepository PageRepository { get; }
        IModuleRepository ModuleRepository { get; }
        IPageModuleRepository PageModuleRepository { get; }
       
        IMenuRepository MenuRepository { get; }
        IMenuTypeRepository MenuTypeRepository { get; }
        IMenuPositionRepository MenuPositionRepository { get; }
        IMenuPermissionLevelRepository MenuPermissionLevelRepository { get; }
        ISiteMapRepository SiteMapRepository { get; }


        IChatPrivateMessageMasterRepository ChatPrivateMessageMasterRepository { get; }
        IChatPrivateMessageRepository ChatPrivateMessageRepository { get; }
        IChatUserRepository ChatUserRepository { get; }
        IChatMessageRepository ChatMessageRepository { get; }


        INotificationSenderRepository NotificationSenderRepository { get; }
        IMailServerProviderRepository MailServerProviderRepository { get; }
        IMessageTypeRepository MessageTypeRepository { get; }
        IMessageTemplateRepository MessageTemplateRepository { get; }
        IMessageQueueRepository MessageQueueRepository { get; }
        INotificationTypeRepository NotificationTypeRepository { get; }
        INotificationTargetRepository NotificationTargetRepository { get; }
        INotificationTargetDefaultRepository NotificationTargetDefaultRepository { get; }
        INotificationMessageRepository NotificationMessageRepository { get; }
        INotificationMessageTypeRepository NotificationMessageTypeRepository { get; }



        ISkinPackageTemplateRepository SkinPackageTemplateRepository { get; }
        ISkinPackageBackgroundRepository SkinPackageBackgroundRepository { get; }
        ISkinPackageRepository SkinPackageRepository { get; }
        ISkinPackageTypeRepository SkinPackageTypeRepository { get; }


        INewsRepository NewsRepository { get; }
        INewsCategoryRepository NewsCategoryRepository { get; }
        INewsCommentRepository NewsCommentRepository { get; }
        INewsRatingRepository NewsRatingRepository { get; }


        IEventRepository EventRepository { get; }
        IEventTypeRepository EventTypeRepository { get; }

        IFeedbackRepository FeedbackRepository { get; }


        IDocumentationRepository DocumentationRepository { get; }
        IDocumentFolderRepository DocumentFolderRepository { get; }
        IDocumentFileRepository DocumentFileRepository { get; }
        IDocumentPermissionRepository DocumentPermissionRepository { get; }


        ILogRepository LogRepository { get; }

        ITagRepository TagRepository { get; }
        ITagIntegrationRepository TagIntegrationRepository { get; }

        IBannerRepository BannerRepository { get; }
        IBannerPositionRepository BannerPositionRepository { get; }
        IBannerTypeRepository BannerTypeRepository { get; }
        IBannerScopeRepository BannerScopeRepository { get; }
        IBannerPageRepository BannerPageRepository { get; }
        IBannerZoneRepository BannerZoneRepository { get; }

        IGalleryCollectionRepository GalleryCollectionRepository { get; }
        IGalleryTopicRepository GalleryTopicRepository { get; }
        IGalleryFileRepository GalleryFileRepository { get; }


        IMediaAlbumRepository MediaAlbumRepository { get; }
        IMediaArtistRepository MediaArtistRepository { get; }
        IMediaComposerRepository MediaComposerRepository { get; }
        IMediaFileRepository MediaFileRepository { get; }
        IMediaPlayListRepository MediaPlayListRepository { get; }
        IMediaTopicRepository MediaTopicRepository { get; }
        IMediaTypeRepository MediaTypeRepository { get; }
        IMediaAlbumFileRepository MediaAlbumFileRepository { get; }
        IMediaPlayListFileRepository MediaPlayListFileRepository { get; }

        ICurrencyRepository CurrencyRepository { get; }
        ICurrencyRateRepository CurrencyRateRepository { get; }


        ICustomerRepository CustomerRepository { get; }
        ICustomerTypeRepository CustomerTypeRepository { get; }

        IProductRepository ProductRepository { get; }
        IProductAlbumRepository ProductAlbumRepository { get; }
        IProductTypeRepository ProductTypeRepository { get; }
        IProductCategoryRepository ProductCategoryRepository { get; }
        IProductTaxRateRepository ProductTaxRateRepository { get; }
        IProductAttributeRepository ProductAttributeRepository { get; }
        IAttributeRepository AttributeRepository { get; }
        IProductAttributeOptionRepository ProductAttributeOptionRepository { get; }
        IAttributeOptionRepository AttributeOptionRepository { get; }
        IProductDiscountRepository ProductDiscountRepository { get; }
        IProductFileRepository ProductFileRepository { get; }
        IProductCommentRepository ProductCommentRepository { get; }
        IProductRatingRepository ProductRatingRepository { get; }

        IOrderRepository OrderRepository { get; }
        IOrderTempRepository OrderTempRepository { get; }
        IOrderProductRepository OrderProductRepository { get; }
        IOrderProductTempRepository OrderProductTempRepository { get; }
        IOrderProductOptionRepository OrderProductOptionRepository { get; }
        IOrderPaymentRepository OrderPaymentRepository { get; }
        IOrderShipmentRepository OrderShipmentRepository { get; }


        IPaymentMethodRepository PaymentMethodRepository { get; }
        ITransactionMethodRepository TransactionMethodRepository { get; }

        IManufacturerCategoryRepository ManufacturerCategoryRepository { get; }
        IManufacturerRepository ManufacturerRepository { get; }

        IVendorRepository VendorRepository { get; }
        IVendorCurrencyRepository VendorCurrencyRepository { get; }
        IVendorAddressRepository VendorAddressRepository { get; }
        IVendorCreditCardRepository VendorCreditCardRepository { get; }
        IVendorPartnerRepository VendorPartnerRepository { get; }
        IVendorShippingCarrierRepository VendorShippingCarrierRepository { get; }
        IVendorShippingMethodRepository VendorShippingMethodRepository { get; }
        IVendorPaymentMethodRepository VendorPaymentMethodRepository { get; }
        IVendorTransactionMethodRepository VendorTransactionMethodRepository { get; }


        IPromotionRepository PromotionRepository { get; }

        IShippingCarrierRepository ShippingCarrierRepository { get; }
        IShippingMethodRepository ShippingMethodRepository { get; }
        IShippingFeeRepository ShippingFeeRepository { get; }

        IServicePeriodRepository ServicePeriodRepository { get; }
        IServiceTaxRateRepository ServiceTaxRateRepository { get; }
        IServiceCategoryRepository ServiceCategoryRepository { get; }
        IServiceDiscountRepository ServiceDiscountRepository { get; }
        IServicePackRepository ServicePackRepository { get; }
        IServicePackOptionRepository ServicePackOptionRepository { get; }
        IServicePackDurationRepository ServicePackDurationRepository { get; }
        IServicePackProviderRepository ServicePackProviderRepository { get; }
        IServicePackRatingRepository ServicePackRatingRepository { get; }
        IServicePackTypeRepository ServicePackTypeRepository { get; }
     
        IContractRepository ContractRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }
        IEmployeeAvailabilityRepository EmployeeAvailabilityRepository { get; }
        IEmployeePositionRepository EmployeePositionRepository { get; }
        IEmployeeSkillRepository EmployeeSkillRepository { get; }
        IEmployeeTimeOffRepository EmployeeTimeOffRepository { get; }
        IJobPositionRepository JobPositionRepository { get; }
        IJobPositionSkillRepository JobPositionSkillRepository { get; }
        IQualificationRepository QualificationRepository { get; }
        IRewardDisciplineRepository RewardDisciplineRepository { get; }
        ISalaryRepository SalaryRepository { get; }
        ISkillRepository SkillRepository { get; }
        ITerminationRepository TerminationRepository { get; }
        IWorkingHistoryRepository WorkingHistoryRepository { get; }


        IPublicHolidaySetRepository PublicHolidaySetRepository { get; }
        IPublicHolidayRepository PublicHolidayRepository { get; }
        IShiftRepository ShiftRepository { get; }
        IShiftPositionRepository ShiftPositionRepository { get; }
        IShiftSwapRepository ShiftSwapRepository { get; }
        IShiftTypeRepository ShiftTypeRepository { get; }
        ITimeOffTypeRepository TimeOffTypeRepository { get; }
        ITimesheetRepository TimesheetRepository { get; }

        IBrandRepository BrandRepository { get; }

        //IRepository<TEntity> Repository<TEntity>() where TEntity : class, IObjectState;
        //void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified);
        //bool Commit();
        //void Rollback();
        //void Dispose(bool disposing);
    }
}
