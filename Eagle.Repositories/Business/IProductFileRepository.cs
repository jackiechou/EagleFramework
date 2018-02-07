using System.Collections.Generic;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Products;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business
{
    public interface IProductFileRepository : IRepositoryBase<ProductFile>
    {
        IEnumerable<ProductFile> GetList(ProductFileStatus? status, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<ProductFile> GetList(int productId, ProductFileStatus? status);
        bool HasDataExisted(int productId, string fileName);
    }
}
