using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Business.Products
{
    [Table("Production.ProductType")]
    public class ProductType : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public string Description { get; set; }
        public int? ListOrder { get; set; }
        public ProductTypeStatus? IsActive { get; set; }


        public int CategoryId { get; set; }
        public int VendorId { get; set; }
    }
}
