using System.Collections.Generic;
using Eagle.Core.Settings;
using Eagle.Entities.Contents.Tags;
using Eagle.Services.Dtos.Contents.Tags;

namespace Eagle.Services.Contents
{
    /// <summary>
    /// Interface for tag service.
    /// Defines methods for tag use on various feature
    /// </summary>
    public interface ITagService : IBaseService
    {
        IEnumerable<TagDetail> GetTags(SearchTagEntry searchTagEntry, ref int? recordCount, int? page = null, int? pageSize = null);
        void CreateTag(TagEntry entry);
        void EditTag(int tagId, TagEntry entry);
        void DeleteTagByTagId(int tagId);


        IEnumerable<TagDetail> GetTagsByTagKey(int tagKey);
        void DeleteTagsByTagKey(int tagKey);
        void UpdateTagIntegrationStatus(int tagId, TagStatus status);
    }
}
