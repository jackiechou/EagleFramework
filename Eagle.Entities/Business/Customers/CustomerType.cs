using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Business.Customers
{
    [Table("Sales.CustomerType")]
    public class CustomerType : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerTypeId { get; set; }
        public string CustomerTypeName { get; set; }
        public int? PromotionalRate { get; set; }
        public string Note { get; set; }
        public CustomerTypeStatus IsActive { get; set; }


        public int VendorId { get; set; }
    }
}
