using Autofac;
using Eagle.EntityFramework;
using Eagle.Repositories.Business;
using Eagle.Repositories.Business.Employees;
using Eagle.Repositories.Business.Roster;
using Eagle.Repositories.Business.Vendor;
using Eagle.Repositories.Contents;
using Eagle.Repositories.Services.Booking;
using Eagle.Repositories.Services.Chat;
using Eagle.Repositories.Services.Event;
using Eagle.Repositories.Services.Messaging;
using Eagle.Repositories.Skins;
using Eagle.Repositories.SystemManagement;

namespace Eagle.Repositories
{
    public class RepositoryAutoFacModule : Module
    {
        private readonly string _connectionString;

        public RepositoryAutoFacModule(string connectionString)
        {
            _connectionString = connectionString;
        }
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new DataContext(_connectionString)).As<IDataContext>().InstancePerRequest();

            // Register repositories for DbContext
            builder.RegisterType<CommonRepository>().As<ICommonRepository>().InstancePerDependency();
            builder.RegisterType<ApplicationRepository>().As<IApplicationRepository>().InstancePerDependency();
            builder.RegisterType<ApplicationSettingRepository>().As<IApplicationSettingRepository>().InstancePerDependency();
            builder.RegisterType<ApplicationLanguageRepository>().As<IApplicationLanguageRepository>().InstancePerDependency();
            builder.RegisterType<LanguageRepository>().As<ILanguageRepository>().InstancePerDependency();
            builder.RegisterType<LogRepository>().As<ILogRepository>().InstancePerDependency();

            builder.RegisterType<AddressRepository>().As<IAddressRepository>().InstancePerDependency();
            builder.RegisterType<ContactRepository>().As<IContactRepository>().InstancePerDependency();
            builder.RegisterType<CountryRepository>().As<ICountryRepository>().InstancePerDependency();
            builder.RegisterType<ProvinceRepository>().As<IProvinceRepository>().InstancePerDependency();
            builder.RegisterType<RegionRepository>().As<IRegionRepository>().InstancePerDependency();

            builder.RegisterType<ModuleRepository>().As<IModuleRepository>().InstancePerDependency();
            builder.RegisterType<PageRepository>().As<IPageRepository>().InstancePerDependency();
            builder.RegisterType<PageModuleRepository>().As<IPageModuleRepository>().InstancePerDependency();
            builder.RegisterType<ModuleCapabilityRepository>().As<IModuleCapabilityRepository>().InstancePerDependency();

            builder.RegisterType<PermissionRepository>().As<IPermissionRepository>().InstancePerDependency();
            builder.RegisterType<ModulePermissionRepository>().As<IModulePermissionRepository>().InstancePerDependency();
            builder.RegisterType<MenuPermissionRepository>().As<IMenuPermissionRepository>().InstancePerDependency();
            builder.RegisterType<PagePermissionRepository>().As<IPagePermissionRepository>().InstancePerDependency();

            builder.RegisterType<RoleRepository>().As<IRoleRepository>().InstancePerDependency();
            builder.RegisterType<GroupRepository>().As<IGroupRepository>().InstancePerDependency();

            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerDependency();
            builder.RegisterType<UserClaimRepository>().As<IUserClaimRepository>().InstancePerDependency();
            builder.RegisterType<UserProfileRepository>().As<IUserProfileRepository>().InstancePerDependency();
            builder.RegisterType<UserRoleRepository>().As<IUserRoleRepository>().InstancePerDependency();
            builder.RegisterType<UserVendorRepository>().As<IUserVendorRepository>().InstancePerDependency();

            builder.RegisterType<ContentItemRepository>().As<IContentItemRepository>().InstancePerDependency();
            builder.RegisterType<ContentTypeRepository>().As<IContentTypeRepository>().InstancePerDependency();

            builder.RegisterType<DocumentationRepository>().As<IDocumentationRepository>().InstancePerDependency();
            builder.RegisterType<DocumentFileRepository>().As<IDocumentFileRepository>().InstancePerDependency();
            builder.RegisterType<DocumentFolderRepository>().As<IDocumentFolderRepository>().InstancePerDependency();
            builder.RegisterType<DocumentPermissionRepository>().As<IDocumentPermissionRepository>().InstancePerDependency();
            
