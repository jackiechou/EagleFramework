namespace ShiningKnight.Build.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Account",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserType = c.Int(nullable: false),
                        ProfileType = c.Int(nullable: false),
                        EmployeeId = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        ProfileId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Profile", t => t.ProfileId, cascadeDelete: true)
                .Index(t => t.ProfileId);
            
            CreateTable(
                "dbo.MailGroup",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        MailGroupName = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Notification",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Type = c.Int(nullable: false),
                        ReferenceObjectId = c.Guid(),
                        SendDate = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        MessageQueueId = c.Guid(nullable: false),
                        FromAccountId = c.Guid(),
                        ToAccountId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Account", t => t.FromAccountId)
                .ForeignKey("dbo.MessageQueue", t => t.MessageQueueId, cascadeDelete: true)
                .ForeignKey("dbo.Account", t => t.ToAccountId)
                .Index(t => t.MessageQueueId)
                .Index(t => t.FromAccountId)
                .Index(t => t.ToAccountId);
            
            CreateTable(
                "dbo.MessageQueue",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        From = c.String(),
                        To = c.String(),
                        Subject = c.String(),
                        Bcc = c.String(),
                        Cc = c.String(),
                        Body = c.String(),
                        PredefinedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Profile",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FirstName = c.String(maxLength: 100),
                        LastName = c.String(maxLength: 100),
                        Email = c.String(),
                        Phone = c.String(maxLength: 12),
                        Mobile = c.String(maxLength: 12),
                        StartDate = c.DateTime(),
                        DOB = c.DateTime(),
                        PhotoUrl = c.String(),
                        JobTitle = c.String(maxLength: 256),
                        IsDeleted = c.Boolean(nullable: false),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AppClaim",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Key = c.String(maxLength: 500),
                        Value = c.String(maxLength: 500),
                        IsDeleted = c.Boolean(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.FunctionCommand",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 256),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RoleGroup",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(maxLength: 256),
                        Description = c.String(maxLength: 500),
                        IsDeleted = c.Boolean(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.Cleaner",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        SecurityCheck = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        AccountId = c.Guid(nullable: false),
                        ClientSiteId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Account", t => t.AccountId, cascadeDelete: true)
                .ForeignKey("dbo.ClientSite", t => t.ClientSiteId)
                .Index(t => t.AccountId)
                .Index(t => t.ClientSiteId);
            
            CreateTable(
                "dbo.ClientSite",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        SiteId = c.Int(nullable: false, identity: true),
                        SiteName = c.String(),
                        SiteLocation = c.String(),
                        Street = c.String(),
                        Suburb = c.String(),
                        Postcode = c.String(),
                        StateCode = c.String(),
                        Frequency = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        KPI = c.Single(nullable: false),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        ClientId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Client", t => t.ClientId, cascadeDelete: true)
                .Index(t => t.ClientId);
            
            CreateTable(
                "dbo.Client",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ClientId = c.Int(nullable: false, identity: true),
                        ClientName = c.String(maxLength: 256),
                        HeadLocation = c.String(maxLength: 256),
                        ContactEmail = c.String(),
                        ContactPhone = c.String(maxLength: 12),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ClientAdmin",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AccountId = c.Guid(nullable: false),
                        ClientId = c.Guid(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Account", t => t.AccountId, cascadeDelete: true)
                .ForeignKey("dbo.Client", t => t.ClientId, cascadeDelete: true)
                .Index(t => t.AccountId)
                .Index(t => t.ClientId);
            
            CreateTable(
                "dbo.QAReport",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        QAFormNo = c.String(maxLength: 100),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        KPI = c.Int(nullable: false),
                        AuditCompletedId = c.Guid(),
                        AuditSignId = c.Guid(),
                        CreateAt = c.DateTime(nullable: false),
                        QAReportStatus = c.Int(nullable: false),
                        QAReportGeneratorStatus = c.Int(nullable: false),
                        ClientSiteId = c.Guid(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QAAudit", t => t.AuditCompletedId)
                .ForeignKey("dbo.QAAudit", t => t.AuditSignId)
                .ForeignKey("dbo.ClientSite", t => t.ClientSiteId, cascadeDelete: true)
                .Index(t => t.AuditCompletedId)
                .Index(t => t.AuditSignId)
                .Index(t => t.ClientSiteId);
            
            CreateTable(
                "dbo.QAAudit",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AuditBy = c.Guid(nullable: false),
                        AuditType = c.Int(nullable: false),
                        AuditTime = c.DateTime(nullable: false),
                        Note = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SiteAdmin", t => t.AuditBy, cascadeDelete: true)
                .Index(t => t.AuditBy);
            
            CreateTable(
                "dbo.SiteAdmin",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AccountId = c.Guid(nullable: false),
                        ClientSiteId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Account", t => t.AccountId, cascadeDelete: true)
                .ForeignKey("dbo.ClientSite", t => t.ClientSiteId, cascadeDelete: true)
                .Index(t => t.AccountId)
                .Index(t => t.ClientSiteId);
            
            CreateTable(
                "dbo.ReportTask",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        QAReportId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QAReport", t => t.QAReportId, cascadeDelete: true)
                .Index(t => t.QAReportId);
            
            CreateTable(
                "dbo.TaskAssessment",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Score = c.Int(),
                        Comment = c.String(maxLength: 200),
                        ReportTaskId = c.Guid(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ReportTask", t => t.ReportTaskId, cascadeDelete: true)
                .Index(t => t.ReportTaskId);
            
            CreateTable(
                "dbo.TaskAttachment",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Url = c.String(),
                        TaskAssessmentId = c.Guid(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TaskAssessment", t => t.TaskAssessmentId, cascadeDelete: true)
                .Index(t => t.TaskAssessmentId);
            
            CreateTable(
                "dbo.SiteTask",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        ClientSiteId = c.Guid(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClientSite", t => t.ClientSiteId, cascadeDelete: true)
                .Index(t => t.ClientSiteId);
            
            CreateTable(
                "dbo.Task",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        SiteTaskId = c.Guid(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SiteTask", t => t.SiteTaskId, cascadeDelete: true)
                .Index(t => t.SiteTaskId);
            
            CreateTable(
                "dbo.CRM",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ManagerId = c.Guid(nullable: false),
                        AccountId = c.Guid(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Account", t => t.AccountId, cascadeDelete: true)
                .ForeignKey("dbo.Manager", t => t.ManagerId, cascadeDelete: true)
                .Index(t => t.ManagerId)
                .Index(t => t.AccountId);
            
            CreateTable(
                "dbo.Manager",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        Account_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Account", t => t.Account_Id)
                .Index(t => t.Account_Id);
            
            CreateTable(
                "dbo.Frequency",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        NumberOfDay = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.HostSetting",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        SettingName = c.String(),
                        SettingValue = c.String(),
                        IsSecure = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MailAccount",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        MailServerProviderId = c.Int(nullable: false),
                        SenderName = c.String(),
                        ContactName = c.String(),
                        MailAddress = c.String(),
                        Password = c.String(),
                        Signature = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MailServerProvider", t => t.MailServerProviderId, cascadeDelete: true)
                .Index(t => t.MailServerProviderId);
            
            CreateTable(
                "dbo.MailServerProvider",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        MailServerProviderName = c.String(),
                        MailServerProtocol = c.String(),
                        IncomingMailServerHost = c.String(),
                        IncomingMailServerPort = c.Int(),
                        OutgoingMailServerHost = c.String(),
                        OutgoingMailServerPort = c.Int(),
                        Ssl = c.String(),
                        Tls = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MailMessage",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FromAccountId = c.Guid(),
                        ToAccountId = c.Guid(),
                        Subject = c.String(),
                        Body = c.String(),
                        Type = c.Int(nullable: false),
                        SentDate = c.DateTime(nullable: false),
                        ReadDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Account", t => t.FromAccountId)
                .ForeignKey("dbo.Account", t => t.ToAccountId)
                .Index(t => t.FromAccountId)
                .Index(t => t.ToAccountId);
            
            CreateTable(
                "dbo.QAReportNotification",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        QAReportId = c.Guid(nullable: false),
                        MailMessageId = c.Guid(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MailMessage", t => t.MailMessageId, cascadeDelete: true)
                .ForeignKey("dbo.QAReport", t => t.QAReportId, cascadeDelete: true)
                .Index(t => t.QAReportId)
                .Index(t => t.MailMessageId);
            
            CreateTable(
                "dbo.SRNotification",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ServiceRequestId = c.Guid(nullable: false),
                        MailMessageId = c.Guid(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MailMessage", t => t.MailMessageId, cascadeDelete: true)
                .ForeignKey("dbo.ServiceRequest", t => t.ServiceRequestId, cascadeDelete: true)
                .Index(t => t.ServiceRequestId)
                .Index(t => t.MailMessageId);
            
            CreateTable(
                "dbo.ServiceRequest",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ServiceRequestId = c.Int(nullable: false, identity: true),
                        ClientId = c.Guid(nullable: false),
                        SiteId = c.Guid(nullable: false),
                        ScheduleId = c.Guid(nullable: false),
                        Subject = c.String(),
                        CustomerRef = c.String(),
                        Commnent = c.String(),
                        Priority = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClientSite", t => t.SiteId, cascadeDelete: true)
                .ForeignKey("dbo.ScheduleRequest", t => t.ScheduleId, cascadeDelete: true)
                .Index(t => t.SiteId)
                .Index(t => t.ScheduleId);
            
            CreateTable(
                "dbo.ScheduleRequest",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VRNotification",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        VariationRequestId = c.Guid(nullable: false),
                        MailMessageId = c.Guid(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MailMessage", t => t.MailMessageId, cascadeDelete: true)
                .ForeignKey("dbo.VariationRequest", t => t.VariationRequestId, cascadeDelete: true)
                .Index(t => t.VariationRequestId)
                .Index(t => t.MailMessageId);
            
            CreateTable(
                "dbo.VariationRequest",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        VariationRequestId = c.Int(nullable: false, identity: true),
                        AccountId = c.Guid(nullable: false),
                        SiteId = c.Guid(nullable: false),
                        Subject = c.String(),
                        Commnent = c.String(),
                        CustomerRef = c.String(),
                        ScheduleId = c.Guid(nullable: false),
                        Status = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Account", t => t.AccountId, cascadeDelete: true)
                .ForeignKey("dbo.ClientSite", t => t.SiteId, cascadeDelete: true)
                .ForeignKey("dbo.ScheduleRequest", t => t.ScheduleId, cascadeDelete: true)
                .Index(t => t.AccountId)
                .Index(t => t.SiteId)
                .Index(t => t.ScheduleId);
            
            CreateTable(
                "dbo.MessageCategory",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        ParentId = c.Int(nullable: false),
                        CategoryName = c.String(),
                        Description = c.String(),
                        SortKey = c.Int(nullable: false),
                        Depth = c.Int(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        Status = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MessageTemplate",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        TemplateName = c.String(),
                        TemplateSender = c.String(),
                        TemplateFrom = c.String(),
                        TemplateTo = c.String(),
                        TemplateCc = c.String(),
                        TemplateBcc = c.String(),
                        TemplateSubject = c.String(),
                        TemplateBody = c.String(),
                        TemplateDescription = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MessageType",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Description = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NotificationTransmit",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        NotificationId = c.Guid(nullable: false),
                        Status = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        CompleteDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Notification", t => t.NotificationId, cascadeDelete: true)
                .Index(t => t.NotificationId);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.UserRole",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Role", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.SiteTemplates",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        TemplateName = c.String(),
                        DisplayName = c.String(),
                        HighlightColor = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TemplateTasks",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        SiteTemplateId = c.Guid(nullable: false),
                        TaskTitle = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SiteTemplates", t => t.SiteTemplateId, cascadeDelete: true)
                .Index(t => t.SiteTemplateId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Email = c.String(maxLength: 256),
                        FirstName = c.String(nullable: false, maxLength: 100),
                        LastName = c.String(maxLength: 100),
                        ChangePassword = c.Boolean(nullable: false),
                        IsActived = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(maxLength: 256),
                        SecurityStamp = c.String(maxLength: 256),
                        PhoneNumber = c.String(maxLength: 20),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.UserClaim",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(maxLength: 500),
                        ClaimValue = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserLogin",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.NotificationTarget",
                c => new
                    {
                        MailGroupId = c.Guid(nullable: false),
                        NotificationId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.MailGroupId, t.NotificationId })
                .ForeignKey("dbo.Notification", t => t.MailGroupId, cascadeDelete: true)
                .ForeignKey("dbo.MailGroup", t => t.NotificationId, cascadeDelete: true)
                .Index(t => t.MailGroupId)
                .Index(t => t.NotificationId);
            
            CreateTable(
                "dbo.AccountMailGroup",
                c => new
                    {
                        MailGroupId = c.Guid(nullable: false),
                        AccountId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.MailGroupId, t.AccountId })
                .ForeignKey("dbo.Account", t => t.MailGroupId, cascadeDelete: true)
                .ForeignKey("dbo.MailGroup", t => t.AccountId, cascadeDelete: true)
                .Index(t => t.MailGroupId)
                .Index(t => t.AccountId);
            
            CreateTable(
                "dbo.CommandClaim",
                c => new
                    {
                        AppClaimId = c.Guid(nullable: false),
                        FunctionCommandId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.AppClaimId, t.FunctionCommandId })
                .ForeignKey("dbo.AppClaim", t => t.AppClaimId, cascadeDelete: true)
                .ForeignKey("dbo.FunctionCommand", t => t.FunctionCommandId, cascadeDelete: true)
                .Index(t => t.AppClaimId)
                .Index(t => t.FunctionCommandId);
            
            CreateTable(
                "dbo.GroupClaim",
                c => new
                    {
                        RoleGroupId = c.Guid(nullable: false),
                        AppClaimId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoleGroupId, t.AppClaimId })
                .ForeignKey("dbo.RoleGroup", t => t.RoleGroupId, cascadeDelete: true)
                .ForeignKey("dbo.AppClaim", t => t.AppClaimId, cascadeDelete: true)
                .Index(t => t.RoleGroupId)
                .Index(t => t.AppClaimId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRole", "UserId", "dbo.User");
            DropForeignKey("dbo.RoleGroup", "ApplicationUser_Id", "dbo.User");
            DropForeignKey("dbo.UserLogin", "UserId", "dbo.User");
            DropForeignKey("dbo.UserClaim", "UserId", "dbo.User");
            DropForeignKey("dbo.AppClaim", "ApplicationUser_Id", "dbo.User");
            DropForeignKey("dbo.TemplateTasks", "SiteTemplateId", "dbo.SiteTemplates");
            DropForeignKey("dbo.UserRole", "RoleId", "dbo.Role");
            DropForeignKey("dbo.NotificationTransmit", "NotificationId", "dbo.Notification");
            DropForeignKey("dbo.VRNotification", "VariationRequestId", "dbo.VariationRequest");
            DropForeignKey("dbo.VariationRequest", "ScheduleId", "dbo.ScheduleRequest");
            DropForeignKey("dbo.VariationRequest", "SiteId", "dbo.ClientSite");
            DropForeignKey("dbo.VariationRequest", "AccountId", "dbo.Account");
            DropForeignKey("dbo.VRNotification", "MailMessageId", "dbo.MailMessage");
            DropForeignKey("dbo.MailMessage", "ToAccountId", "dbo.Account");
            DropForeignKey("dbo.SRNotification", "ServiceRequestId", "dbo.ServiceRequest");
            DropForeignKey("dbo.ServiceRequest", "ScheduleId", "dbo.ScheduleRequest");
            DropForeignKey("dbo.ServiceRequest", "SiteId", "dbo.ClientSite");
            DropForeignKey("dbo.SRNotification", "MailMessageId", "dbo.MailMessage");
            DropForeignKey("dbo.QAReportNotification", "QAReportId", "dbo.QAReport");
            DropForeignKey("dbo.QAReportNotification", "MailMessageId", "dbo.MailMessage");
            DropForeignKey("dbo.MailMessage", "FromAccountId", "dbo.Account");
            DropForeignKey("dbo.MailAccount", "MailServerProviderId", "dbo.MailServerProvider");
            DropForeignKey("dbo.CRM", "ManagerId", "dbo.Manager");
            DropForeignKey("dbo.Manager", "Account_Id", "dbo.Account");
            DropForeignKey("dbo.CRM", "AccountId", "dbo.Account");
            DropForeignKey("dbo.Task", "SiteTaskId", "dbo.SiteTask");
            DropForeignKey("dbo.SiteTask", "ClientSiteId", "dbo.ClientSite");
            DropForeignKey("dbo.TaskAttachment", "TaskAssessmentId", "dbo.TaskAssessment");
            DropForeignKey("dbo.TaskAssessment", "ReportTaskId", "dbo.ReportTask");
            DropForeignKey("dbo.ReportTask", "QAReportId", "dbo.QAReport");
            DropForeignKey("dbo.QAReport", "ClientSiteId", "dbo.ClientSite");
            DropForeignKey("dbo.QAReport", "AuditSignId", "dbo.QAAudit");
            DropForeignKey("dbo.QAReport", "AuditCompletedId", "dbo.QAAudit");
            DropForeignKey("dbo.QAAudit", "AuditBy", "dbo.SiteAdmin");
            DropForeignKey("dbo.SiteAdmin", "ClientSiteId", "dbo.ClientSite");
            DropForeignKey("dbo.SiteAdmin", "AccountId", "dbo.Account");
            DropForeignKey("dbo.ClientSite", "ClientId", "dbo.Client");
            DropForeignKey("dbo.ClientAdmin", "ClientId", "dbo.Client");
            DropForeignKey("dbo.ClientAdmin", "AccountId", "dbo.Account");
            DropForeignKey("dbo.Cleaner", "ClientSiteId", "dbo.ClientSite");
            DropForeignKey("dbo.Cleaner", "AccountId", "dbo.Account");
            DropForeignKey("dbo.GroupClaim", "AppClaimId", "dbo.AppClaim");
            DropForeignKey("dbo.GroupClaim", "RoleGroupId", "dbo.RoleGroup");
            DropForeignKey("dbo.CommandClaim", "FunctionCommandId", "dbo.FunctionCommand");
            DropForeignKey("dbo.CommandClaim", "AppClaimId", "dbo.AppClaim");
            DropForeignKey("dbo.Account", "ProfileId", "dbo.Profile");
            DropForeignKey("dbo.AccountMailGroup", "AccountId", "dbo.MailGroup");
            DropForeignKey("dbo.AccountMailGroup", "MailGroupId", "dbo.Account");
            DropForeignKey("dbo.Notification", "ToAccountId", "dbo.Account");
            DropForeignKey("dbo.Notification", "MessageQueueId", "dbo.MessageQueue");
            DropForeignKey("dbo.NotificationTarget", "NotificationId", "dbo.MailGroup");
            DropForeignKey("dbo.NotificationTarget", "MailGroupId", "dbo.Notification");
            DropForeignKey("dbo.Notification", "FromAccountId", "dbo.Account");
            DropIndex("dbo.GroupClaim", new[] { "AppClaimId" });
            DropIndex("dbo.GroupClaim", new[] { "RoleGroupId" });
            DropIndex("dbo.CommandClaim", new[] { "FunctionCommandId" });
            DropIndex("dbo.CommandClaim", new[] { "AppClaimId" });
            DropIndex("dbo.AccountMailGroup", new[] { "AccountId" });
            DropIndex("dbo.AccountMailGroup", new[] { "MailGroupId" });
            DropIndex("dbo.NotificationTarget", new[] { "NotificationId" });
            DropIndex("dbo.NotificationTarget", new[] { "MailGroupId" });
            DropIndex("dbo.UserLogin", new[] { "UserId" });
            DropIndex("dbo.UserClaim", new[] { "UserId" });
            DropIndex("dbo.User", "UserNameIndex");
            DropIndex("dbo.TemplateTasks", new[] { "SiteTemplateId" });
            DropIndex("dbo.UserRole", new[] { "RoleId" });
            DropIndex("dbo.UserRole", new[] { "UserId" });
            DropIndex("dbo.Role", "RoleNameIndex");
            DropIndex("dbo.NotificationTransmit", new[] { "NotificationId" });
            DropIndex("dbo.VariationRequest", new[] { "ScheduleId" });
            DropIndex("dbo.VariationRequest", new[] { "SiteId" });
            DropIndex("dbo.VariationRequest", new[] { "AccountId" });
            DropIndex("dbo.VRNotification", new[] { "MailMessageId" });
            DropIndex("dbo.VRNotification", new[] { "VariationRequestId" });
            DropIndex("dbo.ServiceRequest", new[] { "ScheduleId" });
            DropIndex("dbo.ServiceRequest", new[] { "SiteId" });
            DropIndex("dbo.SRNotification", new[] { "MailMessageId" });
            DropIndex("dbo.SRNotification", new[] { "ServiceRequestId" });
            DropIndex("dbo.QAReportNotification", new[] { "MailMessageId" });
            DropIndex("dbo.QAReportNotification", new[] { "QAReportId" });
            DropIndex("dbo.MailMessage", new[] { "ToAccountId" });
            DropIndex("dbo.MailMessage", new[] { "FromAccountId" });
            DropIndex("dbo.MailAccount", new[] { "MailServerProviderId" });
            DropIndex("dbo.Manager", new[] { "Account_Id" });
            DropIndex("dbo.CRM", new[] { "AccountId" });
            DropIndex("dbo.CRM", new[] { "ManagerId" });
            DropIndex("dbo.Task", new[] { "SiteTaskId" });
            DropIndex("dbo.SiteTask", new[] { "ClientSiteId" });
            DropIndex("dbo.TaskAttachment", new[] { "TaskAssessmentId" });
            DropIndex("dbo.TaskAssessment", new[] { "ReportTaskId" });
            DropIndex("dbo.ReportTask", new[] { "QAReportId" });
            DropIndex("dbo.SiteAdmin", new[] { "ClientSiteId" });
            DropIndex("dbo.SiteAdmin", new[] { "AccountId" });
            DropIndex("dbo.QAAudit", new[] { "AuditBy" });
            DropIndex("dbo.QAReport", new[] { "ClientSiteId" });
            DropIndex("dbo.QAReport", new[] { "AuditSignId" });
            DropIndex("dbo.QAReport", new[] { "AuditCompletedId" });
            DropIndex("dbo.ClientAdmin", new[] { "ClientId" });
            DropIndex("dbo.ClientAdmin", new[] { "AccountId" });
            DropIndex("dbo.ClientSite", new[] { "ClientId" });
            DropIndex("dbo.Cleaner", new[] { "ClientSiteId" });
            DropIndex("dbo.Cleaner", new[] { "AccountId" });
            DropIndex("dbo.RoleGroup", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.AppClaim", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Notification", new[] { "ToAccountId" });
            DropIndex("dbo.Notification", new[] { "FromAccountId" });
            DropIndex("dbo.Notification", new[] { "MessageQueueId" });
            DropIndex("dbo.Account", new[] { "ProfileId" });
            DropTable("dbo.GroupClaim");
            DropTable("dbo.CommandClaim");
            DropTable("dbo.AccountMailGroup");
            DropTable("dbo.NotificationTarget");
            DropTable("dbo.UserLogin");
            DropTable("dbo.UserClaim");
            DropTable("dbo.User");
            DropTable("dbo.TemplateTasks");
            DropTable("dbo.SiteTemplates");
            DropTable("dbo.UserRole");
            DropTable("dbo.Role");
            DropTable("dbo.NotificationTransmit");
            DropTable("dbo.MessageType");
            DropTable("dbo.MessageTemplate");
            DropTable("dbo.MessageCategory");
            DropTable("dbo.VariationRequest");
            DropTable("dbo.VRNotification");
            DropTable("dbo.ScheduleRequest");
            DropTable("dbo.ServiceRequest");
            DropTable("dbo.SRNotification");
            DropTable("dbo.QAReportNotification");
            DropTable("dbo.MailMessage");
            DropTable("dbo.MailServerProvider");
            DropTable("dbo.MailAccount");
            DropTable("dbo.HostSetting");
            DropTable("dbo.Frequency");
            DropTable("dbo.Manager");
            DropTable("dbo.CRM");
            DropTable("dbo.Task");
            DropTable("dbo.SiteTask");
            DropTable("dbo.TaskAttachment");
            DropTable("dbo.TaskAssessment");
            DropTable("dbo.ReportTask");
            DropTable("dbo.SiteAdmin");
            DropTable("dbo.QAAudit");
            DropTable("dbo.QAReport");
            DropTable("dbo.ClientAdmin");
            DropTable("dbo.Client");
            DropTable("dbo.ClientSite");
            DropTable("dbo.Cleaner");
            DropTable("dbo.RoleGroup");
            DropTable("dbo.FunctionCommand");
            DropTable("dbo.AppClaim");
            DropTable("dbo.Profile");
            DropTable("dbo.MessageQueue");
            DropTable("dbo.Notification");
            DropTable("dbo.MailGroup");
            DropTable("dbo.Account");
        }
    }
}
