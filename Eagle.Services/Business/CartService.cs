using Eagle.Core.Settings;
using Eagle.Repositories;
using System;
using System.Linq;

namespace Eagle.Services.Business
{
    public class CartService : BaseService, ICartService
    {
        private IProductService ProductService { get; set; }

        public CartService(IUnitOfWork unitOfWork, IProductService productService) : base(unitOfWork)
        {
            this.ProductService = productService;
        }
        public void ValidateCart(ShoppingCart cart)
        {
            if (cart == null || !cart.Items.Any()) return;

            foreach (var item in cart.Items)
            {
                var productEntity = ProductService.GetProductDetail(item.Id);
                if (productEntity != null)
                {
                    if (productEntity.Status == ProductStatus.InActive)
                    {
                        item.Status = CartItemStatus.NotBeenSalesYet;
                    }
                    else if ((productEntity.StartDate.HasValue && productEntity.StartDate.Value > DateTime.UtcNow) || (productEntity.EndDate.HasValue && productEntity.EndDate.Value < DateTime.UtcNow))
                    {
                        item.Status = CartItemStatus.NotBeenSalesYet;
                    }
                    else if (productEntity.UnitsInStock.HasValue && productEntity.UnitsInStock.Value <= 0)
                    {
                        item.UnitsInStock = 0;
                        item.Status = CartItemStatus.OutOfStock;
                    }
                    else if (productEntity.UnitsInStock.HasValue && productEntity.UnitsInStock.Value < item.Quantity)
                    {
                        item.UnitsInStock = productEntity.UnitsInStock.Value;
                        item.Status = CartItemStatus.ExceedUnitOfStock;
                    } else
                    {
                        item.Status = CartItemStatus.Available;
                    }
                }
            }
        }
       
        #region Dispose

        private bool _disposed;
        protected override void Dispose(bool isDisposing)
        {
            if (!_disposed)
            {
                if (isDisposing)
                {
                    ProductService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }
        #endregion
    }
}
