using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Skins
{
    [NotMapped]
    public class SkinPackageBackgroundInfo : SkinPackageBackground
    {
        public virtual SkinPackage Package { get; set; }
    }
}
