using System.Collections.Generic;
using System.Linq;
using Eagle.Core.Common;
using Eagle.Core.Settings;
using Eagle.Entities.Contents.Media;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Contents
{
    public class MediaComposerRepository: RepositoryBase<MediaComposer>, IMediaComposerRepository
    {
        public MediaComposerRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<MediaComposer> GetMediaComposers(string composerName, MediaComposerStatus? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var queryable = (from t in DataContext.Get<MediaComposer>()
                       where (status == null || t.Status == status)
                       select t);
            if (!string.IsNullOrEmpty(composerName))
            {
                queryable = queryable.Where(x => x.ComposerName.ToLower().Contains(composerName.ToLower())
                        || x.Description.ToLower().Contains(composerName.ToLower()));
            }

            return queryable.AsEnumerable().WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }
        public IEnumerable<MediaComposer> GetMediaComposers(MediaComposerStatus? status)
        {
            return (from t in DataContext.Get<MediaComposer>()
                where (status == null || t.Status == status)
                select t).AsEnumerable();
        }
        public int GetNewListOrder()
        {
            int listOrder = 1;
            var query = from u in DataContext.Get<MediaComposer>() select (int)u.ListOrder;
            if (query.Any())
            {
                listOrder = query.Max() + 1;
            }
            return listOrder;
        }


    }
}
