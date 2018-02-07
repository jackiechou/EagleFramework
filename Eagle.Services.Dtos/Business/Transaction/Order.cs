using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Orders;
using Eagle.Resources;
using Eagle.Services.Dtos.Business.Personnel;
using Eagle.Services.Dtos.Common;

namespace Eagle.Services.Dtos.Business.Transaction
{
    #region Order

    public class OrderSearchBaseEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "SearchText")]
        public string SearchText { get; set; }
    }

    public class OrderSearch : OrderSearchBaseEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "VendorId")]
        public int VendorId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CustomerId")]
        public int CustomerId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "OrderStatus")]
        public OrderStatus? Status { get; set; }
    }
    public class OrderSearchEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "SearchText")]
        public string SearchText { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "FromDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FromDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ToDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ToDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public OrderStatus? Status { get; set; }
    }
    public class OrderEntry : DtoBase
    {
        public OrderEntry()
        {
            OrderDate = DateTime.UtcNow;
            SubTotal = 0;
            TotalFees = 0;
            SubTotal = 0;
            TotalFees = 0;
            Discount = 0;
            Tax = 0;
            Deposit = 0;
        }

        [Display(ResourceType = typeof(LanguageResource), Name = "CustomerId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int CustomerId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TransactionMethodId")]
        public int? TransactionMethodId { get; set; }

        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(ResourceType = typeof(LanguageResource), Name = "OrderDate")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public DateTime OrderDate { get; set; }

        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(ResourceType = typeof(LanguageResource), Name = "DueDate")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public DateTime? DueDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ShippingRate")]
        public decimal? ShippingRate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PromotionCode")]
        [StringLength(32, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string PromotionCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Deposit")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Currency)]
        public decimal? Deposit { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SubTotal")]
        public decimal? SubTotal { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TotalFees")]
        public decimal? TotalFees { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Discount")]
        public decimal? Discount { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Tax")]
        public decimal? Tax { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CurrencyCode")]
        [StringLength(50, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string CurrencyCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Comment")]
        [StringLength(250, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "MaxStringLength")]
        public string Comment { get; set; }
    }
    public class OrderEditEntry : OrderEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "OrderId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int OrderId { get; set; }
    }
    public class OrderDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "OrderId")]
        public int OrderId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "OrderNo")]
        public Guid OrderNo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "OrderDate")]
        public DateTime OrderDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "DueDate")]
        public DateTime? DueDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ShippingRate")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Currency)]
        public decimal? ShippingRate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PromotionCode")]
        public string PromotionCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Deposit")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Currency)]
        public decimal? Deposit { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SubTotal")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Currency)]
        public decimal? SubTotal { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TotalFees")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Currency)]
        public decimal? TotalFees { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Discount")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Currency)]
        public decimal? Discount { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Tax")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        public decimal? Tax { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CurrencyCode")]
        public string CurrencyCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Comment")]
        public string Comment { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MarkAsRead")]
        public MarkAsRead MarkAsRead { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        [EnumDataType(typeof(OrderStatus))]
        public OrderStatus Status { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CreatedDate")]
        public DateTime? CreatedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LastModifiedDate")]
        public DateTime? LastModifiedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "VendorId")]
        public int VendorId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TransactionMethodId")]
        public int? TransactionMethodId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CustomerId")]
        public int CustomerId { get; set; }
    }

    public class OrderInfoDetail : OrderDetail
    {
        public CustomerDetail Customer { get; set; }
        public TransactionMethodDetail TransactionMethod { get; set; }
        public OrderPaymentDetail OrderPayment { get; set; }
        public OrderShipmentDetail Shipment { get; set; }
        public PaymentMethodDetail PaymentMethod { get; set; }
        public IEnumerable<OrderProductInfoDetail> OrderProducts { get; set; }
    }

    #endregion
    
    #region Order Product
    
    public class OrderProductEntry : DtoBase
    {
        public OrderProductEntry()
        {
            TypeId = ItemType.Product;
            EmployeeId = 1;
            PeriodGroupId = 1;
            FromPeriod = 33;
            ToPeriod = 43;
            StartDate = DateTime.UtcNow.Date;
            EndDate = DateTime.UtcNow.Date;
        }

        [Display(ResourceType = typeof(LanguageResource), Name = "OrderNo")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public Guid OrderNo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TypeId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public ItemType TypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ProductId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int ProductId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CustomerId")]
        public int? CustomerId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EmployeeId")]
        public int? EmployeeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Quantity")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "GreaterThanZero")]
        public int Quantity { get; set; }
        
        [Display(ResourceType = typeof(LanguageResource), Name = "Weight")]
        public decimal? Weight { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PeriodGroupId")]
        public int? PeriodGroupId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "StartDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public DateTime? StartDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EndDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "From")]
        public int? FromPeriod { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "To")]
        public int? ToPeriod { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "NetPrice")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public decimal? NetPrice { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "GrossPrice")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        public decimal? GrossPrice { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TaxRate")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        public decimal? TaxRate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "DiscountRate")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        public decimal? DiscountRate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CurrencyCode")]
        public string CurrencyCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Comment")]
        public string Comment { get; set; }
    }
    public class OrderProductEditEntry : OrderProductEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "OrderProductId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int OrderProductId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public OrderProductStatus Status { get; set; }

        public ItemDetail Item { get; set; }
        public CustomerInfoDetail Customer { get; set; }
        public EmployeeInfoDetail Employee { get; set; }
    }
    public class OrderProductDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "OrderProductId")]
        public int OrderProductId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "OrderNo")]
        public Guid OrderNo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Type")]
        public ItemType TypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ProductNo")]
        public int ProductId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CustomerId")]
        public int? CustomerId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EmployeeId")]
        public int? EmployeeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Quantity")]
        public int Quantity { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Weight")]
        public decimal? Weight { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "NetPrice")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        public decimal? NetPrice { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "GrossPrice")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        public decimal? GrossPrice { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TaxRate")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        public decimal? TaxRate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "DiscountRate")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        public decimal? DiscountRate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CurrencyCode")]
        public string CurrencyCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PeriodGroupId")]
        public int? PeriodGroupId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "From")]
        public int? FromPeriod { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "To")]
        public int? ToPeriod { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "StartDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? StartDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EndDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Comment")]
        public string Comment { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        public OrderProductStatus Status { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CreatedDate")]
        public DateTime? CreatedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LastModifiedDate")]
        public DateTime? LastModifiedDate { get; set; }
    }

    public class OrderProductInfoDetail : OrderProductDetail
    {
        public ItemDetail Item { get; set; }
        public CustomerInfoDetail Customer { get; set; }
        public EmployeeInfoDetail Employee { get; set; }
    }
    
    #endregion 

    #region  Order Payment
    
    public class OrderPaymentEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "OrderNo")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public Guid OrderNo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CustomerId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int CustomerId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PaymentMethodId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int PaymentMethodId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CardType")]
        public string CardType { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CardHolder")]
        public string CardHolder { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CardNo")]
        public string CardNo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ExpMonth")]
        public int ExpMonth { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ExpMonth")]
        public int ExpYear { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Cvv")]
        public string Cvv { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PaymentCode")]
        public string PaymentCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Amount")]
        public decimal? Amount { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PublicKey")]
        public string PublicKey { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PaymentToken")]
        public string PaymentToken { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "BusinessName")]
        public string BusinessName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        public SelectList CardTypes { get; set; }
    }

    public class OrderPaymentEditEntry : OrderPaymentEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "OrderPaymentId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int OrderPaymentId { get; set; }
    }

    public class OrderPaymentDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "OrderPaymentId")]
        public int OrderPaymentId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "OrderNo")]
        public Guid OrderNo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CustomerId")]
        public int CustomerId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PaymentMethodId")]
        public int PaymentMethodId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PaymentToken")]
        public string PaymentToken { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PaymentCode")]
        public string PaymentCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CardType")]
        public string CardType { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CardHolder")]
        public string CardHolder { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CardNo")]
        public string CardNo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ExpMonth")]
        public int? ExpMonth { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ExpMonth")]
        public int? ExpYear { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Cvv")]
        public string Cvv { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Amount")]
        public decimal? Amount { get; set; }
    }


    #endregion

    #region Order Shipment
    
    public class OrderShipmentEntry : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "OrderNo")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public Guid OrderNo { get; set; }


        [Display(ResourceType = typeof(LanguageResource), Name = "CustomerId")]
        public int? CustomerId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ShippingMethodId")]
        public int ShippingMethodId { get; set; }


        [Display(ResourceType = typeof(LanguageResource), Name = "ShipDate")]
        public DateTime? ShipDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Weight")]
        public decimal? Weight { get; set; }


        [Display(ResourceType = typeof(LanguageResource), Name = "IsInternational")]
        public bool? IsInternational { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ReceiverName")]
        public string ReceiverName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Email")]
        public string ReceiverEmail { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Address")]
        public string ReceiverAddress { get; set; }


        [Display(ResourceType = typeof(LanguageResource), Name = "Phone")]
        public string ReceiverPhone { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CountryId")]
        public int? CountryId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CityId")]
        public int? CityId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ProvinceId")]
        public int? ProvinceId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "RegionId")]
        public int? RegionId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PostalCode")]
        public string PostalCode { get; set; }
    }

    public class OrderShipmentEditEntry : OrderShipmentEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "OrderShipmentId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int OrderShipmentId { get; set; }
    }
    public class OrderShipmentDetail : DtoBase
    {
        public int OrderShipmentId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "OrderNo")]
        public Guid OrderNo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CustomerId")]
        public int? CustomerId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ShippingMethodId")]
        public int ShippingMethodId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ShipDate")]
        public DateTime? ShipDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Weight")]
        public decimal? Weight { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsInternational")]
        public bool? IsInternational { get; set; }


        [Display(ResourceType = typeof(LanguageResource), Name = "ReceiverName")]
        public string ReceiverName { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Email")]
        public string ReceiverEmail { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Address")]
        public string ReceiverAddress { get; set; }
        
        [Display(ResourceType = typeof(LanguageResource), Name = "Phone")]
        public string ReceiverPhone { get; set; }


        [Display(ResourceType = typeof(LanguageResource), Name = "CountryId")]
        public int? CountryId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CityId")]
        public int? CityId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ProvinceId")]
        public int? ProvinceId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "RegionId")]
        public int? RegionId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PostalCode")]
        public string PostalCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CreatedDate")]
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

        [Display(ResourceType = typeof(LanguageResource), Name = "LastModifiedDate")]
        public DateTime? LastModifiedDate { get; set; }
    }

    #endregion

    #region Order Temp

    public class OrderTempDetail : DtoBase
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "OrderId")]
        public int OrderId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "OrderNo")]
        public Guid OrderNo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "OrderDate")]
        public DateTime OrderDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "DueDate")]
        public DateTime? DueDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ShippingRate")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Currency)]
        public decimal? ShippingRate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PromotionCode")]
        public string PromotionCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Deposit")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Currency)]
        public decimal? Deposit { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SubTotal")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Currency)]
        public decimal? SubTotal { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TotalFees")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Currency)]
        public decimal? TotalFees { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Discount")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Currency)]
        public decimal? Discount { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Tax")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        public decimal? Tax { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CurrencyCode")]
        public string CurrencyCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Comment")]
        public string Comment { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MarkAsRead")]
        public MarkAsRead MarkAsRead { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        [EnumDataType(typeof(OrderStatus))]
        public OrderStatus Status { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CreatedDate")]
        public DateTime? CreatedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LastModifiedDate")]
        public DateTime? LastModifiedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "VendorId")]
        public int VendorId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TransactionMethodId")]
        public int? TransactionMethodId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CustomerId")]
        public int CustomerId { get; set; }
    }

    public class OrderTempInfoDetail : OrderTempDetail
    {
        public CustomerInfoDetail Customer { get; set; }
        public PromotionInfo PromotionInfo { get; set; }
        public ShipmentInfo ShipmentInfo { get; set; }
        public TransactionMethodDetail TransactionMethod { get; set; }
        public OrderPaymentDetail OrderPayment { get; set; }
        public OrderShipmentDetail Shipment { get; set; }
        public PaymentMethodDetail PaymentMethod { get; set; }
        public IEnumerable<OrderProductInfoDetail> OrderProducts { get; set; }
    }


    #endregion

    #region Order Product Temp

    public class OrderProductTempEntry : DtoBase
    {
        public OrderProductTempEntry()
        {
            TypeId = ItemType.Product;
            EmployeeId = 1;
            PeriodGroupId = 1;
            FromPeriod = 33;
            ToPeriod = 43;
            StartDate = DateTime.UtcNow.Date;
            EndDate = DateTime.UtcNow.Date;
        }

        [Display(ResourceType = typeof(LanguageResource), Name = "OrderNo")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public Guid OrderNo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TypeId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public ItemType TypeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ProductId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int ProductId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CustomerId")]
        public int? CustomerId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EmployeeId")]
        public int? EmployeeId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Quantity")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "GreaterThanZero")]
        public int Quantity { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Weight")]
        public decimal? Weight { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PeriodGroupId")]
        public int? PeriodGroupId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "StartDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public DateTime StartDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "EndDate")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "From")]
        public int? FromPeriod { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "To")]
        public int? ToPeriod { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "NetPrice")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public decimal? NetPrice { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "GrossPrice")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        public decimal? GrossPrice { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TaxRate")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        public decimal? TaxRate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "DiscountRate")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        public decimal? DiscountRate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CurrencyCode")]
        public string CurrencyCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Comment")]
        public string Comment { get; set; }
    }
    public class OrderProductTempEditEntry : OrderProductTempEntry
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "OrderProductId")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public int OrderProductId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        [Required(ErrorMessageResourceType = typeof(LanguageResource), ErrorMessageResourceName = "Required")]
        public OrderProductStatus Status { get; set; }

        public ItemDetail Item { get; set; }
        public CustomerInfoDetail Customer { get; set; }
        public EmployeeInfoDetail Employee { get; set; }
    }
    public class OrderProductTempDetail : DtoBase
    {
        public int OrderProductId { get; set; }
        public Guid OrderNo { get; set; }
        public int? EmployeeId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal? Weight { get; set; }
        public decimal? NetPrice { get; set; }
        public decimal? GrossPrice { get; set; }
        public decimal? TaxRate { get; set; }
        public decimal? DiscountRate { get; set; }
        public int? PeriodGroupId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? FromPeriod { get; set; }
        public int? ToPeriod { get; set; }
        public string CurrencyCode { get; set; }
        public string Comment { get; set; }
        public OrderProductTempStatus Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }

    public class OrderProductTempInfoDetail : OrderProductTempDetail
    {

    }
    #endregion
    
    #region TRANSACTION

    public class TransactionState
    {
        public List<RuleViolation> Errors { get; set; }
        public OrderDetail Order { get; set; }
    }

    public enum PaymentMethodSetting
    {
        Offline = 0,
        Stripe = 1,
        Paypal = 2
    }

    public class PaymentState
    {
        public static string Approved = "approved";
        public static string Created = "created";
        public static string Cancelled = "cancelled";
    }

    public class TransactionRedirectUrls : DtoBase
    {
        //Url where the payer would be redirected to after approving the payment. 
        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "return_url")]
        public string ReturnUrl { get; set; }

        //Url where the payer would be redirected to after canceling the payment.
        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore, PropertyName = "cancel_url")]
        public string CancelUrl { get; set; }
    }

    public class OrderTransactionEntry : DtoBase
    { 
        [Display(ResourceType = typeof(LanguageResource), Name = "OrderId")]
        public int OrderId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "OrderNo")]
        public Guid OrderNo { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "OrderDate")]
        public DateTime OrderDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "DueDate")]
        public DateTime? DueDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "ShippingRate")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Currency)]
        public decimal? ShippingRate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PromotionCode")]
        public string PromotionCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Deposit")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Currency)]
        public decimal? Deposit { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "SubTotal")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Currency)]
        public decimal? SubTotal { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TotalFees")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Currency)]
        public decimal? TotalFees { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Discount")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Currency)]
        public decimal? Discount { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Tax")]
        [DisplayFormat(DataFormatString = "{0:#,###}", ApplyFormatInEditMode = true)]
        public decimal? Tax { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CurrencyCode")]
        public string CurrencyCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Comment")]
        public string Comment { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "MarkAsRead")]
        public MarkAsRead MarkAsRead { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "Status")]
        [EnumDataType(typeof(OrderStatus))]
        public OrderStatus Status { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CreatedDate")]
        public DateTime? CreatedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "LastModifiedDate")]
        public DateTime? LastModifiedDate { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "VendorId")]
        public int VendorId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "TransactionMethodId")]
        public int? TransactionMethodId { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "CustomerId")]
        public int CustomerId { get; set; }

        public OrderPaymentEntry Payment { get; set; }
        public OrderShipmentEntry Shipment { get; set; }
        public IEnumerable<OrderProductInfoDetail> OrderProducts { get; set; }
        public TransactionRedirectUrls RedirectUrls { get; set; }
    }
   
    #endregion
}