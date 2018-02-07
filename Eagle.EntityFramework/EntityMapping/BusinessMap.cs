using System.Data.Entity;
using Eagle.Entities.Business.Brand;
using Eagle.Entities.Business.Companies;
using Eagle.Entities.Business.Customers;
using Eagle.Entities.Business.Employees;
using Eagle.Entities.Business.Manufacturers;
using Eagle.Entities.Business.Orders;
using Eagle.Entities.Business.Products;
using Eagle.Entities.Business.Roster;
using Eagle.Entities.Business.Shipping;
using Eagle.Entities.Business.Transactions;
using Eagle.Entities.Business.Vendors;

namespace Eagle.EntityFramework.EntityMapping
{
    public static class BusinessMap
    {
        public static void Configure(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().ToTable("dbo.Company");

            modelBuilder.Entity<Manufacturer>().ToTable("Production.Manufacturer");
            modelBuilder.Entity<ManufacturerCategory>().ToTable("Production.ManufacturerCategory");

            modelBuilder.Entity<Product>().ToTable("Production.Product");
            modelBuilder.Entity<ProductAttribute>().ToTable("Production.ProductAttribute");
            modelBuilder.Entity<ProductAttributeOption>().ToTable("Production.ProductAttributeOption");
            modelBuilder.Entity<ProductCategory>().ToTable("Production.ProductCategory");
            modelBuilder.Entity<ProductComment>().ToTable("Production.ProductComment");
            modelBuilder.Entity<ProductDiscount>().ToTable("Production.ProductDiscount");
            modelBuilder.Entity<ProductFile>().ToTable("Production.ProductFile");
            modelBuilder.Entity<ProductTaxRate>().ToTable("Production.ProductTaxRate");
            modelBuilder.Entity<ProductType>().ToTable("Production.ProductType");
            modelBuilder.Entity<ProductVote>().ToTable("Production.ProductVote");
            modelBuilder.Entity<ProductRating>().ToTable("Production.ProductRating");
            modelBuilder.Entity<ProductAlbum>().ToTable("Production.ProductAlbum");
            modelBuilder.Entity<Brand>().ToTable("Production.Brand");


            modelBuilder.Entity<Customer>().ToTable("Sales.Customer");
            modelBuilder.Entity<CustomerType>().ToTable("Sales.CustomerType");
            modelBuilder.Entity<PaymentMethod>().ToTable("Sales.PaymentMethod");
            modelBuilder.Entity<Promotion>().ToTable("Sales.Promotion");
            modelBuilder.Entity<ShippingCarrier>().ToTable("Sales.ShippingCarrier");
            modelBuilder.Entity<ShippingMethod>().ToTable("Sales.ShippingMethod");
            modelBuilder.Entity<ShippingFee>().ToTable("Sales.ShippingFee");
            modelBuilder.Entity<TransactionMethod>().ToTable("Sales.TransactionMethod");

            modelBuilder.Entity<Order>().ToTable("Sales.Order");
            modelBuilder.Entity<OrderTemp>().ToTable("Sales.Order_Temp");
            modelBuilder.Entity<OrderPayment>().ToTable("Sales.OrderPayment");
            modelBuilder.Entity<OrderProduct>().ToTable("Sales.OrderProduct");
            modelBuilder.Entity<OrderProductTemp>().ToTable("Sales.OrderProduct_Temp");
            modelBuilder.Entity<OrderProductOption>().ToTable("Sales.OrderProductOption");
            modelBuilder.Entity<OrderShipment>().ToTable("Sales.OrderShipment");

            modelBuilder.Entity<CurrencyGroup>().ToTable("Purchasing.Currency");
            modelBuilder.Entity<CurrencyRate>().ToTable("Purchasing.CurrencyRate");
            modelBuilder.Entity<CreditCard>().ToTable("Purchasing.CreditCard");

            modelBuilder.Entity<Vendor>().ToTable("Purchasing.Vendor");
            modelBuilder.Entity<VendorAddress>().ToTable("Purchasing.VendorAddress");
            modelBuilder.Entity<VendorCreditCard>().ToTable("Purchasing.VendorCreditCard");
            modelBuilder.Entity<VendorCurrency>().ToTable("Purchasing.VendorCurrency");
            modelBuilder.Entity<VendorPartner>().ToTable("Purchasing.VendorPartner");
            modelBuilder.Entity<VendorShippingCarrier>().ToTable("Purchasing.VendorShippingCarrier");
            modelBuilder.Entity<VendorShippingMethod>().ToTable("Purchasing.VendorShippingMethod");
            modelBuilder.Entity<VendorPaymentMethod>().ToTable("Purchasing.VendorPaymentMethod");
            modelBuilder.Entity<VendorTransactionMethod>().ToTable("Purchasing.VendorTransactionMethod");

            modelBuilder.Entity<Contract>().ToTable("Personnel.Contract");
            modelBuilder.Entity<Employee>().ToTable("Personnel.Employee");
            modelBuilder.Entity<EmployeeAvailability>().ToTable("Personnel.EmployeeAvailability");
            modelBuilder.Entity<EmployeePosition>().ToTable("Personnel.EmployeePosition");
            modelBuilder.Entity<EmployeeSkill>().ToTable("Personnel.EmployeeSkill");
            modelBuilder.Entity<EmployeeTimeOff>().ToTable("Personnel.EmployeeTimeOff");
            modelBuilder.Entity<JobPosition>().ToTable("Personnel.JobPosition");
            modelBuilder.Entity<JobPositionSkill>().ToTable("Personnel.JobPositionSkill");
            modelBuilder.Entity<Qualification>().ToTable("Personnel.Qualification");
            modelBuilder.Entity<RewardDiscipline>().ToTable("Personnel.RewardDiscipline");
            modelBuilder.Entity<Salary>().ToTable("Personnel.Salary");
            modelBuilder.Entity<Skill>().ToTable("Personnel.Skill");
            modelBuilder.Entity<Termination>().ToTable("Personnel.Termination");
            modelBuilder.Entity<WorkingHistory>().ToTable("Personnel.WorkingHistory");

            modelBuilder.Entity<PublicHolidaySet>().ToTable("Roster.PublicHolidaySet");
            modelBuilder.Entity<PublicHoliday>().ToTable("Roster.PublicHoliday");
            modelBuilder.Entity<Shift>().ToTable("Roster.Shift");
            modelBuilder.Entity<ShiftPosition>().ToTable("Roster.ShiftPosition");
            modelBuilder.Entity<ShiftSwap>().ToTable("Roster.ShiftSwap");
            modelBuilder.Entity<ShiftType>().ToTable("Roster.ShiftType");
            modelBuilder.Entity<TimeOffType>().ToTable("Roster.TimeOffType");
            modelBuilder.Entity<Timesheet>().ToTable("Roster.Timesheet");

        }
    }
}
