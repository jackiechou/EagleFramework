using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Entities.Business.Customers;
using Eagle.Entities.Business.Transactions;

namespace Eagle.Entities.Business.Orders
{
    [NotMapped]
    public class OrderInfo: Order
    {
        public virtual Customer Customer { get; set; }
        public virtual TransactionMethod TransactionMethod { get; set; }
        public virtual OrderPayment OrderPayment { get; set; }
        public virtual OrderShipment Shipment { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }
    }
}
