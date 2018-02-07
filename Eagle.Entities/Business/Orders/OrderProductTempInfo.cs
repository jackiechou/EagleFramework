using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Entities.Business.Employees;

namespace Eagle.Entities.Business.Orders
{
    [NotMapped]
    public class OrderProductTempInfo : OrderProductTemp
    {
        public OrderTemp Order { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
