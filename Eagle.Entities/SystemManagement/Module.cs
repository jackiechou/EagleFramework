using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.SystemManagement
{
    [Table("Module")]
    public class Module: EntityBase
    {
        public Module()
        {
            IsActive = ModuleStatus.Active;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ModuleId { get; set; }
        public ModuleType ModuleTypeId { get; set; }
        public string ModuleCode { get; set; } 
        public string ModuleName { get; set; }
        public string ModuleTitle { get; set; }
        public string Description { get; set; }
        public string Header { get; set; }
        public string Footer { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? ViewOrder { get; set; }

        public bool? InheritViewPermissions { get; set; }
        public bool? AllPages { get; set; }
        public bool? IsSecured { get; set; }
        public ModuleStatus? IsActive { get; set; }

        public Guid ApplicationId { get; set; }

        //[ForeignKey("ModuleId")]
        public virtual ICollection<PageModule> PageModules { get; set; }
        public virtual ICollection<ModuleCapability> ModuleCapabilities { get; set; }
        //public virtual ICollection<ModuleSet> ModuleGroups { get; set; }
        //public virtual ICollection<ModulePermission> PermissionModuleCapabilityAccess { get; set; }
    }
}
