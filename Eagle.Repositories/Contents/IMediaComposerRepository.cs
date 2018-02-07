using System.Collections.Generic;
using Eagle.Core.Settings;
using Eagle.Entities.Contents.Media;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Contents
{
    public interface IMediaComposerRepository : IRepositoryBase<MediaComposer>
    {
        IEnumerable<MediaComposer> GetMediaComposers(string composerName, MediaComposerStatus? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        IEnumerable<MediaComposer> GetMediaComposers(MediaComposerStatus? status);
        int GetNewListOrder();
    }
}
