using System.Collections.Generic;
using System.Linq;
using Eagle.Core.Settings;
using Eagle.Entities.Contents.Media;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Contents
{
    public class MediaAlbumFileRepository: RepositoryBase<MediaAlbumFile>, IMediaAlbumFileRepository
    {
        public MediaAlbumFileRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<MediaAlbumFile> GetListByAlbumId(int albumId, MediaAlbumFileStatus? status = null)
        {
            return (from x in DataContext.Get<MediaAlbumFile>()
                    where x.AlbumId == albumId && (status == null || x.Status == status)
                    orderby x.ListOrder descending 
                    select x).AsEnumerable();
        }
        
        public IEnumerable<MediaAlbumFile> GetListByFileId(int fileId, MediaAlbumFileStatus? status = null)
        {
            return (from x in DataContext.Get<MediaAlbumFile>()
                    where x.FileId == fileId && (status == null || x.Status == status)
                    orderby x.ListOrder descending
                    select x).AsEnumerable();
        }
        public MediaAlbumFile GetDetails(int albumId, int fileId)
        {
            return (from pm in DataContext.Get<MediaAlbumFile>()
                    where pm.AlbumId == albumId && pm.FileId == fileId
                    select pm).FirstOrDefault();
        }
        public int GetNewListOrder()
        {
            int listOrder = 1;
            var query = from u in DataContext.Get<MediaAlbumFile>() select (int)u.ListOrder;
            if (query.Any())
            {
                listOrder = query.Max() + 1;
            }
            return listOrder;
        }
    }
}
