using System;
using System.Collections.Generic;
using Eagle.Core.Settings;

namespace Eagle.Services.Dtos.Business.Transaction
{
    /**
     * CartItem Class
     * 
     * Create cart contruction
     */
    [Serializable]
    public class CartItem : IEquatable<CartItem>
    {
        #region Properties
        
        public int Id { get; set; }
        public ItemType Type { get; set; }
        public string Name { get; set; }
        

        // A place to store the quantity in the cart      
        public int Quantity { get; set; }
        public decimal? Weight { get; set; }
        public decimal? GrossPrice { get; set; }
        public decimal? NetPrice { get; set; }
        public decimal? TaxRate { get; set; }
        public decimal? DiscountRate { get; set; }
        public string CurrencyCode { get; set; }
        public string Image { get; set; }
        public decimal? Amount => NetPrice * Quantity;
        public int? UnitsInStock { get; set; }
        public CartItemStatus Status { get; set; }

        #endregion

        // CartItem constructor just needs a productId or productNo
        public CartItem(int id)
        {
            Id = id;
        }

        /**
         * Equals() - Needed to implement the IEquatable interface
         *    Tests whether or not this item is equal to the parameter
         *    This method is called by the Contains() method in the List class
         *    We used this Contains() method in the ShoppingCart AddItem() method
         */
        public bool Equals(CartItem item)
        {
            return item != null && item.Id == Id;
        }
    }
    public class CartInfo : DtoBase
    {
        public string OrderCode { get; set; }
        public decimal Weights { get; set; }
        public decimal Tax { get; set; }
        public decimal Discount { get; set; }
        public decimal Promotion { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public string CurrencyCode { get; set; }
        public int Count { get; set; }
        public List<CartItem> Items { get; set; }

        public CustomerInfoDetail CustomerInfo { get; set; }
        public ShipmentInfo ShipmentInfo { get; set; }
        public PromotionInfo PromotionInfo { get; set; }
    }
    public class ShipmentInfo : DtoBase
    {
        public int ShippingMethodId { get; set; }
        public decimal? Weight { get; set; }
        public decimal? RateFee { get; set; }
        public decimal? PackageFee { get; set; }
        public decimal? Vat { get; set; }
        public decimal TotalShippingFee
        {
            get
            {
                decimal rateFee = RateFee ?? 0;
                decimal packageFee = PackageFee ?? 0;
                decimal vat = Vat ?? 0;
                return rateFee + packageFee + vat;
            }
        }
    }
    public class PromotionInfo : DtoBase
    {
        public int PromotionId { get; set; }
        public string PromotionCode { get; set; }
        public string PromotionTitle { get; set; }
        public decimal PromotionValue { get; set; }
        public bool IsPercent { get; set; }
    }
}
