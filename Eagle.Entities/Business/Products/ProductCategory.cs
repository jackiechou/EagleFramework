using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Business.Products
{
    [Table("Production.ProductCategory")]
    public class ProductCategory : BaseEntity
    {
        public ProductCategory()
        {
            CategoryCode = Guid.NewGuid().ToString();
            ParentId = 0;
            Depth = 1;
            Status = ProductCategoryStatus.Active;
        }

        [NotMapped]
        public int Id => CategoryId;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }
        public string CategoryCode { get; set; } 
        public string CategoryName { get; set; }
        public string CategoryAlias { get; set; }
        public string CategoryLink { get; set; }
        public int? ParentId { get; set; }
        public int? Depth { get; set; }
        public string Lineage { get; set; }
        public bool? HasChild { get; set; }
        public int? ViewOrder { get; set; }
        public string Icon { get; set; }
        public string BriefDescription { get; set; }
        public string Description { get; set; }
        public ProductCategoryStatus Status { get; set; }

        public string LanguageCode { get; set; }
        public int VendorId { get; set; }
    }
}
