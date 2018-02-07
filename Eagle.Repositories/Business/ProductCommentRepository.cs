using System;
using System.Collections.Generic;
using System.Linq;
using Eagle.Core.Common;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Products;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Business
{
    public class ProductCommentRepository : RepositoryBase<ProductComment>, IProductCommentRepository
    {
        public ProductCommentRepository(IDataContext dataContext) : base(dataContext)
        {
        }

        public IEnumerable<ProductComment> GetProductComments(string email, ProductCommentStatus? status, ref int? recordCount,
          string orderBy = null, int? page = null, int? pageSize = null)
        {
            var query = from d in DataContext.Get<ProductComment>()
                        where (status == null || d.IsActive == status)
                        select d;
            
            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(x => string.Equals(x.CommentEmail, email, StringComparison.CurrentCultureIgnoreCase));
            }

            return query.WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }

        public IEnumerable<ProductComment> GetProductComments(int? productId, ProductCommentStatus? status)
        {
            if (productId == null || productId== 0) return null;

            var query = from x in DataContext.Get<ProductComment>()
                        where (x.ProductId == productId) && (status == null || x.IsActive == status)
                        select x;
          
            return query.AsEnumerable();
        }
    }
}
