using System;
using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Contents.Feedbacks
{
    public class FeedbackDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "FeedbackId")]
        public int FeedbackId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SenderName")]
        public string SenderName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Email")]
        public string Email { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Mobile")]
        public string Mobile { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Subject")]
        public string Subject { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Body")]
        public string Body { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsReplied")]
        public bool IsReplied { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public bool IsActive { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CreatedDate")]
        public DateTime? CreatedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LastModifiedDate")]
        public DateTime? LastModifiedDate { get; set; }
    }
    public class FeedbackEntry : DtoBase
    {
        public FeedbackEntry()
        {
            IsReplied = false;
        }

        [Display(ResourceType = typeof(LanguageResource), Name = "SenderName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string SenderName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Email")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(150, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        [DataType(DataType.EmailAddress, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "InvalidEmail")]
        [EmailAddress(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "InvalidEmail")]
        public string Email { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Mobile")]
        [MaxLength(15, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Mobile { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Subject")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string Subject { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Body")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(500, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string Body { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsReplied")]
        public bool IsReplied { get; set; }
    }
    public class FeedbackEditEntry : FeedbackEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "FeedbackId")]
        public int FeedbackId { get; set; }
    }
}
