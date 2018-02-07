using AutoMapper;
using Eagle.Entities;
using Eagle.Entities.Common;
using Eagle.Entities.Services.Messaging;
using Eagle.Entities.Skins;
using Eagle.Entities.SystemManagement;
using Eagle.Entities.SystemManagement.FileStorage;
using Eagle.Services.Dtos;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Services.Message;
using Eagle.Services.Dtos.Skins;
using Eagle.Services.Dtos.SystemManagement;
using Eagle.Services.Dtos.SystemManagement.FileStorage;
using Eagle.Services.Dtos.SystemManagement.Identity;
using Eagle.Services.EntityMapping.Common;

namespace Eagle.Services.EntityMapping
{
    public class SystemMapping
    {
        public static void ConfigureMapping()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<BaseEntity, BaseDto>();
                cfg.CreateMap<AppClaim, AppClaimDetail>();
                cfg.CreateMap<ApplicationEntity, ApplicationDetail>();
                cfg.CreateMap<DocumentFolder, DocumentFolderDetail>();
                cfg.CreateMap<DocumentFolderEntry, DocumentFolder>().IgnoreUnmapped();
                cfg.CreateMap<DocumentFileEntry, DocumentFile>().IgnoreUnmapped();
                cfg.CreateMap<DownloadTracking, DownloadTrackingDetail>();
                cfg.CreateMap<DocumentFile, DocumentFileDetail>();

                cfg.CreateMap<DocumentInfo, DocumentInfoDetail>()
                    .IncludeBase<DocumentFile, DocumentFileDetail>();

                cfg.CreateMap<FileBrowser, FileBrowserDetail>();

                cfg.CreateMap<Group, GroupDetail>().ReverseMap();
                cfg.CreateMap<GroupEntry, Group>().ReverseMap();

                cfg.CreateMap<Role, RoleDetail>().ReverseMap();
                cfg.CreateMap<RoleInfo, RoleInfoDetail>()
                 .ForMember(dest => dest.Groups, src => src.MapFrom(x => x.Groups))
                 .ForMember(dest => dest.Users, src => src.MapFrom(x => x.Users));
                cfg.CreateMap<RoleEntry, Role>();
                cfg.CreateMap<RoleEditEntry, Role>();
                cfg.CreateMap<RoleGroup, RoleGroupDetail>();

                cfg.CreateMap<Group, GroupDetail>();
                cfg.CreateMap<GroupEntry, Group>();
                cfg.CreateMap<GroupEditEntry, Group>();

                cfg.CreateMap<Contact, ContactDetail>();
                cfg.CreateMap<ContactEntry, Contact>().IgnoreUnmapped();

                cfg.CreateMap<Address, AddressDetail>();
                cfg.CreateMap<UserAddress, UserAddressDetail>()
                   .ForMember(dest => dest.Address, src => src.MapFrom(x => x.Address));
                cfg.CreateMap<UserAddressInfo, UserAddressInfoDetail>()
                    .IncludeBase<UserAddress, UserAddressDetail>()
                    .ForMember(dest => dest.Address, src => src.MapFrom(x => x.Address));
                cfg.CreateMap<UserRoleGroup, UserRoleGroupDetail>();
                cfg.CreateMap<UserRoleGroupEntry, UserRoleGroup>();
                cfg.CreateMap<UserProfile, UserProfileDetail>();
                    //.ForMember(dest => dest.User, src => src.MapFrom(x => x.User))
                    //.ForMember(dest => dest.Contact, src => src.MapFrom(x => x.Contact))
                    //.ForMember(dest => dest.Addresses, src => src.MapFrom(x => x.Addresses));
                cfg.CreateMap<User, UserDetail>();
                cfg.CreateMap<UserEntry, User>().IgnoreUnmapped();
                cfg.CreateMap<UserInfo, UserInfoDetail>()
                    .ForMember(dest => dest.Application, src => src.MapFrom(x => x.Application))
                    .ForMember(dest => dest.User, src => src.MapFrom(x => x.User))
                    .ForMember(dest => dest.Profile, src => src.MapFrom(x => x.Profile));
                cfg.CreateMap<UserRole, UserRoleDetail>()
                 .ForMember(dest => dest.User, src => src.MapFrom(x => x.User))
                 .ForMember(dest => dest.Role, src => src.MapFrom(x => x.Role));
                cfg.CreateMap<UserRoleInfo, UserRoleInfoDetail>()
                 .ForMember(dest => dest.User, src => src.MapFrom(x => x.User))
                 .ForMember(dest => dest.Role, src => src.MapFrom(x => x.Role));

                cfg.CreateMap<ContentItem, ContentItemDetail>().ReverseMap();
                cfg.CreateMap<ContentItemInfo, ContentItemDetail>();
                cfg.CreateMap<ContentType, ContentTypeDetail>().ReverseMap();

                //cfg.CreateMap<ApplicationLanguage, ApplicationLanguageDetail>()
                //    .Include<Language, LanguageDetail>();
                    //.ForMember(dest => dest.Language, src => src.MapFrom(x => x.Language))
                    //     .AfterMap((src, dest) =>
                    //     {
                    //         Mapper.Map(src, dest);
                    //     });
                cfg.CreateMap<Language, LanguageDetail>().ReverseMap();
                cfg.CreateMap<ApplicationLanguageEntry, ApplicationLanguage>().IgnoreUnmapped();

                cfg.CreateMap<Module, ModuleDetail>().ReverseMap();
                cfg.CreateMap<ModuleEntry, Module>();
                cfg.CreateMap<ModuleCapability, ModuleCapabilityDetail>().ReverseMap();
                cfg.CreateMap<ModuleCapabilityEntry, ModuleCapability>();


                cfg.CreateMap<Page, PageDetail>().ReverseMap();
                cfg.CreateMap<PageEntry, Page>();
                cfg.CreateMap<PageModule, PageModuleDetail>().ReverseMap();
                cfg.CreateMap<PagePermission, PagePermissionDetail>().ReverseMap();
                cfg.CreateMap<PageInfo, PageInfoDetail>();

                cfg.CreateMap<NotificationSender, NotificationSenderDetail>().ReverseMap();
                cfg.CreateMap<MailServerProvider, MailServerProviderDetail>().ReverseMap();
                cfg.CreateMap<MessageQueue, MessageQueueDetail>().ReverseMap();
                cfg.CreateMap<MessageTemplate, MessageTemplateDetail>().ReverseMap();

                cfg.CreateMap<MenuEntry, Menu>().IgnoreUnmapped();
                cfg.CreateMap<Menu, MenuDetail>();
                cfg.CreateMap<MenuPermission, MenuPermissionDetail>().ReverseMap();
                cfg.CreateMap<MenuPermissionEntry, MenuPermission>();
                cfg.CreateMap<MenuPage, MenuPageDetail>()
                   .ForMember(dest => dest.Page, opt => opt.MapFrom(src => src.Page));
                cfg.CreateMap<MenuTree, MenuTreeDetail>().ReverseMap();

                cfg.CreateMap<TreeEntity, TreeDetail>();

                //Mapper.CreateMap<MemberProfile, Dtos.Community.Birthdays>()
                //    .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Member.FirstName))
                //    .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Member.Surname))
                //    .ForMember(dest => dest.Birthday, opt => opt.MapFrom(src => src.BirthDay))
                //    .ForMember(dest => dest.NetworkId, opt => opt.MapFrom(src => src.Member.NetworkId))
                //    .ForMember(dest => dest.MemberId, opt => opt.MapFrom(src => src.MemberId));

                cfg.CreateMap<Theme, ThemeDetail>();

            });
        }
    }
}
