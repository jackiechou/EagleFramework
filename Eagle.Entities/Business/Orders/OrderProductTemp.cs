using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Core.Configuration;
using Eagle.Core.Settings;

namespace Eagle.Entities.Business.Orders
{
    [Table("Sales.OrderProduct_Temp")]
    public class OrderProductTemp : EntityBase
    {
        public OrderProductTemp()
        {
            TypeId = ItemType.Product;
            Status = OrderProductTempStatus.Active;
            Quantity = 1;
            Weight = 0;
            NetPrice = 0;
            GrossPrice = 0;
            TaxRate = 0;
            DiscountRate = 0;
            CurrencyCode = GlobalSettings.DefaultCurrencyCode;
            CreatedDate = DateTime.UtcNow;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderProductId { get; set; }
        public Guid OrderNo { get; set; }
        public ItemType TypeId { get; set; }
        public int? CustomerId { get; set; }
        public int? EmployeeId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal? Weight { get; set; }
        public decimal? NetPrice { get; set; }
        public decimal? GrossPrice { get; set; }
        public decimal? TaxRate { get; set; }
        public decimal? DiscountRate { get; set; }
        public int? PeriodGroupId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? FromPeriod { get; set; }
        public int? ToPeriod { get; set; }
        public string CurrencyCode { get; set; }
        public string Comment { get; set; }
        public OrderProductTempStatus Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
