using System.Collections.Generic;
using System.Linq;
using Eagle.Core.Common;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Products;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Business
{
    public class ProductFileRepository : RepositoryBase<ProductFile>, IProductFileRepository
    {
        public ProductFileRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<ProductFile> GetList(ProductFileStatus? status, ref int? recordCount,
      string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = DataContext.Get<ProductFile>().Where(x => (status == null || x.Status == status));
            return lst.WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }
        public IEnumerable<ProductFile> GetList(int productId, ProductFileStatus? status)
        {
            return (from f in DataContext.Get<ProductFile>()
                    where f.ProductId == productId && (status==null || f.Status== status)
                    select f).AsEnumerable();
        }

        public bool HasDataExisted(int productId, string fileName)
        {
            var entity =
                DataContext.Get<ProductFile>().FirstOrDefault(x =>
                x.ProductId == productId
                && x.FileName.Contains(fileName)
                && x.Status == ProductFileStatus.Active
                );
            return entity != null;
        }
    }
}