            builder.RegisterType<MenuRepository>().As<IMenuRepository>().InstancePerDependency();
            builder.RegisterType<MenuPositionRepository>().As<IMenuPositionRepository>().InstancePerDependency();
            builder.RegisterType<MenuTypeRepository>().As<IMenuTypeRepository>().InstancePerDependency();
            builder.RegisterType<MenuPermissionLevelRepository>().As<IMenuPermissionLevelRepository>().InstancePerDependency();
            builder.RegisterType<SiteMapRepository>().As<ISiteMapRepository>().InstancePerDependency();

            builder.RegisterType<SkinPackageRepository>().As<ISkinPackageRepository>().InstancePerDependency();
            builder.RegisterType<SkinPackageTypeRepository>().As<ISkinPackageTypeRepository>().InstancePerDependency();
            builder.RegisterType<SkinPackageTemplateRepository>().As<ISkinPackageTemplateRepository>().InstancePerDependency();
            builder.RegisterType<SkinPackageBackgroundRepository>().As<ISkinPackageBackgroundRepository>().InstancePerDependency();

            builder.RegisterType<CompanyRepository>().As<ICompanyRepository>().InstancePerDependency();
            builder.RegisterType<EventRepository>().As<IEventRepository>().InstancePerDependency();
            builder.RegisterType<FeedbackRepository>().As<IFeedbackRepository>().InstancePerDependency();

            builder.RegisterType<NewsCategoryRepository>().As<INewsCategoryRepository>().InstancePerDependency();
            builder.RegisterType<NewsRepository>().As<INewsRepository>().InstancePerDependency();
            builder.RegisterType<NewsCommentRepository>().As<INewsCommentRepository>().InstancePerDependency();
            builder.RegisterType<NewsRatingRepository>().As<INewsRatingRepository>().InstancePerDependency();
            
            builder.RegisterType<BannerPositionRepository>().As<IBannerPositionRepository>().InstancePerDependency();
            builder.RegisterType<BannerRepository>().As<IBannerRepository>().InstancePerDependency();
            builder.RegisterType<BannerTypeRepository>().As<IBannerTypeRepository>().InstancePerDependency();
            builder.RegisterType<BannerScopeRepository>().As<IBannerScopeRepository>().InstancePerDependency();
            builder.RegisterType<BannerPageRepository>().As<IBannerPageRepository>().InstancePerDependency();
            builder.RegisterType<BannerZoneRepository>().As<IBannerZoneRepository>().InstancePerDependency();

            builder.RegisterType<GalleryCollectionRepository>().As<IGalleryCollectionRepository>().InstancePerDependency();
            builder.RegisterType<GalleryTopicRepository>().As<IGalleryTopicRepository>().InstancePerDependency();
            builder.RegisterType<GalleryFileRepository>().As<IGalleryFileRepository>().InstancePerDependency();
            builder.RegisterType<TagRepository>().As<ITagRepository>().InstancePerDependency();

            builder.RegisterType<MediaAlbumRepository>().As<IMediaAlbumRepository>().InstancePerDependency();
            builder.RegisterType<MediaArtistRepository>().As<IMediaArtistRepository>().InstancePerDependency();
            builder.RegisterType<MediaComposerRepository>().As<IMediaComposerRepository>().InstancePerDependency();
            builder.RegisterType<MediaFileRepository>().As<IMediaFileRepository>().InstancePerDependency();
            builder.RegisterType<MediaPlayListRepository>().As<IMediaPlayListRepository>().InstancePerDependency();
            builder.RegisterType<MediaTopicRepository>().As<IMediaTopicRepository>().InstancePerDependency();
            builder.RegisterType<MediaTypeRepository>().As<IMediaTypeRepository>().InstancePerDependency();

            builder.RegisterType<CurrencyRepository>().As<ICurrencyRepository>().InstancePerDependency();
            builder.RegisterType<CurrencyRateRepository>().As<ICurrencyRateRepository>().InstancePerDependency();
            builder.RegisterType<CustomerRepository>().As<ICustomerRepository>().InstancePerDependency();
            builder.RegisterType<CustomerTypeRepository>().As<ICustomerTypeRepository>().InstancePerDependency();

