using Autofac;
using Eagle.Repositories;
using Eagle.Services.Business;
using Eagle.Services.Contents;
using Eagle.Services.Messaging;
using Eagle.Services.Service;
using Eagle.Services.Service.MessageBroadCaster;
using Eagle.Services.Skins;
using Eagle.Services.SystemManagement;
using log4net.Core;
using log4net.Repository.Hierarchy;

namespace Eagle.Services
{
    public class ServicesAutoFacModule : Autofac.Module
    {
        private readonly string _connectionString;
        public ServicesAutoFacModule(string connectionString)
        {
            _connectionString = connectionString;
        }
        protected override void Load(ContainerBuilder builder)
        {
            // Registers Loggers here
            builder.RegisterType<Logger>().As<ILogger>();
            builder.RegisterModule(new Log4NetAutofacModule());

            builder.RegisterType<CacheService>().As<ICacheService>().InstancePerLifetimeScope();
            builder.RegisterType<CommonService>().As<ICommonService>().InstancePerLifetimeScope();
            builder.RegisterType<ContactService>().As<IContactService>().InstancePerLifetimeScope();
            builder.RegisterType<LogService>().As<ILogService>().InstancePerLifetimeScope();

            builder.RegisterType<ApplicationService>().As<IApplicationService>().InstancePerLifetimeScope();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
            builder.RegisterType<RoleService>().As<IRoleService>().InstancePerLifetimeScope();
            builder.RegisterType<PermissionService>().As<IPermissionService>().InstancePerLifetimeScope();

            builder.RegisterType<SecurityService>().As<ISecurityService>().InstancePerLifetimeScope();
            builder.RegisterType<LanguageService>().As<ILanguageService>().InstancePerLifetimeScope();
            builder.RegisterType<CompanyService>().As<ICompanyService>().InstancePerLifetimeScope();

            builder.RegisterType<ContentService>().As<IContentService>().InstancePerLifetimeScope();
            builder.RegisterType<ModuleService>().As<IModuleService>().InstancePerLifetimeScope();
            builder.RegisterType<PageService>().As<IPageService>().InstancePerLifetimeScope();
            builder.RegisterType<MenuService>().As<IMenuService>().InstancePerLifetimeScope();

            builder.RegisterType<BannerService>().As<IBannerService>().InstancePerLifetimeScope();
            builder.RegisterType<DocumentService>().As<IDocumentService>().InstancePerLifetimeScope();
            builder.RegisterType<NewsService>().As<INewsService>().InstancePerLifetimeScope();
            builder.RegisterType<GalleryService>().As<IGalleryService>().InstancePerLifetimeScope();
            builder.RegisterType<MediaService>().As<IMediaService>().InstancePerLifetimeScope();
            builder.RegisterType<TagService>().As<ITagService>().InstancePerLifetimeScope();

            builder.RegisterType<MailService>().As<IMailService>().InstancePerLifetimeScope();
            builder.RegisterType<MessageService>().As<IMessageService>().InstancePerLifetimeScope();
            builder.RegisterType<NotificationService>().As<INotificationService>().InstancePerLifetimeScope();
            builder.RegisterType<EventService>().As<IEventService>().InstancePerLifetimeScope();
            builder.RegisterType<BookingService>().As<IBookingService>().InstancePerLifetimeScope();

            builder.RegisterType<ThemeService>().As<IThemeService>().InstancePerLifetimeScope();
            builder.RegisterType<CurrencyService>().As<ICurrencyService>().InstancePerLifetimeScope();
            builder.RegisterType<CustomerService>().As<ICustomerService>().InstancePerLifetimeScope();
            builder.RegisterType<ProductService>().As<IProductService>().InstancePerLifetimeScope();
            builder.RegisterType<CartService>().As<ICartService>().InstancePerLifetimeScope();
            builder.RegisterType<SupplierService>().As<ISupplierService>().InstancePerLifetimeScope();
            builder.RegisterType<OrderService>().As<IOrderService>().InstancePerLifetimeScope();
            builder.RegisterType<TransactionService>().As<ITransactionService>().InstancePerLifetimeScope();
            builder.RegisterType<VendorService>().As<IVendorService>().InstancePerLifetimeScope();
            builder.RegisterType<ReportService>().As<IReportService>().InstancePerLifetimeScope();
            builder.RegisterType<ShippingService>().As<IShippingService>().InstancePerLifetimeScope();
            builder.RegisterType<EmployeeService>().As<IEmployeeService>().InstancePerLifetimeScope();
            builder.RegisterType<ContractService>().As<IContractService>().InstancePerLifetimeScope();
            builder.RegisterType<RosterService>().As<IRosterService>().InstancePerLifetimeScope();
            builder.RegisterType<BroadCastService>().As<IBroadCastService>().InstancePerLifetimeScope();
            builder.RegisterType<ChatService>().As<IChatService>().InstancePerLifetimeScope();
            builder.RegisterType<DocumentationService>().As<IDocumentationService>().InstancePerLifetimeScope();
            builder.RegisterType<BrandService>().As<IBrandService>().InstancePerLifetimeScope();
            builder.RegisterType<PayPalService>().As<IPayPalService>().InstancePerLifetimeScope();
            builder.RegisterModule(new RepositoryAutoFacModule(_connectionString));
        }
    }
}