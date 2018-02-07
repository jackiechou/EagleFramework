using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Entities.SystemManagement;

namespace Eagle.Entities.Business.Customers
{
    [NotMapped]
    public class CustomerInfo: Customer
    {
        public virtual Address Address { get; set; }
    }
}
