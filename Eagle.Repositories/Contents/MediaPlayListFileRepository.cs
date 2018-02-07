using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Entities.Contents.Media;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.Contents
{
    public class MediaPlayListFileRepository : RepositoryBase<MediaPlayListFile>, IMediaPlayListFileRepository
    {
        public MediaPlayListFileRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<MediaPlayListFile> GetListByPlayListId(int playListId, MediaPlayListFileStatus? status = null)
        {
            return (from x in DataContext.Get<MediaPlayListFile>()
                    where x.PlayListId == playListId && (status == null || x.Status == status)
                    orderby x.ListOrder
                    select x).AsEnumerable();
        }

        public IEnumerable<MediaPlayListFile> GetListByFileId(int fileId, MediaPlayListFileStatus? status = null)
        {
            return (from x in DataContext.Get<MediaPlayListFile>()
                    where x.FileId == fileId && (status == null || x.Status == status)
                    orderby x.ListOrder
                    select x).AsEnumerable();
        }
        public MediaPlayListFile GetDetails(int playListId, int fileId)
        {
            return (from pm in DataContext.Get<MediaPlayListFile>()
                    where pm.PlayListId == playListId && pm.FileId == fileId
                    select pm).FirstOrDefault();
        }
        public int GetNewListOrder()
        {
            int listOrder = 1;
            var query = from u in DataContext.Get<MediaPlayListFile>() select (int)u.ListOrder;
            if (query.Any())
            {
                listOrder = query.Max() + 1;
            }
            return listOrder;
        }
    }
}
