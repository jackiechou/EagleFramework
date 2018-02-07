using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.SystemManagement
{
    public class MenuPageDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "MenuId")]
        public int MenuId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MenuCode")]
        public Guid MenuCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ParentId")]
        public int? ParentId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Depth")]
        public int Depth { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Lineage")]
        public string Lineage { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ListOrder")]
        public int ListOrder { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "HasChild")]
        public bool? HasChild { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MenuName")]
        public string MenuName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MenuTitle")]
        public string MenuTitle { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MenuAlias")]
        public string MenuAlias { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Target")]
        public string Target { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IconClass")]
        public string IconClass { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IconFile")]
        public int? IconFile { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Color")]
        public string Color { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CssClass")]
        public string CssClass { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsSecured")]
        public bool? IsSecured { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        [EnumDataType(typeof(MenuStatus))]
        public MenuStatus Status { get; set; }


        [Display(ResourceType = typeof(LanguageResource), Name = "ApplicationId")]
        public Guid ApplicationId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PageId")]
        public int? PageId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TypeId")]
        public int TypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PositionId")]
        public string PositionId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TemplateId")]
        public int? TemplateId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IPLog")]
        public string Ip { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IPLastUpdated")]
        public string LastUpdatedIp { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CreatedDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "DateValid")]
        public DateTime? CreatedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LastModifiedDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "DateValid")]
        public DateTime? LastModifiedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CreatedByUserId")]
        public Guid CreatedByUserId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LastModifiedByUserId")]
        public Guid? LastModifiedByUserId { get; set; }

        public IEnumerable<MenuPageDetail> Children { get; set; }
        public PageInfoDetail Page { get; set; }
    }
}
