using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Contents.Banners
{
    public class BannerPositionDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PositionId", Description = "PositionId")]
        public int PositionId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PositionName", Description = "PositionName")]
        public string PositionName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Description", Description = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ListOrder", Description = "ListOrder")]
        public int ListOrder { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status", Description = "Status")]
        public BannerPositionStatus Status { get; set; }
    }
    public class BannerPositionEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PositionName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string PositionName { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        [StringLength(500, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Description { get; set; }

        //[EnumDataType(typeof(BannerPositionStatus))]
        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public BannerPositionStatus Status { get; set; }
    }

    public class BannerPositionEditEntry : BannerPositionEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PositionId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int PositionId { get; set; }
    }
}
