using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Business.Products
{
    [Table("Production.Attribute")]
    public class Attribute : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttributeId { get; set; }
        public string AttributeName { get; set; }
        public int ListOrder { get; set; }
        public ProductAttributeStatus IsActive { get; set; }

        public int CategoryId { get; set; }
    }
}
