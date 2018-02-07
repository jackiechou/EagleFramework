using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Skins
{
    [NotMapped]
    public class Theme : EntityBase
    {
        public string PackageName { get; set; }
        public string PackageSrc { get; set; }
    }
}
