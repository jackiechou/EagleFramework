using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Eagle.Core.Attributes;
using Eagle.Resources;

namespace Eagle.Services.Dtos.Business
{
    #region Shipping Carrier
    public class ShippingCarrierSearchEntry : DtoBase
    {
        public string ShippingCarrierName { get; set; }
        public bool? IsActive { get; set; }
    }
    public class ShippingCarrierEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ShippingCarrierName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(150, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string ShippingCarrierName { get; set; }


        [Display(ResourceType = typeof(LanguageResource), Name = "Phone")]
        [StringLength(30, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string Phone { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Address")]
        public string Address { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public bool IsActive { get; set; }
    }
    public class ShippingCarrierEditEntry : ShippingCarrierEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ShippingCarrierId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int ShippingCarrierId { get; set; }
    }
    public class ShippingCarrierDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ShippingCarrierId")]
        public int ShippingCarrierId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "VendorId")]
        public int VendorId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ShippingCarrierName")]
        public string ShippingCarrierName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Phone")]
        public string Phone { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Address")]
        public string Address { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsSelected")]
        public bool IsSelected { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public bool IsActive { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ListOrder")]
        public int? ListOrder { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CreatedDate")]
        public DateTime? CreatedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LastModifiedDate")]
        public DateTime? LastModifiedDate { get; set; }
    }

    #endregion

    #region Shipping Fee
    public class ShippingFeeSearchEntry : DtoBase
    {
        public string ShippingFeeName { get; set; }
        public bool? IsActive { get; set; }
    }
    public class ShippingFeeSearchZipCodeEntry : DtoBase
    {
        public int ShippingMethodId { get; set; }
        public int ShippingCarrierId { get; set; }
        public string ZipCode { get; set; }
        public decimal TotalWeight { get; set; }
    }
    public class ShippingFeeDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ShippingFeeId")]
        public int ShippingFeeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ShippingCarrierId")]
        public int ShippingCarrierId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ShippingMethodId")]
        public int ShippingMethodId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ShippingFeeName")]
        public string ShippingFeeName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ZipStart")]
        public string ZipStart { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ZipEnd")]
        public string ZipEnd { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "WeightStart")]
        public decimal WeightStart { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "WeightEnd")]
        public decimal WeightEnd { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "RateFee")]
        public decimal RateFee { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PackageFee")]
        public decimal PackageFee { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Vat")]
        public decimal Vat { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CurrencyCode")]
        public string CurrencyCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ListOrder")]
        public int ListOrder { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public bool IsActive { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CreatedDate")]
        public DateTime? CreatedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LastModifiedDate")]
        public DateTime? LastModifiedDate { get; set; }
    }
    public class ShippingFeeEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ShippingCarrierId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [ValidInteger(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "OnlyNumbersAllowed")]
        public int ShippingCarrierId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ShippingMethodId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [ValidInteger(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "OnlyNumbersAllowed")]
        public int ShippingMethodId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ShippingFeeName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(255, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 2)]
        public string ShippingFeeName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ZipStart")]
        public string ZipStart { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ZipEnd")]
        public string ZipEnd { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "WeightStart")]
        [ValidDecimal(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "OnlyDecimalNumbersAllowed")]
        public decimal WeightStart { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "WeightEnd")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [ValidDecimal(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "OnlyDecimalNumbersAllowed")]
        public decimal WeightEnd { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "RateFee")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [ValidDecimal(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "OnlyDecimalNumbersAllowed")]
        public decimal RateFee { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PackageFee")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [ValidDecimal(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "OnlyDecimalNumbersAllowed")]
        public decimal PackageFee { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Vat")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [ValidDecimal(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "OnlyDecimalNumbersAllowed")]
        public decimal Vat { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public bool IsActive { get; set; }
    }
    public class ShippingFeeEditEntry : ShippingFeeEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ShippingFeeId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int ShippingFeeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CurrencyCode")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public string CurrencyCode { get; set; }
    }
    #endregion

    #region Shipping Method
    public class ShippingMethodSearchEntry : DtoBase
    {
        public string ShippingMethodName { get; set; }
        public bool? IsActive { get; set; }
    }
    public class ShippingMethodEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ShippingMethodName")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [StringLength(150, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "CustomStringLength", MinimumLength = 3)]
        public string ShippingMethodName { get; set; }


        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public bool IsActive { get; set; }
    }
    public class ShippingMethodEditEntry : ShippingMethodEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ShippingMethodId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int ShippingMethodId { get; set; }
    }
    public class ShippingMethodDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "ShippingMethodId")]
        public int ShippingMethodId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ShippingMethodName")]
        public string ShippingMethodName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public bool IsActive { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ListOrder")]
        public int? ListOrder { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CreatedDate")]
        public DateTime? CreatedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LastModifiedDate")]
        public DateTime? LastModifiedDate { get; set; }
    }
    public class ShippingMethodInfo
    {
        public ShippingMethodDetail ShippingMethod { get; set; }
        public List<ShippingCarrierDetail> Carriers { get; set; }
    }
    #endregion
}