            builder.RegisterType<ProductRepository>().As<IProductRepository>().InstancePerDependency();
            builder.RegisterType<ProductAlbumRepository>().As<IProductAlbumRepository>().InstancePerDependency();
            builder.RegisterType<ProductTypeRepository>().As<IProductTypeRepository>().InstancePerDependency();
            builder.RegisterType<ProductAttributeRepository>().As<IProductAttributeRepository>().InstancePerDependency();
            builder.RegisterType<ProductAttributeOptionRepository>().As<IProductAttributeOptionRepository>().InstancePerDependency();
            builder.RegisterType<ProductCategoryRepository>().As<IProductCategoryRepository> ().InstancePerDependency();
            builder.RegisterType<ProductCommentRepository>().As<IProductCommentRepository>().InstancePerDependency();
            builder.RegisterType<ProductDiscountRepository>().As<IProductDiscountRepository>().InstancePerDependency();
            builder.RegisterType<ProductFileRepository>().As<IProductFileRepository>().InstancePerDependency();
            builder.RegisterType<ProductTaxRateRepository>().As<IProductTaxRateRepository>().InstancePerDependency();
            builder.RegisterType<ProductTypeRepository>().As<IProductTypeRepository>().InstancePerDependency();
            builder.RegisterType<PromotionRepository>().As<IPromotionRepository>().InstancePerDependency();
            builder.RegisterType<ProductRatingRepository>().As<IProductRatingRepository>().InstancePerDependency();
            builder.RegisterType<BrandRepository>().As<IBrandRepository>().InstancePerDependency();

            builder.RegisterType<PaymentMethodRepository>().As<IPaymentMethodRepository>().InstancePerDependency();
            builder.RegisterType<TransactionMethodRepository>().As<ITransactionMethodRepository>().InstancePerDependency();
            builder.RegisterType<OrderRepository>().As<IOrderRepository>().InstancePerDependency();
            builder.RegisterType<OrderTempRepository>().As<IOrderTempRepository>().InstancePerDependency();
            builder.RegisterType<OrderProductRepository>().As<IOrderProductRepository>().InstancePerDependency();
            builder.RegisterType<OrderProductTempRepository>().As<IOrderProductTempRepository>().InstancePerDependency();
            builder.RegisterType<OrderProductOptionRepository>().As<IOrderProductOptionRepository>().InstancePerDependency();
            builder.RegisterType<OrderPaymentRepository>().As<IOrderPaymentRepository>().InstancePerDependency();
            builder.RegisterType<OrderShipmentRepository>().As<IOrderShipmentRepository>().InstancePerDependency();


            builder.RegisterType<ShippingCarrierRepository>().As<IShippingCarrierRepository>().InstancePerDependency();
            builder.RegisterType<ShippingMethodRepository>().As<IShippingMethodRepository>().InstancePerDependency();
            builder.RegisterType<ShippingFeeRepository>().As<IShippingFeeRepository>().InstancePerDependency();

            builder.RegisterType<VendorRepository>().As<IVendorRepository>().InstancePerDependency();
            builder.RegisterType<VendorAddressRepository>().As<IVendorAddressRepository>().InstancePerDependency();
            builder.RegisterType<VendorCreditCardRepository>().As<IVendorCreditCardRepository>().InstancePerDependency();
            builder.RegisterType<VendorCurrencyRepository>().As<IVendorCurrencyRepository>().InstancePerDependency();
            builder.RegisterType<VendorPartnerRepository>().As<IVendorPartnerRepository>().InstancePerDependency();
            builder.RegisterType<VendorShippingCarrierRepository>().As<IVendorShippingCarrierRepository>().InstancePerDependency();
            builder.RegisterType<VendorShippingMethodRepository>().As<IVendorShippingMethodRepository>().InstancePerDependency();
            builder.RegisterType<VendorPaymentMethodRepository>().As<IVendorPaymentMethodRepository>().InstancePerDependency();
            builder.RegisterType<VendorTransactionMethodRepository>().As<IVendorTransactionMethodRepository>().InstancePerDependency();



            builder.RegisterType<ServiceCategoryRepository>().As<IServiceCategoryRepository>().InstancePerDependency();
            builder.RegisterType<ServiceDiscountRepository>().As<IServiceDiscountRepository>().InstancePerDependency();
            builder.RegisterType<ServicePackRepository>().As<IServicePackRepository>().InstancePerDependency();
            builder.RegisterType<ServicePackDurationRepository>().As<IServicePackDurationRepository>().InstancePerDependency();
            builder.RegisterType<ServicePackProviderRepository>().As<IServicePackProviderRepository>().InstancePerDependency();
            builder.RegisterType<ServicePackRatingRepository>().As<IServicePackRatingRepository>().InstancePerDependency();
            builder.RegisterType<ServicePackTypeRepository>().As<IServicePackTypeRepository>().InstancePerDependency();
            builder.RegisterType<ServicePeriodRepository>().As<IServicePeriodRepository>().InstancePerDependency();
            builder.RegisterType<ServicePackOptionRepository>().As<IServicePackOptionRepository>().InstancePerDependency();
            builder.RegisterType<ServiceTaxRateRepository>().As<IServiceTaxRateRepository>().InstancePerDependency();

