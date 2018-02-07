using Eagle.Entities.Business.Orders;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Business
{
    public class OrderProductOptionRepository: RepositoryBase<OrderProductOption>, IOrderProductOptionRepository
    {
        public OrderProductOptionRepository(IDataContext dataContext) : base(dataContext) { }

    }
}
