using System.Data.Entity;
using Eagle.Entities.Services.Booking;
using Eagle.Entities.Services.Chat;
using Eagle.Entities.Services.Events;
using Eagle.Entities.Services.Messaging;

namespace Eagle.EntityFramework.EntityMapping
{
    public static class ServiceMap
    {
        public static void Configure(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>().ToTable("Event");
            modelBuilder.Entity<EventType>().ToTable("EventType");

            modelBuilder.Entity<ServicePack>().ToTable("Booking.ServicePack");
            modelBuilder.Entity<ServicePackOption>().ToTable("Booking.ServicePackOption");
            modelBuilder.Entity<ServicePackDuration>().ToTable("Booking.ServicePackDuration");
            modelBuilder.Entity<ServicePackProvider>().ToTable("Booking.ServicePackProvider");
            modelBuilder.Entity<ServicePackRating>().ToTable("Booking.ServicePackRating");
            modelBuilder.Entity<ServicePackType>().ToTable("Booking.ServicePackType");
            modelBuilder.Entity<ServiceDiscount>().ToTable("Booking.ServiceDiscount");
            modelBuilder.Entity<ServiceCategory>().ToTable("Booking.ServiceCategory");
            modelBuilder.Entity<ServicePeriod>().ToTable("Booking.ServicePeriod");
            modelBuilder.Entity<ServiceTaxRate>().ToTable("Booking.ServiceTaxRate");
     
            modelBuilder.Entity<MailServerProvider>().ToTable("Chat.ChatMessage");
            modelBuilder.Entity<ChatPrivateMessage>().ToTable("Chat.ChatPrivateMessage");
            modelBuilder.Entity<ChatPrivateMessageMaster>().ToTable("Chat.ChatPrivateMessageMaster");
            modelBuilder.Entity<ChatUser>().ToTable("Chat.ChatUser");

            modelBuilder.Entity<MailServerProvider>().ToTable("Messaging.MailServerProvider");
            modelBuilder.Entity<MessageQueue>().ToTable("Messaging.MessageQueue");
            modelBuilder.Entity<MessageTemplate>().ToTable("Messaging.MessageTemplate");
            modelBuilder.Entity<MessageType>().ToTable("Messaging.MessageType");
            modelBuilder.Entity<NotificationType>().ToTable("Messaging.NotificationType");
            modelBuilder.Entity<NotificationMessageType>().ToTable("Messaging.NotificationMessageType");
            modelBuilder.Entity<NotificationSender>().ToTable("Messaging.NotificationSender");
            modelBuilder.Entity<NotificationTarget>().ToTable("Messaging.NotificationTarget");
            modelBuilder.Entity<NotificationTargetDefault>().ToTable("Messaging.NotificationTargetDefault");
            modelBuilder.Entity<NotificationMessage>().ToTable("Messaging.NotificationMessage");

            //modelBuilder.Configurations.Add(new MailAccountMap());
            //modelBuilder.Configurations.Add(new MailServerProviderMap());
            //modelBuilder.Configurations.Add(new MessageTemplateMap());
            //modelBuilder.Configurations.Add(new MessageTypeMap());
            //modelBuilder.Configurations.Add(new MailMessageQueueMap());
        }

        //private class MailAccountMap : EntityTypeConfiguration<MailAccount>
        //{
        //    public MailAccountMap()
        //    {
        //        ToTable("MailAccount");
        //        // Primary Key
        //        HasKey(t => t.Id);
        //        HasKey(t => t.MailServerProviderId);
        //        // Setup foreign key to NetworkGroup
        //        HasRequired(t => t.MailServerProvider);
        //        // Properties
        //        Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        //        Property(t => t.SenderName).HasColumnName("SenderName").IsRequired();
        //        Property(t => t.ContactName).HasColumnName("ContactName").IsOptional();
        //        Property(t => t.MailAddress).HasColumnName("MailAddress").IsRequired();
        //        Property(t => t.Password).HasColumnName("Password").IsRequired();
        //        Property(t => t.Signature).HasColumnName("Signature").IsOptional();
        //    }
        //}