            builder.RegisterType<ContractRepository>().As<IContractRepository>().InstancePerDependency();
            builder.RegisterType<EmployeeRepository>().As<IEmployeeRepository>().InstancePerDependency();
            builder.RegisterType<EmployeeAvailabilityRepository>().As<IEmployeeAvailabilityRepository>().InstancePerDependency();
            builder.RegisterType<EmployeePositionRepository>().As<IEmployeePositionRepository>().InstancePerDependency();
            builder.RegisterType<EmployeeSkillRepository>().As<IEmployeeSkillRepository>().InstancePerDependency();
            builder.RegisterType<EmployeeTimeOffRepository>().As<IEmployeeTimeOffRepository>().InstancePerDependency();
            builder.RegisterType<JobPositionRepository>().As<IJobPositionRepository>().InstancePerDependency();
            builder.RegisterType<JobPositionSkillRepository>().As<IJobPositionSkillRepository>().InstancePerDependency();
            builder.RegisterType<QualificationRepository>().As<IQualificationRepository>().InstancePerDependency();
            builder.RegisterType<RewardDisciplineRepository>().As<IRewardDisciplineRepository>().InstancePerDependency();
            builder.RegisterType<SalaryRepository>().As<ISalaryRepository>().InstancePerDependency();

            builder.RegisterType<PublicHolidaySetRepository>().As<IPublicHolidaySetRepository>().InstancePerDependency();
            builder.RegisterType<PublicHolidayRepository>().As<IPublicHolidayRepository>().InstancePerDependency();
            builder.RegisterType<ShiftRepository>().As<IShiftRepository>().InstancePerDependency();
            builder.RegisterType<ShiftPositionRepository>().As<IShiftPositionRepository>().InstancePerDependency();
            builder.RegisterType<ShiftSwapRepository>().As<IShiftSwapRepository>().InstancePerDependency();
            builder.RegisterType<ShiftTypeRepository>().As<IShiftTypeRepository>().InstancePerDependency();
            builder.RegisterType<TimeOffTypeRepository>().As<ITimeOffTypeRepository>().InstancePerDependency();
            builder.RegisterType<TimesheetRepository>().As<ITimesheetRepository>().InstancePerDependency();


            builder.RegisterType<NotificationSenderRepository>().As<INotificationSenderRepository>().InstancePerDependency();
            builder.RegisterType<MailServerProviderRepository>().As<IMailServerProviderRepository>().InstancePerDependency();
            builder.RegisterType<MessageTypeRepository>().As<IMessageTypeRepository>().InstancePerDependency();
            builder.RegisterType<MessageTemplateRepository>().As<IMessageTemplateRepository>().InstancePerDependency();
            builder.RegisterType<MessageQueueRepository>().As<IMessageQueueRepository>().InstancePerDependency();
            builder.RegisterType<NotificationTypeRepository>().As<INotificationTypeRepository>().InstancePerDependency();
            builder.RegisterType<NotificationTargetRepository>().As<INotificationTargetRepository>().InstancePerDependency();
            builder.RegisterType<NotificationTargetDefaultRepository>().As<INotificationTargetDefaultRepository>().InstancePerDependency();
            builder.RegisterType<NotificationMessageTypeRepository>().As<INotificationMessageTypeRepository>().InstancePerDependency();

            builder.RegisterType<ChatPrivateMessageMasterRepository>().As<IChatPrivateMessageMasterRepository>().InstancePerDependency();
            builder.RegisterType<ChatPrivateMessageRepository>().As<IChatPrivateMessageRepository>().InstancePerDependency();
            builder.RegisterType<ChatUserRepository>().As<IChatUserRepository>().InstancePerDependency();
            builder.RegisterType<ChatMessageRepository>().As<IChatMessageRepository>().InstancePerDependency();


            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();

            builder.RegisterModule(new DataEntityFrameworkAutoFacModule(_connectionString));
        }
    }
}
