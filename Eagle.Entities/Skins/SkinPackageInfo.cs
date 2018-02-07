using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Skins
{
    [NotMapped]
    public class SkinPackageInfo : SkinPackage
    {
        public SkinPackageType Type { get; set; }
    }
}
