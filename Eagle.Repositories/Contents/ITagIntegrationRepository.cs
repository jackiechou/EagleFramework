using System.Collections.Generic;
using Eagle.Entities.Contents.Tags;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Contents
{
    public interface ITagIntegrationRepository : IRepositoryBase<TagIntegration>
    {
        IEnumerable<TagIntegration> GetTagIntegrationsByKey(int tagKey);
        TagIntegration GetTagIntegrationByTagId(int tagId);
    }
}
