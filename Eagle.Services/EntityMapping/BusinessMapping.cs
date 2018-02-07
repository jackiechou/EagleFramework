using AutoMapper;
using Eagle.Entities.Business.Brand;
using Eagle.Entities.Business.Customers;
using Eagle.Entities.Business.Employees;
using Eagle.Entities.Business.Manufacturers;
using Eagle.Entities.Business.Orders;
using Eagle.Entities.Business.Products;
using Eagle.Entities.Business.Shipping;
using Eagle.Entities.Business.Transactions;
using Eagle.Entities.Business.Vendors;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Business.Personnel;
using Eagle.Services.Dtos.Business.Roster;

namespace Eagle.Services.EntityMapping
{
    public class BusinessMapping
    {
        public static void ConfigureMapping()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Customer, CustomerDetail>();
                cfg.CreateMap<CustomerType, CustomerTypeDetail>();
                cfg.CreateMap<Employee, EmployeeDetail>();
                cfg.CreateMap<Contract, ContractDetail>();
                cfg.CreateMap<Manufacturer, ManufacturerDetail>();
                cfg.CreateMap<ManufacturerCategory, ManufacturerCategoryDetail>();
                cfg.CreateMap<Product, ProductDetail>();
                cfg.CreateMap<ProductCategory, ProductCategoryDetail>();
                cfg.CreateMap<ProductType, ProductTypeDetail>();
                cfg.CreateMap<ShippingCarrier, ShippingCarrierDetail>();
                cfg.CreateMap<ShippingFee, ShippingFeeDetail>();
                cfg.CreateMap<ShippingMethod, ShippingMethodDetail>();

                //cfg.CreateMap<CreditCard, CreditCardDetail>();
                //cfg.CreateMap<Currency, CurrencyDetail>();
                cfg.CreateMap<CurrencyRate, CurrencyRateDetail>();
                cfg.CreateMap<PaymentMethod, PaymentMethodDetail>();
                cfg.CreateMap<Promotion, PromotionDetail>();
                cfg.CreateMap<TransactionMethod, TransactionMethodDetail>();
                cfg.CreateMap<Vendor, VendorDetail>();

                cfg.CreateMap<Brand, BrandInfo>();
                cfg.CreateMap<Brand, BrandEditEntry>();
                cfg.CreateMap<Brand, BrandDetail>();
                cfg.CreateMap<Brand, BrandEntry>();
            });
        }
    }
}
