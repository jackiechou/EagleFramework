using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.SystemManagement
{
    [Table("ModuleCapability")]
    public class ModuleCapability : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CapabilityId { get; set; }
        public string CapabilityName { get; set; }
        public string CapabilityCode { get; set; }
        public string Description { get; set; }
        public int DisplayOrder { get; set; }
        public ModuleCapabilityStatus? IsActive { get; set; }

        public int ModuleId { get; set; }
    }
}
