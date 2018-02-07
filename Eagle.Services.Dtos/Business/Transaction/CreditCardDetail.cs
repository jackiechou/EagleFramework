using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eagle.Core.Settings;

namespace Eagle.Services.Dtos.Business.Transaction
{
    public class CreditCardDetail : DtoBase
    {
        public int CreditCardId { get; set; }
        public string CreditCardName { get; set; }
        public string CreditCardCode { get; set; }
        public string CardType { get; set; }
        public string CardNumber { get; set; }
        public int ExpiredMonth { get; set; }
        public int ExpiredYear { get; set; }
        public CreditCardStatus IsActive { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
