using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Eagle.Core.Settings;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Business.Transaction
{
    #region GET - Shopping Cart CheckOut
    public class ShoppingCheckOut
    {
        public CartInfoForShopping Cart { get; set; }
        //public List<ShippingMethodInfo> ShippingMethods { get; set; }
        public List<OrderState> States { get; set; }
    }

    public class CheckoutShipmentInfoViewModel
    {
        public ShipmentInfo ShipmentInfo { get; set; }
        public decimal Total { get; set; }
    }
    public class CheckOutPromotionViewModel
    {
        public decimal Promotion { get; set; }
        public decimal Total { get; set; }
    }
    
    #endregion

    #region POST - Shopping Cart CheckOut
   
    public class CheckOutSubmitViewModel
    {
        public int? TransactionMethodId { get; set; }

        // Customer Info
        public CheckOutSubmitCustomerInfo CustomerInfo { get; set; }
        // Shipping Info
        public CheckOutSubmitShipment ShipmentInfo { get; set; }
        // Promotion Info
        public CheckOutSubmitPromotionInfo PromotionInfo { get; set; }
        // Payment 
        public OrderPaymentEntry PaymentInfo { get; set; }
    }
    public class CheckOutSubmitShipment
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ReceiverName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string ReceiverName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Address")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string AddressDetail { get; set; }
       
        [Display(ResourceType = typeof(LanguageResource), Name = "CountryId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int? CountryId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ProvinceId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int? ProvinceId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CityId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int? CityId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "RegionId")]
        public int? RegionId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PostalCode")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string PostalCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ShippingMethodId")]
        public int ShippingMethodId { get; set; }
    }
    public class CheckOutSubmitCustomerInfo
    {
        [EmailAddress]
        [Display(ResourceType = typeof(LanguageResource), Name = "Email")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string Email { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Comment")]
        public string Comment { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Phone")]
        public string Phone { get; set; }
    }
    public class CheckOutSubmitPromotionInfo
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PromotionCode")]
        public string PromotionCode { get; set; }
    }
  
    
    #endregion
}
