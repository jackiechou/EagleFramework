using System.Collections.Generic;
using Eagle.Core.Settings;
using Eagle.Entities.Contents.Media;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Contents
{
    public interface IMediaArtistRepository : IRepositoryBase<MediaArtist>
    {
        IEnumerable<MediaArtist> GetMediaArtists(string artistName, MediaArtistStatus? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        int GetNewListOrder();
    }
}
