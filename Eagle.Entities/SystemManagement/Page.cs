using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.SystemManagement
{
    [Table("Page")]
    public class Page: BaseEntity
    {
        public Page()
        {
            PageCode = Guid.NewGuid().ToString();
            TemplateId = 1;
            DisplayTitle = true;
            DisableLink = false;
            IsExtenalLink = false;
            IsSecured = false;
            IsMenu = true;
            IsActive = PageStatus.Active;
        }

        [Key, Column("PageId"),DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PageId { get; set; }
        public string PageCode { get; set; }
        public PageType PageTypeId { get; set; }
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


        [NotMapped]
        public virtual ICollection<PageModule> PageModules { get; set; }
    }
}
