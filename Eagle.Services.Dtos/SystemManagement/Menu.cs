using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services.Dtos.SystemManagement.FileStorage;

namespace Eagle.Services.Dtos.SystemManagement
{
    public class MenuEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PageId")]
        public int? PageId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MenuTypeId")]
        //[EnumDataType(typeof(MenuType))]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int TypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ParentId")]
        public int? ParentId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MenuName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(255, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string MenuName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MenuTitle")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(255, MinimumLength = 2, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MinMaxTitleLength")]
        [RegularExpression(@"[a-zA-Z0-9_ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ\\s \\\-~!@#$%^&*()_+={}:|""?`;:><',./[\]]+", ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "InvalidTitle")]
        public string MenuTitle { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Target")]
        [StringLength(10, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Target { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FileUpload")]
        public HttpPostedFileBase FileUpload { get; set; }


        [Display(ResourceType = typeof(LanguageResource), Name = "IconClass")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string IconClass { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Color")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Color { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CssClass")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string CssClass { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        [StringLength(4000, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        [EnumDataType(typeof(MenuStatus))]
        public MenuStatus Status { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsSecured")]
        public bool? IsSecured { get; set; } = false;

        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public List<int> SelectedPositions { get; set; }

    }
    public class MenuEditEntry : MenuEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "MenuId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int MenuId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IconFile")]
        public int? IconFile { get; set; }

        [NotMapped]
        public DocumentInfoDetail DocumentFileInfo { get; set; }
    }
    public class MenuDetail : BaseDto
    {

        [Display(ResourceType = typeof(LanguageResource), Name = "MenuId")]
        public int MenuId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MenuCode")]
        public Guid MenuCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MenuTypeId")]
        public int TypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PositionId")]
        public string PositionId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ParentId")]
        public int? ParentId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Depth")]
        public int Depth { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ListOrder")]
        public int ListOrder { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "HasChild")]
        public bool HasChild { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MenuName")]
        public string MenuName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MenuTitle")]
        public string MenuTitle { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MenuAlias")]
        public string MenuAlias { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Target")]
        public string Target { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IconFile")]
        public int? IconFile { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IconClass")]
        public string IconClass { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Color")]
        public string Color { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CssClass")]
        public string CssClass { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsSecured")]
        public bool? IsSecured { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        [EnumDataType(typeof(MenuStatus))]
        public MenuStatus Status { get; set; }



        [Display(ResourceType = typeof(LanguageResource), Name = "PageId")]
        public int? PageId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ApplicationId")]
        public Guid ApplicationId { get; set; }

        public DocumentInfoDetail DocumentFileInfo { get; set; }

        public virtual List<MenuDetail> Children { get; set; }

        //Modified ====================================================================================
        //public Guid UserId { get; set; }
        //public string IconUrl { get; set; }
        //public bool? IsExtenalLink { get; set; }
        //public string PageUrl { get; set; }
        //public string PagePath { get; set; }
        //public List<Menu> SubMenuList { get; set; }
        //public List<MenuType> MenuTypeList { get; set; }

    }
    public class MenuDeepNodeData
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "MenuId")]
        public int MenuId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ParentId")]
        public int ParentId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TypeId")]
        public int TypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Depth")]
        public int Depth { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ListOrder")]
        public int ListOrder { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MenuName")]
        public string MenuName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Alias")]
        public string Alias { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PageId")]
        public int PageId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Target")]
        public string Target { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IconFile")]
        public int IconFile { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IconClass")]
        public string IconClass { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CssClass")]
        public string CssClass { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public int Status { get; set; }

        public IEnumerable<MenuDeepNodeData> Children { get; set; }
    }
    public class MenuFlatData
    {
        public int MenuId { get; set; }
        public int ParentId { get; set; }
        public int TypeId { get; set; }
        public int Depth { get; set; }
        public int ListOrder { get; set; }
        public string MenuName { get; set; }
        public string Alias { get; set; }
        public int PageId { get; set; }
        public string Target { get; set; }
        public int IconFile { get; set; }
        public string IconClass { get; set; }
        public string CssClass { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }

        public MenuFlatData(int menuId, int parentId, int typeId, int depth,
            int listOrder, string name, string alias, int pageId, string target, int iconFile, string iconClass, string cssClass, string description, int status)
        {
            MenuId = menuId;
            ParentId = parentId;
            TypeId = typeId;
            Depth = depth;
            ListOrder = listOrder;
            MenuName = name;
            Alias = alias;
            PageId = pageId;
            Target = target;
            IconFile = iconFile;
            IconClass = iconClass;
            CssClass = cssClass;
            Description = description;
            Status = status;
        }
    }
    public class MenuNodeData
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "MenuId")]
        public int MenuId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ParentId")]
        public int? ParentId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Depth")]
        public int Depth { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ListOrder")]
        public int ListOrder { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MenuName")]
        public string MenuName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Alias")]
        public string Alias { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PageId")]
        public int? PageId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Target")]
        public string Target { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IconFile")]
        public int? IconFile { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IconClass")]
        public string IconClass { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CssClass")]
        public string CssClass { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public int Status { get; set; }

        public List<MenuNodeData> Children { get; set; }
    }
    public class MenuTreeDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Id")]
        public int Id { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Key")]
        public int Key { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ParentId")]
        public int? ParentId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Name")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Title")]
        public string Title { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Text")]
        public string Text { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Tooltip")]
        public string Tooltip { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsParent")]
        public bool? IsParent { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Open")]
        public bool? Open { get; set; }

        public List<MenuTreeDetail> Children { get; set; }

        public MenuTreeDetail()
        {
            Children = new List<MenuTreeDetail>();
        }
    }
    public class MenuTreeNodeDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Id")]
        public int id { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Key")]
        public int key { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ParentId")]
        public int? parentId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Name")]
        public string name { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Title")]
        public string title { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Text")]
        public string text { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Tooltip")]
        public string tooltip { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsParent")]
        public bool? isParent { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Open")]
        public bool? open { get; set; }

        public List<MenuTreeNodeDetail> children { get; set; } = new List<MenuTreeNodeDetail>();
    }
}
