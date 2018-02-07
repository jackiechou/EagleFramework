using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Skins
{
    [NotMapped]
    public class SkinPackageTemplateInfo: SkinPackageTemplate
    {
        public virtual SkinPackage Package { get; set; }
        public virtual SkinPackageType Type { get; set; }
    }
}
