using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;
using Eagle.Entities.SystemManagement.FileStorage;

namespace Eagle.Entities.SystemManagement
{
    [Table("Menu")]
    public class Menu: BaseEntity
    {
        public Menu()
        {
            MenuCode = Guid.NewGuid();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MenuId { get; set; }
        public Guid MenuCode { get; set; }
        public int TypeId { get; set; }
        public string PositionId { get; set; }
        public int? ParentId { get; set; }
        public int Depth { get; set; }
        public string Lineage { get; set; }
        public int ListOrder { get; set; }
        public bool? HasChild { get; set; }
        public string MenuName { get; set; }
        public string MenuTitle { get; set; }
        public string MenuAlias { get; set; }
        public string Description { get; set; }
        public string Target { get; set; }
        public string IconClass { get; set; }
        public int? IconFile { get; set; }
        public string Color { get; set; }
        public string CssClass { get; set; }
        public bool? IsSecured { get; set; }
        public MenuStatus Status { get; set; }

        public int? PageId { get; set; }
        public Guid ApplicationId { get; set; }

        [NotMapped]
        public virtual ICollection<Menu> Children { get; set; }

        //[NotMapped]
        //public virtual DocumentInfo DocumentFileInfo { get; set; }
    }
}
