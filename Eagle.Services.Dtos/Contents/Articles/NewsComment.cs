using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Contents.Articles
{
    public class NewsCommentEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "NewsId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int NewsId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CommentName")]
        [StringLength(150, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string CommentName { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "CommentText")]
        [StringLength(500, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string CommentText { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Email")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(4000, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        [DataType(DataType.EmailAddress, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "InvalidEmail")]
        [EmailAddress(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "InvalidEmail")]
        public string CreatedByEmail { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsReplied")]
        public bool? IsReplied { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsPublished")]
        [EnumDataType(typeof(NewsCommentStatus))]
        public NewsCommentStatus? IsPublished { get; set; }
    }
    public class NewsCommentDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "CommentId")]
        public int CommentId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CommentName")]
        public string CommentName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CommentText")]
        public string CommentText { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CreatedByEmail")]
        public string CreatedByEmail { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsReplied")]
        public bool? IsReplied { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsPublished")]
        public NewsCommentStatus? IsPublished { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "NewsId")]
        public int NewsId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Ip")]
        public string Ip { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CreatedDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CreatedDate { get; set; }
    }
    public class NewsCommentInfoDetail : NewsCommentDetail
    {
        public NewsDetail News { get; set; }
    }
}
