using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Settings;

namespace Eagle.Entities.Business.Orders
{
    [Table("Sales.Order_Temp")]
    public class OrderTemp : EntityBase
    {
        public OrderTemp()
        {
            OrderNo = Guid.NewGuid();
            OrderDate = DateTime.UtcNow;
            SubTotal = 0;
            TotalFees = 0;
            ShippingRate = 0;
            SubTotal = 0;
            TotalFees = 0;
            Discount = 0;
            Tax = 0;
            MarkAsRead = MarkAsRead.UnRead;
            Status = OrderStatus.Pending;
        }

        [Key]
        public Guid OrderNo { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? DueDate { get; set; }
        public decimal? ShippingRate { get; set; }
        public string PromotionCode { get; set; }
        public decimal? Discount { get; set; }
        public decimal? SubTotal { get; set; }
        public decimal? TotalFees { get; set; }
        public decimal? Tax { get; set; }
        public string CurrencyCode { get; set; }
        public string Comment { get; set; }
        public MarkAsRead MarkAsRead { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public int VendorId { get; set; }
        public int? TransactionMethodId { get; set; }
        public int CustomerId { get; set; }
    }
}
