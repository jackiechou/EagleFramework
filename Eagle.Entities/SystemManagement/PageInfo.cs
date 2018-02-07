using System;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.SystemManagement
{
    [NotMapped]
    public class PageInfo : EntityBase
    {
        public int PageId { get; set; }
        public string PageCode { get; set; }
        public int PageTypeId { get; set; }
        public string PageName { get; set; }
        public string PageTitle { get; set; }
        public string PageAlias { get; set; }
        public string PagePath { get; set; }
        public string PageUrl { get; set; }
        public int ListOrder { get; set; }
        public string IconClass { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string PageHeadText { get; set; }
        public string PageFooterText { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? DisableLink { get; set; }
        public bool DisplayTitle { get; set; }
        public bool? IsExtenalLink { get; set; }
        public bool? IsSecured { get; set; }
        public bool? IsMenu { get; set; }
        public PageStatus IsActive { get; set; }


        public Guid ApplicationId { get; set; }
        public int? TemplateId { get; set; }
        public string LanguageCode { get; set; }
    }
}
