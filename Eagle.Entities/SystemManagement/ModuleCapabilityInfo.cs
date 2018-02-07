using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.SystemManagement
{
    [NotMapped]
    public class ModuleCapabilityInfo : ModuleCapability
    {
        public virtual Module Module { get; set; }
    }
}
