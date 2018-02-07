using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eagle.Services.Dtos.Business.Transaction
{
    public class BillingAddressDetail : DtoBase
    {
        public string CountryCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Line1 { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
    }
}
