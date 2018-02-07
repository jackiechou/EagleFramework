using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.SystemManagement
{
    public class PageTree : DtoBase
    {
        public int Id { get; set; }
        public int Key { get; set; }
        public string Value { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Tooltip { get; set; }
        public bool? IsParent { get; set; }
        public bool? Open { get; set; }
        public string Icon { get; set; }
        public List<PageTree> Children { get; set; }

        public PageTree()
        {
            Children = new List<PageTree>();
        }
    }
    public class PageSearchEntry : DtoBase
    {
        public PageSearchEntry()
        {
            PageType = PageType.Admin;
        }

        public PageType PageType { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SearchText")]
        public string SearchText { get; set; }
    }
    public class PageDetail : BaseDto
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PageId")]
        public int PageId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PageCode")]
        public string PageCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PageType")]
        public PageType PageTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PageName")]
        public string PageName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PageTitle")]
        public string PageTitle { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PageAlias")]
        public string PageAlias { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PagePath")]
        public string PagePath { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PageUrl")]
        public string PageUrl { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ListOrder")]
        public int ListOrder { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IconClass")]
        public string IconClass { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Icon")]
        public int? IconFile { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IconUrl")]
        public string IconUrl { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Keywords")]
        public string Keywords { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PageHeadText")]
        public string PageHeadText { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PageFooterText")]
        public string PageFooterText { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "StartDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? StartDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EndDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? EndDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "DisableLink")]
        public bool? DisableLink { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "DisplayTitle")]
        public bool DisplayTitle { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsExtenalLink")]
        public bool? IsExtenalLink { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsMenu")]
        public bool IsMenu { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsSecured")]
        public bool? IsSecured { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        [EnumDataType(typeof(PageStatus))]
        public PageStatus IsActive { get; set; }


        [Display(ResourceType = typeof(LanguageResource), Name = "ApplicationId")]
        public Guid ApplicationId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TemplateId")]
        public int? TemplateId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LanguageCode")]
        public string LanguageCode { get; set; }

        public IEnumerable<PageModuleDetail> PageModules { get; set; }
    }
    public class PageEntry : DtoBase 
    {
        public PageEntry()
        {
            StartDate = DateTime.UtcNow;
            TemplateId = 1;
            DisplayTitle = true;
            DisableLink = false;
            IsExtenalLink = false;
            IsSecured = false;
            IsMenu = true;
        }

        [Display(ResourceType = typeof(LanguageResource), Name = "PageTypeId")]
        public int? PageTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TemplateId")]
        public int? TemplateId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PageName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(200, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string PageName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PageTitle")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(200, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string PageTitle { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PageCode")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(200, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 2)]
        [RegularExpression(@"^[-_ a-zA-Z0-9]+$", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "InvalidCode")]
        public string PageCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PageUrl")]
        [StringLength(255, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string PageUrl { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PagePath")]
        [StringLength(255, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string PagePath { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IconClass")]
        public string IconClass { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        [StringLength(4000, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Keywords")]
        [StringLength(500, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Keywords { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "PageHeadText")]
        [StringLength(500, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string PageHeadText { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "PageFooterText")]
        public string PageFooterText { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "StartDate")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        //[DataType(DataType.Date, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "DateValid")]

        public DateTime? StartDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EndDate")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        //[DataType(DataType.Date, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "DateValid")]

        public DateTime? EndDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "DisableLink")]
        public bool? DisableLink { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "DisplayTitle")]
        public bool DisplayTitle { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsExtenalLink")]
        public bool? IsExtenalLink { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsMenu")]
        public bool IsMenu { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsSecured")]
        public bool? IsSecured { get; set; }

        public int[] SelectedModules { get; set; }
    }
    public class PageEditEntry : PageEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PageId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int PageId { get; set; }

        public List<PagePermissionEntry> PagePermissions { get; set; }
    }
    public class PageInfoDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PageId")]
        public int PageId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PageCode")]
        public Guid? PageCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PageType")]
        public int PageTypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PageName")]
        public string PageName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PageTitle")]
        public string PageTitle { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PageAlias")]
        public string PageAlias { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PagePath")]
        public string PagePath { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PageUrl")]
        public string PageUrl { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ListOrder")]
        public int ListOrder { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IconClass")]
        public string IconClass { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Keywords")]
        public string Keywords { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PageHeadText")]
        public string PageHeadText { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PageFooterText")]
        public string PageFooterText { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "StartDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? StartDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EndDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? EndDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "DisableLink")]
        public bool? DisableLink { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "DisplayTitle")]
        public bool DisplayTitle { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsExtenalLink")]
        public bool? IsExtenalLink { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsSecured")]
        public bool? IsSecured { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsMenu")]
        public bool IsMenu { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        //[EnumDataType(typeof(PageStatus))]
        public PageStatus IsActive { get; set; }


        [Display(ResourceType = typeof(LanguageResource), Name = "ApplicationId")]
        public Guid ApplicationId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TemplateId")]
        public int? TemplateId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LanguageCode")]
        public string LanguageCode { get; set; }

        //public IEnumerable<PageModuleDetail> PageModules { get; set; }
    }
}
