using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using System.Web.Mvc;
using Eagle.Core.Pagination;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.SystemManagement.FileStorage;

namespace Eagle.Services.Dtos.Contents.Articles
{
    public class NewsEntry : DtoBase
    {
        public NewsEntry()
        {
            PostedDate = DateTime.UtcNow;
        }

        [Display(ResourceType = typeof(LanguageResource), Name = "CategoryId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int CategoryId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Title")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(400, MinimumLength = 2, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MinMaxTitleLength")]
        public string Title { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Headline")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(400, MinimumLength = 2, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MinMaxTitleLength")]
        //[RegularExpression(@"[a-zA-Z0-9 \\\-~!@#$%^&*()_+={}:|""?`;:><',./[\]]+", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "InvalidHeadline")]
        //[RegularExpression(@"[a-zA-Z0-9_ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ\\s \\\-~!@#$%^&*()_+={}:|""?`;:><',./[\]]+", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "InvalidTitle")]
        public string Headline { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Summary")]
        [StringLength(4000, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Summary { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Authors")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Authors { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Url")]
        [Url(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "InvalidUrl")]
        [StringLength(150, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string NavigateUrl { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "MainText")]
        //[StringLength(int.MaxValue, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string MainText { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Source")]
        [StringLength(100, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Source { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [IgnoredDateType]
        [Display(ResourceType = typeof(LanguageResource), Name = "PostedDate")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public DateTime PostedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "File")]
        public HttpPostedFileBase File { get; set; }
    }
    public class NewsEditEntry : NewsEntry
    {
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [Display(ResourceType = typeof(LanguageResource), Name = "NewsId")]
        public int NewsId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Attachment")]
        public int? FrontImage { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Attachment")]
        public int? MainImage { get; set; }

        public List<DocumentInfoDetail> DocumentInfos { get; set; }
    }
    public class NewsDetail : BaseDto
    {
        [NotMapped]
        [Display(ResourceType = typeof(LanguageResource), Name = "Id")]
        public int Id => NewsId;

        [Display(ResourceType = typeof(LanguageResource), Name = "NewsId")]
        public int NewsId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CategoryId")]
        public int CategoryId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CategoryId")]
        public int? VendorId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Title")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string Title { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Headline")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string Headline { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Alias")]
        public string Alias { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Summary")]
        public string Summary { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Authors")]
        public string Authors { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Url")]
        public string NavigateUrl { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Attachment")]
        public int? FrontImage { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Attachment")]
        public int? MainImage { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MainText")]
        public string MainText { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Source")]
        public string Source { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Tags")]
        public string Tags { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ListOrder")]
        public int? ListOrder { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TotalRates")]
        public decimal? TotalRates { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TotalViews")]
        public int? TotalViews { get; set; }

        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(ResourceType = typeof(LanguageResource), Name = "PostedDate")]
        public DateTime PostedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        [EnumDataType(typeof(NewsStatus))]
        public NewsStatus Status { get; set; }
    }
    public class NewsInfoDetail : NewsDetail
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "CategoryName")]
        public string CategoryName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FullName")]
        public string FullName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FrontImageUrl")]
        public string FrontImageUrl { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MainImageUrl")]
        public string MainImageUrl { get; set; }

        public NewsCategoryDetail Category { get; set; }
        public List<NewsCommentDetail> Comments { get; set; }
        public List<DocumentInfoDetail> DocumentInfos { get; set; }
    }
    public class NewsListOrderEntry : DtoBase
    {
        public List<NewsSortOrderEntry> ListOrders { get; set; }
    }
    public class NewsSearchEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Keywords")]
        [StringLength(400, MinimumLength = 2, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MinMaxTitleLength")]
        public string Keywords { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Author")]
        [StringLength(255, MinimumLength = 2, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MinMaxTitleLength")]
        public string Authors { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FromDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FromDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ToDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ToDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public NewsStatus? Status { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CategoryId")]
        public int? CategoryId { get; set; }
    }
    public class NewsSearchResult : DtoBase
    {
        public NewsSearchResult(NewsSearchEntry filter, IPagedList<NewsInfoDetail> pagedList)
        {
            Filter = filter;
            PagedList = pagedList;
        }
        public NewsSearchEntry Filter { get; set; }
        public IPagedList<NewsInfoDetail> PagedList { get; set; }
    }
    public class NewsSortOrderEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "NewsId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int NewsId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ListOrder")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int ListOrder { get; set; }
    }
}
