using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Contents.Banners
{
    public class BannerTypeDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "BannerTypeId")]
        public int TypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TypeName")]
        public string TypeName { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public BannerTypeStatus Status { get; set; }
    }

    public class BannerTypeEntry : DtoBase
    {

        [Display(ResourceType = typeof(LanguageResource), Name = "BannerTypeName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "BannerTypeName")]
        [RegularExpression(@"[a-zA-Z0-9_ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ\\s \\\-~!@#$%^&*()_+={}:|""?`;:><',./[\]]+", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "InvalidTitle")]
        [StringLength(100, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string TypeName { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        //[StringLength(int.MaxValue, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Description { get; set; }

        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "BannerTypeStatus")]
        [Display(ResourceType = typeof(LanguageResource), Name = "BannerTypeStatus")]
        public BannerTypeStatus Status { get; set; }
    }
    public class BannerTypeEditEntry : BannerTypeEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TypeId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int TypeId { get; set; }
    }

}
