using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Business
{
    public class ProductCommentSearchEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "CommentEmail")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string CommentEmail { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PostedDate")]
        public DateTime? PostedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public ProductCommentStatus? IsActive { get; set; }
    }
    public class ProductCommentDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "CommentId")]
        public int CommentId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ProductId")]
        public int ProductId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CommentName")]
        public string CommentName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Email")]
        public string CommentEmail { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Mobile")]
        public string CommentMobile { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CommentText")]
        public string CommentText { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsReplied")]
        public bool? IsReplied { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public ProductCommentStatus IsActive { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CreatedDate")]
        public DateTime? CreatedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CreatedDate")]
        public DateTime? LastModifiedDate { get; set; }
    }

    public class ProductCommentEntry : DtoBase
    {
        public ProductCommentEntry()
        {
            IsReplied = false;
        }

        [Display(ResourceType = typeof(LanguageResource), Name = "ProductId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int ProductId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Subject")]
        [StringLength(255, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string CommentName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Mobile")]
        [StringLength(20, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string CommentMobile { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Email")]
        [StringLength(150, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        [DataType(DataType.EmailAddress, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "InvalidEmail")]
        [EmailAddress(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "InvalidEmail")]
        public string CommentEmail { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(LanguageResource), Name = "Body")]
        public string CommentText { get; set; }

        public bool IsReplied { get; set; }
    }

    public class ProductCommentEditEntry : ProductCommentEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "CommentId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int CommentId { get; set; }
    }


}
