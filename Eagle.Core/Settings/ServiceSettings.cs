using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Settings
{
   
    public enum PaymentMethodStatus
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "InActive", Description = "InActive", Order = 1)]
        InActive = 0,
        [Display(ResourceType = typeof(LanguageResource), Name = "Active", Description = "Active", Order = 0)]
        Active = 1
    }
    public enum ServiceType
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "SinglePackage", Description = "SinglePackage", Order = 0)]
        Single = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "FullPackage", Description = "FullPackage", Order = 1)]
        Full = 2,
    }
    public enum ServicePackTypeStatus
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Active", Description = "Active", Order = 0)]
        Active = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "InActive", Description = "InActive", Order = 1)]
        InActive = 0
    }
    public enum ServicePackStatus
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Active", Description = "Active", Order = 0)]
        Active = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "InActive", Description = "InActive", Order = 1)]
        InActive = 0
    }
    public enum ServicePackOptionStatus
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Active", Description = "Active", Order = 0)]
        Active = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "InActive", Description = "InActive", Order = 1)]
        InActive = 0
    }
    public enum ServiceDiscountStatus
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Active", Description = "Active", Order = 0)]
        Active = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "InActive", Description = "InActive", Order = 1)]
        InActive = 0,
    }
    public enum ServiceCategoryRoot
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Default", Description = "Default", Order = 1)]
        Default = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "Voucher", Description = "Voucher", Order = 2)]
        Voucher = 2
    }

    public enum ServiceCategoryStatus
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "InActive", Description = "InActive", Order = 0)]
        InActive = 0,
        [Display(ResourceType = typeof(LanguageResource), Name = "Active", Description = "Active", Order = 1)]
        Active = 1
    }
    public enum ServiceBookingOrderDetailTempStatus
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Pending", Description = "Pending", Order = 0)]
        Pending = 0,
        [Display(ResourceType = typeof(LanguageResource), Name = "Approved", Description = "Approved", Order = 1)]
        Approved = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "Cancelled", Description = "Cancelled", Order = 2)]
        Cancelled = 2,
        [Display(ResourceType = typeof(LanguageResource), Name = "Rejected", Description = "Rejected", Order = 3)]
        Rejected = 3,
    }

    public enum ServiceBookingOrderDetailStatus
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Pending", Description = "Pending", Order = 0)]
        Pending = 0,
        [Display(ResourceType = typeof(LanguageResource), Name = "Approved", Description = "Approved", Order = 1)]
        Approved = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "Cancelled", Description = "Cancelled", Order = 2)]
        Cancelled = 2,
        [Display(ResourceType = typeof(LanguageResource), Name = "Rejected", Description = "Rejected", Order = 3)]
        Rejected = 3,
    }

    public enum ServiceBookingOrderStatus
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Pending", Description = "Pending", Order = 0)]
        Pending = 0,
        [Display(ResourceType = typeof(LanguageResource), Name = "Approved", Description = "Approved", Order = 1)]
        Approved = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "Cancelled", Description = "Cancelled", Order = 2)]
        Cancelled = 2,
        [Display(ResourceType = typeof(LanguageResource), Name = "Rejected", Description = "Rejected", Order = 3)]
        Rejected = 3,
    }

    public enum BookingError
    {
        //TODO: Change the errors to match actual business
        None = 0,
        [Display(ResourceType = typeof(LanguageResource), Name = "OrderPromoCodeUtilized", Description = "OrderPromoCodeUtilized")]
        PromoCodeUtilized = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "OrderCannotSaveOrder", Description = "OrderCannotSaveOrder")]
        CannotSaveOrder = 2,
        [Display(ResourceType = typeof(LanguageResource), Name = "OrderProcessPaymentError", Description = "OrderProcessPaymentError")]
        ProcessPaymentError = 3,
        [Display(ResourceType = typeof(LanguageResource), Name = "OrderQuantityNotSufficient", Description = "OrderQuantityNotSufficient")]
        QuantityNotSufficient = 4,
        [Display(ResourceType = typeof(LanguageResource), Name = "OrderEmptyCart", Description = "OrderEmptyCart")]
        EmptyCart = 5
    }
    public enum BookingWarning
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "OrderPaymentFail", Description = "OrderQuantityNotSufficient")]
        PaymentFail = 1
    }

    //public enum ServiceCategorySetting
    //{
    //    [Display(ResourceType = typeof(LanguageResource), Name = "ConsultingVaccination", Description = "ConsultingVaccination", Order = 0)]
    //    ConsultingVaccination = 1002,
    //    [Display(ResourceType = typeof(LanguageResource), Name = "VoluntaryTesting", Description = "VoluntaryTesting", Order = 1)]
    //    VoluntaryTesting = 2002,
    //    [Display(ResourceType = typeof(LanguageResource), Name = "ChemicalAssay", Description = "ChemicalAssay", Order = 2)]
    //    ChemicalAssay = 2003,
    //    [Display(ResourceType = typeof(LanguageResource), Name = "Spraying", Description = "Spraying", Order = 3)]
    //    Spraying = 2004,
    //    [Display(ResourceType = typeof(LanguageResource), Name = "WaterTesting", Description = "WaterTesting", Order = 4)]
    //    WaterTesting = 2005,
    //    [Display(ResourceType = typeof(LanguageResource), Name = "RentalHall", Description = "RentalHall", Order = 5)]
    //    RentalHall = 2006
    //}
}
