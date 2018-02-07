using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Business.Products
{
    [Table("Production.ProductAttributeOption")]
    public class ProductAttributeOption : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OptionId { get; set; }
        public string OptionName { get; set; }
        public decimal? OptionValue { get; set; }
        public int ListOrder { get; set; }
        public ProductAttributeOptionStatus? IsActive { get; set; }

        public int AttributeId { get; set; }
    }
}
