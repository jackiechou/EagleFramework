using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Entities.Business.Manufacturers;

namespace Eagle.Entities.Business.Products
{
    [NotMapped]
    public class ProductInfo: Product
    {
        public string SmallPhotoUrl { get; set; }
        public string LargePhotoUrl { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
        public virtual ProductDiscount ProductDiscount { get; set; }
        public virtual ProductTaxRate ProductTaxRate { get; set; }
        public virtual ProductType ProductType { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
    }
}
