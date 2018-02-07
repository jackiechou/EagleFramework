using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Business
{
    public class ProductFileDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "VendorId")]
        public int VendorId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileId")]
        public int FileId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ProductNo")]
        public int ProductId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileName")]
        public string FileName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileTitle")]
        public string FileTitle { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileExtension")]
        public string FileExtension { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileUrl")]
        public string FileUrl { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        [StringLength(255, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public byte Status { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsImage")]
        public byte IsImage { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Width")]
        public int Width { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Height")]
        public int Height { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ThumbWidth")]
        public int ThumbWidth { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ThumbHeight")]
        public int ThumbHeight { get; set; }
    }

    public class ProductFileEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "VendorId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "PleaseSelectVendor")]
        public int VendorId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ProductId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int ProductId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string FileName { get; set; }


        [Display(ResourceType = typeof(LanguageResource), Name = "FileTitle")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(400, MinimumLength = 2, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MinMaxTitleLength")]
        [RegularExpression(@"[a-zA-Z0-9_ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ\\s \\\-~!@#$%^&*()_+={}:|""?`;:><',./[\]]+", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "InvalidTitle")]
        public string FileTitle { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileUrl")]
        [StringLength(255, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string FileUrl { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        [StringLength(255, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsImage")]
        public bool? IsImage { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Width")]
        public int? Width { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Height")]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "GreaterThanZero")]
        public int? Height { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ThumbWidth")]
        public int? ThumbWidth { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ThumbHeight")]
        public int? ThumbHeight { get; set; }
    }
}