        //private class MailServerProviderMap : EntityTypeConfiguration<MailServerProvider>
        //{
        //    public MailServerProviderMap()
        //    {
        //        ToTable("MailServerProvider");
        //        // Primary Key
        //        HasKey(t => t.MailServerProviderId);
        //        // Properties
        //        Property(t => t.MailServerProviderId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        //        Property(t => t.MailServerProviderName).HasColumnName("MailServerProviderName").IsRequired();
        //        Property(t => t.MailServerProtocol).HasColumnName("MailServerProtocol").IsRequired();
        //        Property(t => t.IncomingMailServerHost).HasColumnName("IncomingMailServerHost").IsRequired();
        //        Property(t => t.IncomingMailServerHost).HasColumnName("IncomingMailServerPort").IsRequired();
        //        Property(t => t.IncomingMailServerPort).HasColumnName("OutgoingMailServerHost").IsOptional();
        //        Property(t => t.OutgoingMailServerHost).HasColumnName("OutgoingMailServerPort").IsOptional();
        //        Property(t => t.OutgoingMailServerHost).HasColumnName("Ssl").IsOptional();
        //        Property(t => t.OutgoingMailServerHost).HasColumnName("Tls").IsOptional();
        //    }
        //}

        //private class MessageTemplateMap : EntityTypeConfiguration<MessageTemplate>
        //{
        //    public MessageTemplateMap()
        //    {
        //        ToTable("MailTemplate");
        //        // Primary Key
        //        HasKey(t => t.TemplateId);
        //        // Properties
        //        Property(t => t.TemplateId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        //        Property(t => t.TemplateName).HasColumnName("TemplateName").IsRequired();
        //        Property(t => t.TemplateBody).HasColumnName("TemplateBody").IsRequired();
        //        Property(t => t.Status).HasColumnName("Status").IsOptional();
        //        Property(t => t.MessageTypeId).HasColumnName("MessageTypeId").IsRequired();
        //    }
        //}

        //private class MessageTypeMap : EntityTypeConfiguration<MessageType>
        //{
        //    public MessageTypeMap()
        //    {
        //        ToTable("MailType");
        //        // Primary Key
        //        HasKey(t => t.MessageTypeId);
        //        // Properties
        //        Property(t => t.MessageTypeId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        //        Property(t => t.MessageTypeName).HasColumnName("TypeName").IsRequired();
        //        Property(t => t.Description).HasColumnName("Description").IsOptional();
        //        Property(t => t.Status).HasColumnName("Status").IsRequired();
        //    }
        //}

        //private class MailMessageQueueMap : EntityTypeConfiguration<MailMessageQueue>
        //{
        //    public MailMessageQueueMap()
        //    {
        //        ToTable("MailMessageQueue");
        //        // Primary Key
        //        HasKey(t => t.QueueId);
        //        // Properties
        //        Property(t => t.QueueId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        //        Property(t => t.FromSender).HasColumnName("FromSender").IsRequired();
        //        Property(t => t.ToReceiver).HasColumnName("ToReceiver").IsRequired();
        //        Property(t => t.Subject).HasColumnName("Subject").IsRequired();
        //        Property(t => t.Message).HasColumnName("Message").IsOptional();
        //        Property(t => t.Bcc).HasColumnName("Bcc").IsOptional();
        //        Property(t => t.Cc).HasColumnName("Cc").IsOptional();
        //        Property(t => t.Status).HasColumnName("Status").IsRequired();
        //        Property(t => t.ResponseStatus).HasColumnName("ResponseStatus").IsOptional();
        //        Property(t => t.ResponseMessage).HasColumnName("ResponseMessage").IsOptional();
        //        Property(t => t.PredefinedDate).HasColumnName("PredefinedDate").IsOptional();
        //        Property(t => t.SentDate).HasColumnName("SentDate").IsOptional();
        //    }
        //}
    }
}
