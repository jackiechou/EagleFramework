using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Eagle.Resources;
using Eagle.Services.Dtos.Common;

namespace Eagle.Services.Dtos.Services.Message
{
    public class MessageTemplateEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "MessageTypeId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int MessageTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "NotificationTypeId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int NotificationTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TemplateName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string TemplateName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TemplateSubject")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string TemplateSubject { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "TemplateBody")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string TemplateBody { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "TemplateBody")]
        public string TemplateCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public bool Status { get; set; }
    }
    public class MessageTemplateEditEntry : MessageTemplateEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TemplateId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "InvalidTemplateId")]
        public int TemplateId { get; set; }
    }
    public class MessageTemplateSearchEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "SearchText")]
        public string SearchText { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ComposerId")]
        public int? TypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public bool? IsActive { get; set; }
    }
    public class MessageTemplateDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "MessageTypeId")]
        public int MessageTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "NotificationTypeId")]
        public int NotificationTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TemplateId")]
        public int TemplateId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TemplateName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string TemplateName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TemplateSubject")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string TemplateSubject { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TemplateBody")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string TemplateBody { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TemplateCode")]
        public string TemplateCode { get; set; }


        [Display(ResourceType = typeof(LanguageResource), Name = "CreatedDate")]
        public DateTime? CreatedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LastModifiedDate")]
        public DateTime? LastModifiedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public bool Status { get; set; }
    }
    public class MessageTemplateInfoDetail : MessageTemplateDetail
    {
        public MessageTypeDetail MessageType { get; set; }
    }
    public class MessageTemplateDetailItemResult : ItemResult<MessageTemplateDetail>
    {
        public MessageTemplateDetail MessageTemplateDetail
        {
            get { return Data; }
            set { Data = value; }
        }
    }
    public class MessageTemplateDetailListResult : ListResult<MessageTemplateDetail, MessageTemplateDetailItemResult>
    {
        public IEnumerable<MessageTemplateDetailItemResult> MessageTemplateDetailResults
        {
            get { return Data; }
            set { Data = value; }
        }
    }
}
