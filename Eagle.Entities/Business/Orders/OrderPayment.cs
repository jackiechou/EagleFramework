using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Business.Orders
{
    [Table("Sales.OrderPayment")]
    public class OrderPayment : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderPaymentId { get; set; }
        public Guid OrderNo { get; set; }
        public int CustomerId { get; set; }
        public int PaymentMethodId { get; set; }
        public string PaymentToken { get; set; }
        public string PaymentCode { get; set; }
        public string CardType { get; set; }
        public string CardHolder { get; set; }
        public string CardNo { get; set; }
        public int? ExpMonth { get; set; }
        public int? ExpYear { get; set; }
        public string Cvv { get; set; }
        public decimal? Amount { get; set; }
    }
}
