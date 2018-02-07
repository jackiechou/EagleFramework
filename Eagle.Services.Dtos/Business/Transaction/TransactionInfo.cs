using System;
using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Business.Transaction
{
    public class TransactionInfo
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "FirstName")]
        public string FirstName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LastName")]
        public string LastName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Email")]
        public string Email { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "OrderNo")]
        public Guid OrderNo { get; set; }

    }
}
