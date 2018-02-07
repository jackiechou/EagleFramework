using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Settings
{
    public enum NotificationTypeSetting
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "System", Description = "System", Order = 0)]
        Sytem = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "Order", Description = "Order", Order = 1)]
        Order = 2,
        [Display(ResourceType = typeof(LanguageResource), Name = "Account", Description = "Account", Order = 2)]
        Account = 3,
        [Display(ResourceType = typeof(LanguageResource), Name = "CreateAccount", Description = "CreateAccount", Order = 3)]
        CreateAccount = 4,
        [Display(ResourceType = typeof(LanguageResource), Name = "UpdatePassword", Description = "UpdatePassword", Order = 4)]
        UpdatePassword = 5,
        [Display(ResourceType = typeof(LanguageResource), Name = "Service", Description = "Service", Order = 5)]
        Service = 6,
        [Display(ResourceType = typeof(LanguageResource), Name = "BookingService", Description = "BookingService", Order = 6)]
        BookingService = 7,
        [Display(ResourceType = typeof(LanguageResource), Name = "ChangeServiceStatus", Description = "ChangeServiceStatus", Order = 7)]
        ChangeBookingServiceStatus = 8,
        [Display(ResourceType = typeof(LanguageResource), Name = "Event", Description = "Event", Order = 8)]
        Event = 9,
        [Display(ResourceType = typeof(LanguageResource), Name = "CreateEvent", Description = "CreateEvent", Order = 9)]
        CreateEvent = 10,
        [Display(ResourceType = typeof(LanguageResource), Name = "ChangeEventStatus", Description = "ChangeEventStatus", Order = 10)]
        ChangeEventStatus = 11,
        [Display(ResourceType = typeof(LanguageResource), Name = "CreateOrder", Description = "CreateOrder", Order = 11)]
        CreateOrder = 12,
        [Display(ResourceType = typeof(LanguageResource), Name = "CustomerCare", Description = "CustomerCare", Order = 12)]
        CustomerCare = 13,
        [Display(ResourceType = typeof(LanguageResource), Name = "CreateFeedback", Description = "CreateFeedback", Order = 13)]
        CreateFeedback = 14,
        [Display(ResourceType = typeof(LanguageResource), Name = "BookingOrder", Description = "BookingOrder", Order = 14)]
        BookingOrder = 15,
        [Display(ResourceType = typeof(LanguageResource), Name = "OrderComplete", Description = "OrderComplete", Order = 15)]
        OrderComplete = 1013,
    }
}
