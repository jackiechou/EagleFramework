using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Business.Manufacturers
{
    [Table("Production.ManufacturerCategory")]
    public class ManufacturerCategory : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public ManufacturerCategoryStatus IsActive { get; set; }

        public int VendorId { get; set; }
    }
}
