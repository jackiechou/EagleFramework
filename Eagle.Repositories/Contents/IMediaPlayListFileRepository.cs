using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Entities.Contents.Media;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Contents
{
   public interface IMediaPlayListFileRepository : IRepositoryBase<MediaPlayListFile>
   {
        IEnumerable<MediaPlayListFile> GetListByPlayListId(int playListId, MediaPlayListFileStatus? status = null);
       IEnumerable<MediaPlayListFile> GetListByFileId(int fileId, MediaPlayListFileStatus? status = null);
        MediaPlayListFile GetDetails(int playListId, int fileId);
        int GetNewListOrder();
   }
}
