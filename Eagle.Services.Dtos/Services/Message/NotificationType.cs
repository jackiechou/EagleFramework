using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Services.Message
{
    [Flags]
    public enum NotificationTypeModel
    {
        NOTIFICATION_ACCOUNT_CREATED = 1,
    }
    public class NotificationTypeEntry : DtoBase
    {
        public NotificationTypeEntry()
        {
            ParentId = 0;
            Status = NotificationTypeStatus.Active;
        }

        [Display(ResourceType = typeof(LanguageResource), Name = "CategoryName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string NotificationTypeName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ParentId")]
        public int? ParentId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public NotificationTypeStatus Status { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SelectedNotificationTargetTypes")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public NotificationTargetType[] SelectedNotificationTargetTypes { get; set; }


        [Display(ResourceType = typeof(LanguageResource), Name = "MessageTypes")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int[] MessageTypeIds { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "NotificationSenderId")]
        public NotificationSenderType NotificationSenderTypeId { get; set; }
    }
    public class NotificationTypeEditEntry : NotificationTypeEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "NotificationTypeId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int NotificationTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MessageTypes")]
        public List<SelectListItem> MessageTypes { get; set; }
    }
    public class NotificationTypeDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "NotificationTypeId")]
        public int NotificationTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "NotificationTypeName")]
        public string NotificationTypeName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ParentId")]
        public int? ParentId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Depth")]
        public string Depth { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Lineage")]
        public string Lineage { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "HasChild")]
        public bool? HasChild { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ListOrder")]
        public int? ListOrder { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public NotificationTypeStatus Status { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MessageTypeId")]
        public int MessageTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "NotificationSenderTypeId")]
        public NotificationSenderType NotificationSenderTypeId { get; set; }

    }
}
