using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services.Dtos.SystemManagement.FileStorage;

namespace Eagle.Services.Dtos.Contents.Banners
{
    public class BannerSearchEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "BannerName")]
        public string BannerName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Zone")]
        public int? BannerPositionId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Type")]
        public int? BannerTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public BannerStatus? Status { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Page")]
        public int? PageId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Advertiser")]
        public string Advertiser { get; set; }
    }
    public class BannerInfoDetail : BannerDetail
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "LinkToImage")]
        public string FileUrl { get; set; }

        public DocumentInfoDetail Document { get; set; }
        public BannerScopeDetail Scope { get; set; }
        public BannerTypeDetail Type { get; set; }
        public IEnumerable<BannerZoneInfoDetail> Zones { get; set; }
    }
    public class BannerDetail : BaseDto
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "BannerId")]
        public int BannerId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "VendorId")]
        public int VendorId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ScopeId")]
        public int ScopeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TypeId")]
        public int TypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LanguageCode")]
        public string LanguageCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "BannerTitle")]
        public string BannerTitle { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "BannerContent")]
        public string BannerContent { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "AltText")]
        public string AltText { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Attachment")]
        public int? FileId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Link")]
        public string Link { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Advertiser")]
        public string Advertiser { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Tags")]
        public string Tags { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ListOrder")]
        public int? ListOrder { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ClickThroughs")]
        public int? ClickThroughs { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Impressions")]
        public int? Impressions { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Width")]
        public int? Width { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Height")]
        public int? Height { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "StartDate")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "DateValid")]
        public DateTime? StartDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EndDate")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "DateValid")]
        public DateTime? EndDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Target")]
        public string Target { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public BannerStatus Status { get; set; }
    }
    public class BannerEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "TypeId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "ThisFieldMustBeSelected")]
        public int TypeId { get; set; }

        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "ThisFieldMustBeSelected")]
        [Display(ResourceType = typeof(LanguageResource), Name = "ScopeId")]
        public int ScopeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "BannerTitle")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "ThisFieldIsRequired")]
        //[RegularExpression(@"[a-zA-Z0-9_ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ\\s \\\-~!@#$%^&*()_+={}:|""?`;:><',./[\]]+", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "InvalidTitle")]
        [StringLength(300, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string BannerTitle { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "BannerContent")]
        //[StringLength(int.MaxValue, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string BannerContent { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "AltText")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string AltText { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Link")]
        [StringLength(4000, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Link { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        //[StringLength(int.MaxValue, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Advertiser")]
        public string Advertiser { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Tags")]
        [StringLength(4000, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Tags { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Impressions")]
        public int? Impressions { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ClickThroughs")]
        public int? ClickThroughs { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Width")]
        public int? Width { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Height")]
        public int? Height { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "StartDate")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "DateValid")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "ThisFieldIsRequired")]
        public DateTime? StartDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EndDate")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "DateValid")]
        public DateTime? EndDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Target")]
        [StringLength(6, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Target { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        [EnumDataType(typeof(BannerStatus))]
        public BannerStatus Status { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "File")]
        public HttpPostedFileBase File { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SelectedPages")]
        public List<int> SelectedPages { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SelectedPositions")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "ThisFieldIsRequired")]
        public List<int> SelectedPositions { get; set; }
    }
    public class BannerEditEntry : BannerEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "BannerId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int BannerId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Attachment")]
        public int? FileId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LinkToImage")]
        //[StringLength(int.MaxValue, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string FileUrl { get; set; }
    }
}
