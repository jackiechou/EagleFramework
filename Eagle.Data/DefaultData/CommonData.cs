using System;
using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Common.Settings;
using Eagle.Core.Configuration;
using Eagle.Entities.Contents.Banners;
using Eagle.Entities.Services.Messaging;
using Eagle.Resources;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Eagle.Data.DefaultData
{
    public class CommonData
    {
        public static void Set(ApplicationDbContext context)
        {
            //var application = new ApplicationEntity()
            //{
            //    ApplicationId = GlobalSettings.DefaultApplicationId,
            //    ApplicationName = "ADMINISTRATOR WEB SYSTEM",
            //    DefaultLanguage = "vi-VN",
            //    HomeDirectory = "",
            //    Currency = "",
            //    TimeZoneOffset = "",
            //    Url = "",
            //    LogoFile = "",
            //    BackgroundFile = "",
            //    KeyWords = "",
            //    CopyRight = "",
            //    FooterText = "",
            //    Description = "",
            //    HostSpace = 0,
            //    HostFee = 0,
            //    ExpiryDate = null,
            //    RegisteredUserId=null
            //};
            //context.Application.Add(application);

            //var applicationLanguages = new List<ApplicationLanguage>
            //{
            //     new ApplicationLanguage
            //    {
            //        ApplicationLanguageId = 1,
            //        ApplicationId = GlobalSettings.DefaultApplicationId,
            //        LanguageCode = "en-US"
            //     },
            //     new ApplicationLanguage
            //    {
            //        ApplicationLanguageId = 2,
            //        ApplicationId = GlobalSettings.DefaultApplicationId,
            //        LanguageCode = "vi-VN"
            //     },
            //};
            //context.ApplicationLanguage.AddRange(applicationLanguages);


            //var applicationSettings = new List<ApplicationSetting>
            //{
            //    new ApplicationSetting()
            //    {
            //        SettingId = 1,
            //        ApplicationId= GlobalSettings.DefaultApplicationId,
            //        SettingName = "Setting 01",
            //        SettingValue = "1",
            //        IsSecure=false
            //    },
            //};
            //context.ApplicationSetting.AddRange(applicationSettings);

            //var bannerScopes = new List<BannerScope>
            //{
            //    new BannerScope
            //    {
            //        Id = 1,
            //        ScopeName = LanguageResource.Unlimited,
            //        Description = LanguageResource.UnlimitedCampaign
            //    },
            //    new BannerScope
            //    {
            //        Id = 2,
            //        ScopeName = LanguageResource.DateTimeScope,
            //        Description = LanguageResource.LimitBannerViewsByDateTime
            //    },
            //    new BannerScope
            //    {
            //        Id = 3,
            //        ScopeName = LanguageResource.Clicks,
            //        Description = LanguageResource.LimitBannerViewsByClicks
            //    },
            //     new BannerScope
            //    {
            //        Id = 4,
            //        ScopeName = LanguageResource.Impressions,
            //        Description = LanguageResource.LimitBannerViewsByImpressions
            //    }
            //};

            //context.BannerScope.AddRange(bannerScopes);


            //// var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            //List<RoleGroup> roleGroups = new List<RoleGroup>()
            //{
            //    new RoleGroup { GroupId = 1, GroupName ="Admin"},
            //    new RoleGroup { GroupId = 2, GroupName ="Moderator"},
            //    new RoleGroup { GroupId = 3, GroupName ="Member"}
            //};

            //foreach (RoleGroup roleGroup in roleGroups)
            //{
            //    context.RoleGroups.Add(roleGroup);
            //}

            //List<FunctionCommand> functionCommands = new List<FunctionCommand>()
            //{
            //    new FunctionCommand { Id =  Guid.NewGuid(), Name ="QAInspection"},
            //    new FunctionCommand { Id =  Guid.NewGuid(), Name ="Reporting"},
            //    new FunctionCommand { Id =  Guid.NewGuid(), Name ="ServiceRequests"},
            //    new FunctionCommand { Id =  Guid.NewGuid(), Name ="AdminPortal"},
            //    new FunctionCommand { Id =  Guid.NewGuid(), Name ="VariationRequests"},
            //    new FunctionCommand { Id =  Guid.NewGuid(), Name ="Cleaners"},
            //    new FunctionCommand { Id =  Guid.NewGuid(), Name ="Contact"},
            //    new FunctionCommand { Id =  Guid.NewGuid(), Name ="StaffAndContractor"},
            //    new FunctionCommand { Id =  Guid.NewGuid(), Name ="CustomerDetails"},
            //    new FunctionCommand { Id =  Guid.NewGuid(), Name ="SiteTemplate"}

            //};

            //foreach (FunctionCommand functionCommand in functionCommands)
            //{
            //    context.FunctionCommands.Add(functionCommand);
            //}

            //List<String> claims = new List<string> { "QAInspection", "Reporting", "ServiceRequests","AdminPortal",
            //    "VariationRequests", "Cleaners", "Contact", "StaffAndContractor", "CustomerDetails", "SiteTemplate" };
            //List<AppClaim> appClaims = new List<AppClaim>();
            //for (int index = 0; index < claims.Count; index++)
            //{
            //    AppClaim claim = new AppClaim { Id = Guid.NewGuid(), Key = "role", Value = claims[index] };
            //    claim.RoleGroups.Add(roleGroups[5]);
            //    claim.FunctionCommands.Add(functionCommands[index]);
            //    appClaims.Add(claim);
            //    context.AppClaim.Add(claim);
            //}

            //ApplicationUser user = new ApplicationUser()
            //{
            //    FirstName = "Admin",
            //    UserName = "interactive@tma.com.vn",
            //    Email = "interactive@tma.com.vn",
            //    ChangePassword = true,
            //    InActived = true
            //};


            //var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            //string password = "12345678x@X";
            //IdentityResult result = manager.Create(user, password);

            //foreach (String appClaim in claims)
            //{
            //    manager.AddClaim(user.Id, new Claim("role", appClaim));
            //}
            //var adminAccount = new Account
            //{
            //    Id = Guid.NewGuid(),
            //    UserType = UserType.ADMIN,
            //    ProfileType = ProfileType.STAFF,
            //    InActive = true,
            //    IsDeleted = false,
            //    Profile = new Profile { Id = Guid.NewGuid(), UserId = user.Id, FirstName = "Admin", LastName = "Admin", StartDate = DateTime.UtcNow, DOB = DateTime.UtcNow, IsDeleted = false }
            //};
            //context.Account.Add(adminAccount);

            //#region Email Template
            //MessageTemplate template = new MessageTemplate()
            //{
            //    Id = Convert.ToInt32(BaseConstants.EMAIL_TEMPLATE_VARIATION_SERVICE_REQUEST),
            //    TemplateName = "Variation Service Create",
            //    TemplateSubject = "Variation Serivce",
            //    TemplateBody = "Dear ##UserName##,<br>"
            //     + "<br>"
            //     + "Thank you for submitting a Variation Request, this is an automated response confirming the receipt of your request.One of our support members will process your request as soon as possible.<br>"
            //     + "<br>"
            //     + "For your records, the details of the ticket are listed below.<br>"
            //     + "<strong>VR Number:</strong> ##VRNumber##<br>"
            //     + "<strong>Date Range:</strong> ##StartDate## - ##EndDate##<br>"
            //     + "<strong>Client Name:</strong> ##ClientName##<br>"
            //     + "<strong>Site Name:</strong> ##SiteName##<br>"
            //     + "<strong>Comments:</strong> ##Comment##<br>"
            //     + "<br>"
            //     + "<strong>Date:</strong> ##CreteDate##<br>"
            //     + "<strong>Time:</strong> ##CreteTime##<br>"
            //     + "<br>"
            //     + "Best Wishes,<br>"
            //     + "Shining Knight Support<br>"
            //};
            //context.MessageTemplate.Add(template);

            //MessageTemplate templateRequestCompletion = new MessageTemplate()
            //{
            //    Id = Convert.ToInt32(BaseConstants.EMAIL_TEMPLATE_VARIATION_SERVICE_REQUEST_COMPLETION),
            //    TemplateName = "Variation Request Completion",
            //    TemplateSubject = "Variation Serivce",
            //    TemplateBody = "Dear ##UserName##,<br>"
            //   + "<br>"
            //   + "This is an automated update to confirm the completion of your Variation Request.<br>"
            //   + "<br>"
            //   + "For your records, the details of the ticket are listed below.<br>"
            //   + "<br>"
            //   + "For your records, the details of the ticket are listed below.<br>"
            //   + "<strong>VR Number:</strong> ##VRNumber##<br>"
            //   + "<strong>Date Range:</strong> ##StartDate## - ##EndDate##<br>"
            //   + "<strong>Client Name:</strong> ##ClientName##<br>"
            //   + "<strong>Site Name:</strong> ##SiteName##<br>"
            //   + "<strong>Comments:</strong> ##Comment##<br>"
            //   + "<br>"
            //   + "<strong>Date:</strong> ##CreteDate##<br>"
            //   + "<strong>Time:</strong> ##CreteTime##<br>"
            //   + "<br>"
            //   + "Best Wishes,<br>"
            //   + "Shining Knight Support<br>"
            //};

            //context.MessageTemplate.Add(templateRequestCompletion);

            //MessageTemplate templateRequestCancellation = new MessageTemplate()
            //{
            //    Id = Convert.ToInt32(BaseConstants.EMAIL_TEMPLATE_VARIATION_SERVICE_REQUEST_CANCELLATION),
            //    TemplateName = "Variation Request Cancellation",
            //    //From = "interative@tma.com.vn",
            //    //Sender = "interative@tma.com.vn",
            //    TemplateSubject = "Variation Serivce",
            //    TemplateBody = "Dear ##UserName##,<br>"
            //   + "<br>"
            //   + "This is an automated update confirming cancellation of your Variation Request.<br>"
            //   + "<br>"
            //   + "For your records, the details of the ticket are listed below.<br>"
            //   + "<br>"
            //   + "For your records, the details of the ticket are listed below.<br>"
            //   + "<strong>VR Number:</strong> ##VRNumber##<br>"
            //   + "<strong>Date Range:</strong> ##StartDate## - ##EndDate##<br>"
            //   + "<strong>Client Name:</strong> ##ClientName##<br>"
            //   + "<strong>Site Name:</strong> ##SiteName##<br>"
            //   + "<strong>Comments:</strong> ##Comment##<br>"
            //   + "<br>"
            //   + "<strong>Date:</strong> ##CreteDate##<br>"
            //   + "<strong>Time:</strong> ##CreteTime##<br>"
            //   + "<br>"
            //   + "Best Wishes,<br>"
            //   + "Shining Knight Support<br>"
            //};

            //context.MessageTemplate.Add(templateRequestCancellation);

            //Domain.Mail.Entities.MessageTemplate userCreationTemplate = new Domain.Mail.Entities.MessageTemplate()
            //{
            //    Id = Convert.ToInt32(BaseConstants.EMAIL_TEMPLATE_USER_CREATION),
            //    TemplateName = "User Creation",
            //    TemplateSubject = "Create new account",
            //    TemplateBody = "Dear ##Username##,"
            //    + "<br>This is an automated message confirming you have been created as a new user within the Shining Knight Facility Services Application – Congratulations!"
            //    + "<br><br>Please find your unique user details below:"
            //    + "<br>Username: ##Username## "
            //    + "<br>Password: ##Password##"
            //    + "<br><br>Click the link below to login & get started! "
            //    + "<br><a href=##UrlLogin##></a>"
            //    + "<br><br>Best Wishes, <br>Shining Knight Support<br>"
            //};
            //context.MessageTemplate.Add(userCreationTemplate);
            //#endregion

            ////Create data to test cleaner list page
            //#region Add data cleaners
            ////Add manager user
            //var mamangerUser = new ApplicationUser { Id = Guid.NewGuid().ToString(), FirstName = "Manager", UserName = "interactive_manager@mailinator.com", Email = "interactive_manager@mailinator.com", ChangePassword = true, InActived = true };
            //manager.Create(mamangerUser, password);
            //foreach (String appClaim in claims)
            //{
            //    manager.AddClaim(mamangerUser.Id, new Claim("role", appClaim));
            //}
            //var managerAccount = new Account
            //{
            //    Id = Guid.NewGuid(),
            //    UserType = UserType.MANAGER,
            //    ProfileType = ProfileType.STAFF,
            //    InActive = true,
            //    IsDeleted = false,
            //    Profile = new Profile { Id = Guid.NewGuid(), UserId = mamangerUser.Id, Email = "interactive_manager@mailinator.com", FirstName = "ManagerF", LastName = "ManagerL", StartDate = DateTime.UtcNow, DOB = DateTime.UtcNow, IsDeleted = false }
            //};
            //context.Account.Add(managerAccount);
            ////Add CRM user
            //var CRMUser = new ApplicationUser { Id = Guid.NewGuid().ToString(), FirstName = "CRM", UserName = "interactive_crm@mailinator.com", Email = "interactive_crm@mailinator.com", ChangePassword = true, InActived = true };
            //manager.Create(CRMUser, password);
            //foreach (String appClaim in claims)
            //{
            //    manager.AddClaim(CRMUser.Id, new Claim("role", appClaim));
            //}
            ////Add client admin user
            //var clientAdminUser = new ApplicationUser { Id = Guid.NewGuid().ToString(), FirstName = "TMAAdmin", UserName = "tma_admin@mailinator.com", Email = "tma_admin@mailinator.com", ChangePassword = true, InActived = true };
            //manager.Create(clientAdminUser, password);
            //foreach (String appClaim in claims)
            //{
            //    manager.AddClaim(clientAdminUser.Id, new Claim("role", appClaim));
            //}
            ////Add site admin users
            //var siteAdminUser1 = new ApplicationUser { Id = Guid.NewGuid().ToString(), FirstName = "Lap1Admin", UserName = "tma_lap1_admin@mailinator.com", Email = "tma_lap1_admin@mailinator.com", ChangePassword = true, InActived = true };
            //manager.Create(siteAdminUser1, password);
            //foreach (String appClaim in claims)
            //{
            //    manager.AddClaim(siteAdminUser1.Id, new Claim("role", appClaim));
            //}
            //var siteAdminUser2 = new ApplicationUser { Id = Guid.NewGuid().ToString(), FirstName = "Lap2Admin", UserName = "tma_lap2_admin@mailinator.com", Email = "tma_lap2_admin@mailinator.com", ChangePassword = true, InActived = true };
            //manager.Create(siteAdminUser2, password);
            //foreach (String appClaim in claims)
            //{
            //    manager.AddClaim(siteAdminUser2.Id, new Claim("role", appClaim));
            //}
            //var siteAdminUser3 = new ApplicationUser { Id = Guid.NewGuid().ToString(), FirstName = "Lap3Admin", UserName = "tma_lap3_admin@mailinator.com", Email = "tma_lap3_admin@mailinator.com", ChangePassword = true, InActived = true };
            //manager.Create(siteAdminUser3, password);
            //foreach (String appClaim in claims)
            //{
            //    manager.AddClaim(siteAdminUser3.Id, new Claim("role", appClaim));
            //}
            //var lap3AdminId = Guid.NewGuid();
            ////Add Client
            //var client = new Client
            //{
            //    Id = Guid.NewGuid(),
            //    ClientName = "TMA",
            //    IsDeleted = false,
            //    ClientAdmins = new List<ClientAdmin> {
            //        new ClientAdmin { Id = Guid.NewGuid(),IsDeleted = false,
            //            Account =  new Account { Id = Guid.NewGuid(), UserType = UserType.CLIENT_ADMIN, ProfileType = ProfileType.STAFF, InActive = true, IsDeleted = false,
            //                Profile = new Profile { Id = Guid.NewGuid(), UserId = clientAdminUser.Id, Email = "tma_admin@mailinator.com", FirstName = "TMAAdminF", LastName = "TMAAdminL", StartDate = DateTime.UtcNow, DOB = DateTime.UtcNow, IsDeleted = false }
            //            }
            //        }
            //    },
            //    //Add ClientSite
            //    ClientSites = new List<ClientSite>() {
            //        //Site Lap 1
            //        new ClientSite { Id = Guid.NewGuid(), SiteName = "Lab 1", SiteLocation = "Phu Nhuan", CreatedAt = DateTime.UtcNow, StartDate = DateTime.UtcNow, Frequency = Domain.Application.Entities.FREQUENCY.MONTHLY, KPI = 1,
            //            SiteAdmins = new List<SiteAdmin> { new SiteAdmin { Id = Guid.NewGuid(),State = Domain.Common.Entities.ObjectState.Added,InActive = true,IsDeleted =false,
            //                    Account =  new Account { Id = Guid.NewGuid(), UserType = UserType.CLIENT_SITE_ADMIN, ProfileType = ProfileType.STAFF, InActive = true, IsDeleted = false,
            //                        Profile = new Profile { Id = Guid.NewGuid(), UserId = siteAdminUser1.Id, Email = "tma_lap1_admin@mailinator.com", FirstName = "Lab1AdminF", LastName = "Lab1AdminL", StartDate = DateTime.UtcNow, DOB = DateTime.UtcNow, IsDeleted = false }
            //                    }
            //                }
            //            },
            //            //Add QA Reports
            //            QAReports = new List<QAReport>(){
            //                new QAReport { Id = Guid.NewGuid(), CreateAt = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month-2, 1), QAReportStatus = QAReportStatus.Completed, IsDeleted = false },
            //            }
            //        },
            //        //Site Lap 2
            //        new ClientSite { Id = Guid.NewGuid(), SiteName = "Lab 2", SiteLocation = "Tan Binh", CreatedAt = DateTime.UtcNow, StartDate = DateTime.UtcNow, Frequency = Domain.Application.Entities.FREQUENCY.MONTHLY, KPI = 1,
            //            SiteAdmins = new List<SiteAdmin> {
            //                new SiteAdmin { Id = Guid.NewGuid(),State = Domain.Common.Entities.ObjectState.Added,InActive = true,IsDeleted =false,
            //                    Account =  new Account { Id = Guid.NewGuid(), UserType = UserType.CLIENT_SITE_ADMIN, ProfileType = ProfileType.STAFF, InActive = true, IsDeleted = false,
            //                        Profile = new Profile { Id = Guid.NewGuid(), UserId = siteAdminUser2.Id, Email = "tma_lap2_admin@mailinator.com", FirstName = "Lab2AdminF", LastName = "Lab2AdminL", StartDate = DateTime.UtcNow, DOB = DateTime.UtcNow, IsDeleted = false }
            //                    }
            //                },
            //                new SiteAdmin { Id = Guid.NewGuid(),State = Domain.Common.Entities.ObjectState.Added,InActive = true,IsDeleted =false,
            //                    Account =  new Account { Id = Guid.NewGuid(), UserType = UserType.CRM, ProfileType = ProfileType.STAFF, InActive = true, IsDeleted = false,
            //                        Profile = new Profile { Id = Guid.NewGuid(), UserId = CRMUser.Id, Email = "interactive_crm@mailinator.com", FirstName = "CRMF", LastName = "CRML", StartDate = DateTime.UtcNow, DOB = DateTime.UtcNow, IsDeleted = false }
            //                    }
            //                },
            //            },
            //            //Add Cleaners
            //            Cleaners = new List<Cleaner> {
            //                new Cleaner { Id = Guid.NewGuid(), SecurityCheck = true, IsDeleted = false,
            //                    Account = new Account { Id = Guid.NewGuid(), ProfileType = ProfileType.CONTRACTOR, UserType = UserType.CONTRACTOR, InActive = true, IsDeleted = false,
            //                        Profile = new Profile { Id = Guid.NewGuid(), FirstName = "Bacon", LastName = "Lewis", StartDate = DateTime.UtcNow, DOB = DateTime.UtcNow, IsDeleted = false }
            //                    },
            //                },
            //                new Cleaner { Id = Guid.NewGuid(), SecurityCheck = true, IsDeleted = false,
            //                    Account = new Account { Id = Guid.NewGuid(), ProfileType = ProfileType.CONTRACTOR, UserType = UserType.CONTRACTOR, InActive = true, IsDeleted = false,
            //                        Profile = new Profile { Id = Guid.NewGuid(), FirstName = "Daly", LastName = "Kiley", StartDate = DateTime.UtcNow, DOB = DateTime.UtcNow, IsDeleted = false }
            //                    },
            //                },
            //            },
            //            //Add QA Reports
            //        },
            //        //Site Lap 3
            //        new ClientSite { Id = Guid.NewGuid(), SiteName = "Lab 3", SiteLocation = "Quan 12", CreatedAt = DateTime.UtcNow, StartDate = DateTime.UtcNow, Frequency = Domain.Application.Entities.FREQUENCY.MONTHLY, KPI = 1,
            //            SiteAdmins = new List<SiteAdmin> { new SiteAdmin {  Id = lap3AdminId,State = Domain.Common.Entities.ObjectState.Added,InActive = true,IsDeleted =false,
            //                    Account =  new Account { Id = Guid.NewGuid(),  UserType = UserType.CLIENT_SITE_ADMIN, ProfileType = ProfileType.STAFF, InActive = true, IsDeleted = false,
            //                        Profile = new Profile { Id = Guid.NewGuid(), UserId = siteAdminUser3.Id, Email = "tma_lap3_admin@mailinator.com", FirstName = "Lab3AdminF", LastName = "Lab3AdminL", StartDate = DateTime.UtcNow, DOB = DateTime.UtcNow, IsDeleted = false }
            //                    }
            //                }
            //            },
            //            //Add Cleaners
            //            Cleaners = new List<Cleaner> {
            //                new Cleaner { Id = Guid.NewGuid(), SecurityCheck = true, IsDeleted = false,
            //                    Account = new Account { Id = Guid.NewGuid(), ProfileType = ProfileType.CONTRACTOR, UserType = UserType.CONTRACTOR, InActive = true, IsDeleted = false,
            //                        Profile = new Profile { Id = Guid.NewGuid(), FirstName = "Peter", LastName = "Maybury", StartDate = DateTime.UtcNow, DOB = DateTime.UtcNow, IsDeleted = false }
            //                    },
            //                },
            //                new Cleaner { Id = Guid.NewGuid(), SecurityCheck = true, IsDeleted = false,
            //                    Account = new Account { Id = Guid.NewGuid(), ProfileType = ProfileType.CONTRACTOR, UserType = UserType.CONTRACTOR, InActive = true, IsDeleted = false,
            //                        Profile = new Profile { Id = Guid.NewGuid(), FirstName = "Joanna", LastName = "Lewis", StartDate = DateTime.UtcNow, DOB = DateTime.UtcNow, IsDeleted = false }
            //                    },
            //                },
            //                new Cleaner { Id = Guid.NewGuid(), SecurityCheck = true, IsDeleted = false,
            //                    Account = new Account { Id = Guid.NewGuid(), ProfileType = ProfileType.CONTRACTOR, UserType = UserType.CONTRACTOR, InActive = true, IsDeleted = false,
            //                        Profile = new Profile { Id = Guid.NewGuid(), FirstName = "Kris", LastName = "Kiley", StartDate = DateTime.UtcNow, DOB = DateTime.UtcNow, IsDeleted = false }
            //                    },
            //                },
            //                new Cleaner { Id = Guid.NewGuid(), SecurityCheck = true, IsDeleted = false,
            //                    Account = new Account { Id = Guid.NewGuid(), ProfileType = ProfileType.CONTRACTOR, UserType = UserType.CONTRACTOR, InActive = true, IsDeleted = false,
            //                        Profile = new Profile { Id = Guid.NewGuid(), FirstName = "Heather", LastName = "Kiernan", StartDate = DateTime.UtcNow, DOB = DateTime.UtcNow, IsDeleted = false }
            //                    },
            //                },
            //            },
            //            //Add QA Reports
            //            QAReports = new List<QAReport>(){
            //                new QAReport { Id = Guid.NewGuid(), CreateAt = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1), QAReportGeneratorStatus = QAReportGeneratorStatus.Done, QAReportStatus = QAReportStatus.New, IsDeleted = false,
            //                    AuditCompleted = new QAAudit { Id = Guid.NewGuid(), AuditBy = lap3AdminId, AuditType = QAAuditType.CompletedBy, AuditTime = DateTime.UtcNow.AddDays(-10), Note = "Completed comments", IsDeleted = false },
            //                    AuditSign = new QAAudit { Id = Guid.NewGuid(), AuditBy = lap3AdminId, AuditType = QAAuditType.SignBy, AuditTime = DateTime.UtcNow.AddDays(-1), Note = "Sign comments", IsDeleted = false },
            //                    QAFormNo = "31023", StartDate = new DateTime(2016, 9, 1), EndDate = new DateTime(2016, 9, 30),
            //                    ReportTasks = new List<ReportTask>(){
            //                        new ReportTask { Id = Guid.NewGuid(), Name = "CARPETS", IsDeleted = false,
            //                            TaskAssessments = new List<TaskAssessment>{
            //                                new TaskAssessment { Id = Guid.NewGuid(), Name = "Pick up litter", IsDeleted = false, Score =  TaskScore.Fair, Comment = "Pick up cmt" },
            //                                new TaskAssessment { Id = Guid.NewGuid(), Name = "Vaccum carpet floors", IsDeleted = false, Score = TaskScore.Good, Comment = "Vaccum cmt" },
            //                                new TaskAssessment { Id = Guid.NewGuid(), Name = "Spot clean carpet", IsDeleted = false, Score = TaskScore.Good, Comment = "Spot cmt",
            //                                    TaskAttachments = new List<TaskAttachment> {
            //                                        new TaskAttachment { Id = Guid.NewGuid(), Name = "Test1", Url = "4hk23j4hk23j5k23j42.jpg" , IsDeleted = false },
            //                                        new TaskAttachment { Id = Guid.NewGuid(), Name = "Test2", Url = "23k4jh2k3j5k34j5h3j.png" , IsDeleted = false },
            //                                        new TaskAttachment { Id = Guid.NewGuid(), Name = "Test3", Url = "q4h35k34h5k4kh345.jpg" , IsDeleted = false },
            //                                    }
            //                                },
            //                            }
            //                        },
            //                        new ReportTask { Id = Guid.NewGuid(), Name ="DURABLE FLOORS", IsDeleted = false,
            //                            TaskAssessments = new List<TaskAssessment>{
            //                                new TaskAssessment { Id = Guid.NewGuid(), Name = "Pick up litter", IsDeleted = false, Score = TaskScore.Poor, Comment = "Pick up cmt" },
            //                                new TaskAssessment { Id = Guid.NewGuid(), Name = "Vacuum all carpet areas/under furniture/attention to corners and edges and skirting", IsDeleted = false, Score = TaskScore.Excellent, Comment = "Vaccum cmt" },
            //                                new TaskAssessment { Id = Guid.NewGuid(), Name = "Mop", IsDeleted = false, Score = TaskScore.Good, Comment = "Mop cmt" },
            //                            }
            //                        },
            //                        new ReportTask { Id = Guid.NewGuid(), Name ="SURFACES", IsDeleted = false,
            //                            TaskAssessments = new List<TaskAssessment>{
            //                                new TaskAssessment { Id = Guid.NewGuid(), Name = "Mirrors", IsDeleted = false, Score = TaskScore.Good, Comment = "Mirrors cmt" },
            //                                new TaskAssessment { Id = Guid.NewGuid(), Name = "Spot-clean all glass surfaces", IsDeleted = false, Score = TaskScore.Good },
            //                                new TaskAssessment { Id = Guid.NewGuid(), Name = "Clean stainless steel, chrome,and all glass / metal surfaces", IsDeleted = false, Score = TaskScore.Good,
            //                                    TaskAttachments = new List<TaskAttachment> {
            //                                        new TaskAttachment { Id = Guid.NewGuid(), Name = "Test4", Url = "243h2lkh5l235l2k344.jpg" , IsDeleted = false },
            //                                    }
            //                                },
            //                                new TaskAssessment { Id = Guid.NewGuid(), Name = "Spot-clean walls, partitions, columns,light switches, doors & door handles", IsDeleted = false, Score = TaskScore.Good },
            //                                new TaskAssessment { Id = Guid.NewGuid(), Name = "Spot-clean all plastic and vinyl chairs", IsDeleted = false, Score = TaskScore.Good },
            //                                new TaskAssessment { Id = Guid.NewGuid(), Name = "Vacuum all soft furnishings", IsDeleted = false, Score = TaskScore.Good },
            //                                new TaskAssessment { Id = Guid.NewGuid(), Name = "Damp-dust all cabinets, cupboards, frames, ledges, and other furniture and fittings below 2 metres", IsDeleted = false, Score = TaskScore.Good },
            //                            }
            //                        },
            //                    }
            //                },
            //            }
            //        },
            //    }
            //};
            //context.Client.Add(client);

            ////Add CRM user
            //var CRMUserGBS = new ApplicationUser { Id = Guid.NewGuid().ToString(), FirstName = "CRM_GBS", UserName = "interactive_crm_gbs@mailinator.com", Email = "interactive_crm_gbs@mailinator.com", ChangePassword = true, InActived = true };
            //manager.Create(CRMUserGBS, password);
            //foreach (String appClaim in claims)
            //{
            //    manager.AddClaim(CRMUserGBS.Id, new Claim("role", appClaim));
            //}

            ////Add client admin user
            //var clientAdminGBSUser = new ApplicationUser { Id = Guid.NewGuid().ToString(), FirstName = "GBSAdmin", UserName = "gbs_admin@mailinator.com", Email = "gbs_admin@mailinator.com", ChangePassword = true, InActived = true };
            //manager.Create(clientAdminGBSUser, password);
            //foreach (String appClaim in claims)
            //{
            //    manager.AddClaim(clientAdminGBSUser.Id, new Claim("role", appClaim));
            //}

            ////Add site admin users
            //var gbsSiteAdminUser = new ApplicationUser { Id = Guid.NewGuid().ToString(), FirstName = "LapMainAdmin", UserName = "gbs_labmain_admin@mailinator.com", Email = "gbs_labmain_admin@mailinator.com", ChangePassword = true, InActived = true };
            //manager.Create(gbsSiteAdminUser, password);
            //foreach (String appClaim in claims)
            //{
            //    manager.AddClaim(gbsSiteAdminUser.Id, new Claim("role", appClaim));
            //}

            //var clientGBS = new Client
            //{
            //    Id = Guid.NewGuid(),
            //    ClientName = "GlobalSS",
            //    IsDeleted = false,
            //    ClientAdmins = new List<ClientAdmin> {
            //        new ClientAdmin { Id = Guid.NewGuid(),IsDeleted = false,
            //            Account =  new Account { Id = Guid.NewGuid(), UserType = UserType.CLIENT_ADMIN, ProfileType = ProfileType.STAFF, InActive = true, IsDeleted = false,
            //                Profile = new Profile { Id = Guid.NewGuid(), UserId = clientAdminGBSUser.Id,  Email = "gbs_admin@mailinator.com", FirstName = "GBSAdminF", LastName = "GBSAdminL", StartDate = DateTime.UtcNow, DOB = DateTime.UtcNow, IsDeleted = false }
            //            }
            //        }
            //    },
            //    //Add ClientSite
            //    ClientSites = new List<ClientSite>() {
            //        //Site Lap 6
            //        new ClientSite { Id = Guid.NewGuid(), SiteName = "Lab Main", SiteLocation = "Quan 12", CreatedAt = DateTime.UtcNow, StartDate = DateTime.UtcNow, Frequency = Domain.Application.Entities.FREQUENCY.MONTHLY, KPI = 1,
            //            SiteAdmins = new List<SiteAdmin> {
            //                new SiteAdmin { Id = Guid.NewGuid(),State = Domain.Common.Entities.ObjectState.Added,InActive = true,IsDeleted =false,
            //                    Account =  new Account { Id = Guid.NewGuid(), UserType = UserType.CLIENT_SITE_ADMIN, ProfileType = ProfileType.STAFF, InActive = true, IsDeleted = false,
            //                        Profile = new Profile { Id = Guid.NewGuid(), UserId = gbsSiteAdminUser.Id, Email = "gbs_labmain_admin@mailinator.com", FirstName = "GBSLapAdminF", LastName = "GBSLapAdminL", StartDate = DateTime.UtcNow, DOB = DateTime.UtcNow, IsDeleted = false }
            //                    }
            //                },
            //                new SiteAdmin { Id = Guid.NewGuid(),State = Domain.Common.Entities.ObjectState.Added,InActive = true,IsDeleted =false,
            //                    Account =  new Account { Id = Guid.NewGuid(), UserType = UserType.CRM, ProfileType = ProfileType.STAFF, InActive = true, IsDeleted = false,
            //                        Profile = new Profile { Id = Guid.NewGuid(), UserId = CRMUserGBS.Id, Email = "interactive_crm_gbs@mailinator.com", FirstName = "CRMGBSF", LastName = "CRMGBSF", StartDate = DateTime.UtcNow, DOB = DateTime.UtcNow, IsDeleted = false }
            //                    }
            //                },
            //            },
            //            //Add Cleaners
            //            Cleaners = new List<Cleaner> {
            //                new Cleaner { Id = Guid.NewGuid(), SecurityCheck = true, IsDeleted = false,
            //                    Account = new Account { Id = Guid.NewGuid(), ProfileType = ProfileType.CONTRACTOR, UserType = UserType.CONTRACTOR, InActive = true, IsDeleted = false,
            //                        Profile = new Profile { Id = Guid.NewGuid(), FirstName = "Steven", LastName = "Gerrard", StartDate = DateTime.UtcNow, DOB = DateTime.UtcNow, IsDeleted = false }
            //                    },
            //                },
            //                new Cleaner { Id = Guid.NewGuid(), SecurityCheck = true, IsDeleted = false,
            //                    Account = new Account { Id = Guid.NewGuid(), ProfileType = ProfileType.CONTRACTOR, UserType = UserType.CONTRACTOR, InActive = true, IsDeleted = false,
            //                        Profile = new Profile { Id = Guid.NewGuid(), FirstName = "David", LastName = "Beckham", StartDate = DateTime.UtcNow, DOB = DateTime.UtcNow, IsDeleted = false }
            //                    },
            //                },
            //            },
            //            //Add QA Reports
            //            QAReports = new List<QAReport>(){
            //                new QAReport { Id = Guid.NewGuid(), CreateAt = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1), QAReportStatus = QAReportStatus.New, IsDeleted = false },
            //            },
            //        },
            //    },
            //};
            //context.Client.Add(clientGBS);
            //#endregion


            //#region Frequency
            //CommonData.Set(context);
            //#endregion

            //#region Host Setting

            //var hostSettings = new List<HostSetting>()
            //{
            //    new HostSetting
            //    {
            //        Id = 1,
            //        SettingName = "SMTP_AccountId",
            //        SettingValue = "1",
            //        IsSecure = HostSettingStatus.IsSecure,
            //        IsDeleted = false
            //    },
            //    new HostSetting
            //    {
            //        Id = 2,
            //        SettingName = "SMTP_Authentication",
            //        SettingValue = "1",
            //        IsSecure = HostSettingStatus.IsSecure,
            //        IsDeleted = false
            //    },
            //    new HostSetting
            //    {
            //        Id = 3,
            //        SettingName = "SMTPEnableSSL",
            //        SettingValue = "True",
            //        IsSecure = HostSettingStatus.IsSecure,
            //        IsDeleted = false
            //    }
            //};
            //context.HostSetting.AddRange(hostSettings);

            //#endregion

            //#region Mail Server Provider

            //var mailServerProviders = new List<MailServerProvider>() {
            //    new MailServerProvider
            //    {
            //        Id=Convert.ToInt32(MailServerProviderOption.Common),
            //        MailServerProviderName= "TMA Mail Provider",
            //        MailServerProtocol="POP3",
            //        IncomingMailServerHost ="pop3.tma.com.vn",
            //        IncomingMailServerPort = 995,
            //        OutgoingMailServerHost ="smtp.tma.com.vn",
            //        OutgoingMailServerPort = 25,
            //        Ssl = "0",
            //        Tls = "0"
            //    },
            //    new MailServerProvider
            //    {
            //        Id=Convert.ToInt32(MailServerProviderOption.Google),
            //        MailServerProviderName= "Google",
            //        MailServerProtocol="POP3",
            //        IncomingMailServerHost ="smtp.gmail.com",
            //        IncomingMailServerPort = 587,
            //        OutgoingMailServerHost ="smtp.gmail.com",
            //        OutgoingMailServerPort = 587,
            //        Ssl = "0",
            //        Tls = "0"
            //    },
            //    new MailServerProvider
            //    {
            //        Id=Convert.ToInt32(MailServerProviderOption.Yahoo),
            //        MailServerProviderName= "Yahoo",
            //        MailServerProtocol="POP3",
            //        IncomingMailServerHost ="pop.mail.yahoo.com",
            //        IncomingMailServerPort = 465,
            //        OutgoingMailServerHost ="smtp.mail.yahoo.com",
            //        OutgoingMailServerPort = 995,
            //        Ssl = "0",
            //        Tls = "0"
            //    },
            //    new MailServerProvider
            //    {
            //        Id=Convert.ToInt32(MailServerProviderOption.Msn),
            //        MailServerProviderName= "MSN",
            //        MailServerProtocol="POP3",
            //        IncomingMailServerHost ="pop3.email.msn.com",
            //        IncomingMailServerPort = 110,
            //        OutgoingMailServerHost ="smtp.email.msn.com",
            //        OutgoingMailServerPort = 110,
            //        Ssl = "0",
            //        Tls = "0"
            //    },
            //    new MailServerProvider
            //    {
            //        Id=Convert.ToInt32(MailServerProviderOption.Lycos),
            //        MailServerProviderName= "Lycos",
            //        MailServerProtocol="POP3",
            //        IncomingMailServerHost ="pop.mail.lycos.com",
            //        IncomingMailServerPort = 110,
            //        OutgoingMailServerHost ="smtp.mail.lycos.com",
            //        OutgoingMailServerPort = 110,
            //        Ssl = "0",
            //        Tls = "0"
            //    },
            //    new MailServerProvider
            //    {
            //        Id=Convert.ToInt32(MailServerProviderOption.Aol),
            //        MailServerProviderName= "AOL",
            //        MailServerProtocol="POP3",
            //        IncomingMailServerHost ="imap.aol.com",
            //        IncomingMailServerPort = 143,
            //        OutgoingMailServerHost ="smtp.aol.com",
            //        OutgoingMailServerPort = 143,
            //        Ssl = "0",
            //        Tls = "0"
            //    },
            //    new MailServerProvider
            //    {
            //        Id=Convert.ToInt32(MailServerProviderOption.MailCom),
            //        MailServerProviderName= "Mail.com",
            //        MailServerProtocol="POP3",
            //        IncomingMailServerHost ="pop1.mail.com",
            //        IncomingMailServerPort = 110,
            //        OutgoingMailServerHost ="pop1.mail.com",
            //        OutgoingMailServerPort = 110,
            //        Ssl = "0",
            //        Tls = "0"
            //    },
            //    new MailServerProvider

            //    {
            //        Id=Convert.ToInt32(MailServerProviderOption.Netscape),
            //        MailServerProviderName= "Netscape Internet Service",
            //        MailServerProtocol="POP3",
            //        IncomingMailServerHost ="pop.3.isp.netscape.com",
            //        IncomingMailServerPort = 110,
            //        OutgoingMailServerHost ="smtp.isp.netscape.com",
            //        OutgoingMailServerPort = 25,
            //        Ssl = "0",
            //        Tls = "0"
            //    },
            //    new MailServerProvider
            //    {
            //        Id=Convert.ToInt32(MailServerProviderOption.Tiscali),
            //        MailServerProviderName= "Tiscali",
            //        MailServerProtocol="POP3",
            //        IncomingMailServerHost ="pop.tiscali.com",
            //        IncomingMailServerPort = 110,
            //        OutgoingMailServerHost ="pop.tiscali.com",
            //        OutgoingMailServerPort = 110,
            //        Ssl = "0",
            //        Tls = "0"
            //    },
            //    new MailServerProvider
            //    {
            //        Id=Convert.ToInt32(MailServerProviderOption.Freeserve),
            //        MailServerProviderName= "Freeserve",
            //        MailServerProtocol="POP3",
            //        IncomingMailServerHost ="pop.freeserve.com",
            //        IncomingMailServerPort = 110,
            //        OutgoingMailServerHost ="pop.freeserve.com",
            //        OutgoingMailServerPort = 110,
            //        Ssl = "0",
            //        Tls = "0"
            //    },
            //    new MailServerProvider
            //    {
            //        Id=Convert.ToInt32(MailServerProviderOption.Supanet),
            //        MailServerProviderName= "Supanet",
            //        MailServerProtocol="POP3",
            //        IncomingMailServerHost ="pop.supanet.com",
            //        IncomingMailServerPort = 110,
            //        OutgoingMailServerHost ="pop.supanet.com",
            //        OutgoingMailServerPort = 110,
            //        Ssl = "0",
            //        Tls = "0"
            //    },
            //    new MailServerProvider
            //    {
            //        Id=Convert.ToInt32(MailServerProviderOption.WindowLive),
            //        MailServerProviderName= "Window Live Outlook",
            //        MailServerProtocol="POP3",
            //        IncomingMailServerHost ="pop3.live.com",
            //        IncomingMailServerPort = 995,
            //        OutgoingMailServerHost ="smtp.live.com",
            //        OutgoingMailServerPort = 587,
            //        Ssl = "0",
            //        Tls = "0"
            //    },
            //};
            //context.MailServerProvider.AddRange(mailServerProviders);

            //#endregion

            //#region Mail Account

            //var mailAccounts = new List<MailAccount>()
            //{
            //    new MailAccount
            //    {
            //        Id = 1,
            //        MailServerProviderId = Convert.ToInt32(MailServerProviderOption.Common),
            //        SenderName = "Interactive",
            //        ContactName = "Interactive",
            //        MailAddress = "interactive@tma.com.vn",
            //        Password = "",
            //        Signature = "Interactive",
            //        IsDeleted = false
            //    },
            //    new MailAccount
            //    {
            //        Id = 2,
            //        MailServerProviderId = Convert.ToInt32(MailServerProviderOption.Google),
            //        SenderName = "Minh Nguyen",
            //        ContactName = "Minh Nguyen",
            //        MailAddress = "phiung1983@gmail.com",
            //        Password = "12345678x@X",
            //        Signature = "Minh Nguyen",
            //        IsDeleted = false
            //    }
            //};
            //context.MailAccount.AddRange(mailAccounts);

            //#endregion

            //#region Message Category
            //var messageCategories = new List<MessageCategory>()
            //{
            //    new MessageCategory
            //    {
            //        Id = Convert.ToInt32(MessageCategoryOption.ServiceRequest),
            //        ParentId = 0,
            //        CategoryName =MessageCategoryOption.ServiceRequest.GetDisplayAttributeValues().Name,
            //        Description ="Service Request",
            //        SortKey=1,
            //        Depth=1,
            //        Status= MessageCategoryStatus.Active,
            //        IsDeleted = false,
            //        CreatedDate = DateTime.UtcNow
            //    },
            //    new MessageCategory
            //    {
            //        Id = Convert.ToInt32(MessageCategoryOption.VariationRequest),
            //        ParentId = 0,
            //        CategoryName =MessageCategoryOption.VariationRequest.GetDisplayAttributeValues().Name,
            //        Description ="Variation Request",
            //        SortKey=1,
            //        Depth=1,
            //        Status= MessageCategoryStatus.Active,
            //        IsDeleted = false,
            //        CreatedDate = DateTime.UtcNow
            //    },
            //};
            //context.MessageCategory.AddRange(messageCategories);
            //#endregion

            //#region Message Type

            //var messageTypes = new List<MessageType>()
            //{
            //    new MessageType
            //    {
            //        Id = 1,
            //        Description =  NotificationType.EMAIL.GetDisplayAttributeValues().Name,
            //        IsDeleted = false
            //    },
            //    new MessageType
            //    {
            //        Id = 2,
            //        Description =  NotificationType.SMS.GetDisplayAttributeValues().Name,
            //        IsDeleted = false
            //    },
            //    new MessageType
            //    {
            //        Id = 3,
            //        Description =  NotificationType.PN.GetDisplayAttributeValues().Name,
            //        IsDeleted = false
            //    },
            //    new MessageType
            //    {
            //        Id = 4,
            //        Description =  NotificationType.INTERNAL.GetDisplayAttributeValues().Name,
            //        IsDeleted = false
            //    },
            //};
            //context.MessageType.AddRange(messageTypes);

            //#endregion

            //#region Message Template
            //var messageTemplates = new List<MessageTemplate>()
            //{
            //    new MessageTemplate
            //    {
            //        Id = Convert.ToInt32(BaseConstants.EMAIL_TEMPLATE_SERVICE_REQUEST_CREATE),
            //        TemplateName = "Service Request Creation",
            //        TemplateSubject = "Service Request Creation",
            //        TemplateBody ="Dear ##FullName##,<br>"
            //        + "<br>"
            //        + "Thank you for submitting a Service Request, this is an automated response confirming the receipt of your request.One of our support members will process your request as soon as possible.<br>"
            //        + "<br>"
            //        + "For your records, the details of the ticket are listed below.<br>"
            //         + "<br>"
            //        + "<strong>SR Form Number:</strong> ##ReferenceNo##<br>"
            //        + "<strong>Client Name:</strong> ##ClientName##<br>"
            //        + "<strong>Site Name:</strong> ##SiteName##<br>"
            //        + "<strong>Priority:</strong> ##Priority##<br>"
            //        + "<strong>Customer Reference Number:</strong> ##CustomerRef##<br>"
            //        + "<strong>Priority:</strong> ##Priority##<br>"
            //        + "<strong>Subject:</strong> ##Subject##<br>"
            //        + "<strong>Comments:</strong> ##Comment##<br>"
            //        +"<br>"
            //        + "<strong>Date:</strong> ##Date##<br>"
            //        + "<strong>Time:</strong> ##Time##<br>"
            //        + "<strong>Status:</strong> ##Status##<br>"
            //        + "<br>"
            //        + "You can track the current status of this ticket here ##Link##,<br>"
            //        + "<br>"
            //        + "Best Wishes,<br>"
            //        + "Shining Knight Support<br>",
            //        TemplateDescription="Service Request Creation",
            //        InActive = true,
            //        IsDeleted = false,
            //        CreatedDate = DateTime.UtcNow
            //    },
            //    new MessageTemplate
            //    {
            //        Id = Convert.ToInt32(BaseConstants.EMAIL_TEMPLATE_SERVICE_REQUEST_UPDATE),
            //        TemplateName =  "Service Request Status Change",
            //        TemplateSubject = "Service Request Status Change",
            //        TemplateBody ="Dear ##FullName##,<br>"
            //        + "<br>"
            //        + "This is an automated update to inform you of a status change to your Service Request.<br>"
            //        + "<br>"
            //        + "For your records, the details of the ticket are listed below.<br>"
            //         + "<br>"
            //        + "<strong>SR Form Number:</strong> ##ReferenceNo##<br>"
            //        + "<strong>Client Name:</strong> ##ClientName##<br>"
            //        + "<strong>Site Name:</strong> ##SiteName##<br>"
            //        + "<strong>Priority:</strong> ##Priority##<br>"
            //        + "<strong>Customer Reference Number:</strong> ##CustomerRef##<br>"
            //        + "<strong>Priority:</strong> ##Priority##<br>"
            //        + "<strong>Subject:</strong> ##Subject##<br>"
            //        + "<strong>Comments:</strong> ##Comment##<br>"
            //        +"<br>"
            //        + "<strong>Date:</strong> ##Date##<br>"
            //        + "<strong>Time:</strong> ##Time##<br>"
            //        + "<strong>Status:</strong> ##Status##<br>"
            //        + "<br>"
            //        + "You can track the current status of this ticket here ##Link##,<br>"
            //        + "<br>"
            //        + "Best Wishes,<br>"
            //        + "Shining Knight Support<br>",
            //        TemplateDescription="Service Request Status Change",
            //        InActive = true,
            //        IsDeleted = false,
            //        CreatedDate = DateTime.UtcNow
            //    },
            //    new MessageTemplate
            //    {
            //        Id = Convert.ToInt32(BaseConstants.EMAIL_TEMPLATE_SERVICE_REQUEST_CANCEL),
            //        TemplateName =  "Service Request Cancellation",
            //        TemplateSubject = "Service Request Cancellation",
            //        TemplateBody ="Dear ##FullName##,<br>"
            //        + "<br>"
            //        + "This is an automated update confirming cancellation of your Service Request.<br>"
            //        + "<br>"
            //        + "For your records, the details of the ticket are listed below.<br>"
            //         + "<br>"
            //        + "<strong>SR Form Number:</strong> ##ReferenceNo##<br>"
            //        + "<strong>Client Name:</strong> ##ClientName##<br>"
            //        + "<strong>Site Name:</strong> ##SiteName##<br>"
            //        + "<strong>Priority:</strong> ##Priority##<br>"
            //        + "<strong>Customer Reference Number:</strong> ##CustomerRef##<br>"
            //        + "<strong>Priority:</strong> ##Priority##<br>"
            //        + "<strong>Subject:</strong> ##Subject##<br>"
            //        + "<strong>Comments:</strong> ##Comment##<br>"
            //        +"<br>"
            //        + "<strong>Date:</strong> ##Date##<br>"
            //        + "<strong>Time:</strong> ##Time##<br>"
            //        + "<strong>Status:</strong> ##Status##<br>"
            //        + "<br>"
            //        + "You can track the current status of this ticket here ##Link##,<br>"
            //        + "<br>"
            //        + "Best Wishes,<br>"
            //        + "Shining Knight Support<br>",
            //        TemplateDescription="Service Request Status Change",
            //        InActive = true,
            //        IsDeleted = false,
            //        CreatedDate = DateTime.UtcNow
            //    },
            //    new MessageTemplate
            //    {
            //        Id = Convert.ToInt32(BaseConstants.EMAIL_TEMPLATE_QA_INSPECTION_SUBMIT),
            //        TemplateName = "QA Report Submit",
            //        TemplateSubject = "QA Report Submit",
            //        TemplateBody = "<html><body style=\"width:1000px;\">"
            //        + "Dear ##UserName##,"
            //        + "<br />"
            //        +"This is an automated update notifying you that there are completed QA Report/s available to view below."
            //        + "<br />"
            //        + "<br />"
            //        + "##EmbedQAReport##"
            //        + "<br />"
            //        + "<br />"
            //        + "Best Wishes,"
            //        + "Shining Knight Support"
            //        + "<br />"
            //        + "</body></html>",
            //        TemplateDescription="Service Request Status Change",
            //        InActive = true,
            //        IsDeleted = false,
            //        CreatedDate = DateTime.UtcNow
            //    },
            //};

            //context.MessageTemplate.AddRange(messageTemplates);
            
        }
    }
}
