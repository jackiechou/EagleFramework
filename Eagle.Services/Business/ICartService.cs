using Eagle.Services.Dtos.Business;

namespace Eagle.Services.Business
{
    public interface ICartService
    {
        void ValidateCart(ShoppingCart cart);
    }
}
