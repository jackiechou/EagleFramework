using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Entities.SystemManagement;

namespace Eagle.Entities.Business.Vendors
{
    [NotMapped]
    public class VendorAddressInfo: VendorAddress
    {
        public virtual Address Address { get; set; }
    }
}
