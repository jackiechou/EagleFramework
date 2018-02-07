using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Skins
{
    [Table("SkinPackage")]
    public class SkinPackage : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PackageId { get; set; }
        public string PackageName { get; set; }
        public string PackageAlias { get; set; }
        public string PackageSrc { get; set; }
        public bool IsSelected { get; set; }
        public bool IsActive { get; set; }

        public Guid ApplicationId { get; set; }
        public int TypeId { get;set; }
    }
}
