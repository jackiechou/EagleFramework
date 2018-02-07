using System.Collections.Generic;
using System.Linq;
using Eagle.Core.Common;
using Eagle.Core.Settings;
using Eagle.Entities.Contents.Media;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Contents
{
    public class MediaArtistRepository: RepositoryBase<MediaArtist>, IMediaArtistRepository
    {
        public MediaArtistRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<MediaArtist> GetMediaArtists(string artistName, MediaArtistStatus? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var queryable = (from t in DataContext.Get<MediaArtist>()
                       where (status == null || t.Status == status)
                       select t);

            if (!string.IsNullOrEmpty(artistName))
            {
                queryable = queryable.Where(x => x.ArtistName.ToLower().Contains(artistName.ToLower())
                        || x.Description.ToLower().Contains(artistName.ToLower()));
            }
            return queryable.AsEnumerable().WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }
        public int GetNewListOrder()
        {
            int listOrder = 1;
            var query = from u in DataContext.Get<MediaArtist>() select (int)u.ListOrder;
            if (query.Any())
            {
                listOrder = query.Max() + 1;
            }
            return listOrder;
        }
    }
}
