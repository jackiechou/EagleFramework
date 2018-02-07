using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Business.Orders
{
    [Table("Sales.OrderProductOption")]
    public class OrderProductOption : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderProductOptionId { get; set; }
        public Guid OrderNo { get; set; }
        public Guid ProductNo { get; set; }
        public int OptionId { get; set; }
        public decimal OptionValue { get; set; }
    }
}
