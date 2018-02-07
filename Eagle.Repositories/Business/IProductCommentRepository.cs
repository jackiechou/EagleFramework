using System.Collections.Generic;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Products;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business
{
    public interface IProductCommentRepository : IRepositoryBase<ProductComment>
    {
        IEnumerable<ProductComment> GetProductComments(string email, ProductCommentStatus? status, ref int? recordCount,
          string orderBy = null, int? page = null, int? pageSize = null);

        IEnumerable<ProductComment> GetProductComments(int? productId, ProductCommentStatus? status);
    }
}
