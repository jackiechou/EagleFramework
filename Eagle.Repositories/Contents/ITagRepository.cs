using System.Collections.Generic;
using Eagle.Core.Settings;
using Eagle.Entities.Contents.Tags;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Contents
{
    public interface ITagRepository : IRepositoryBase<Tag>
    {
        IEnumerable<Tag> GetTags(string tagName, TagType? type, ref int? recordCount, int? page = null, int? pageSize = null);
        IEnumerable<Tag> GetTagsByTagIds(IEnumerable<int> tagIds);
        Tag GetTagsByName(TagType type, string tagName);
    }
}
