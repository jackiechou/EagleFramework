using System;
using System.Collections.Generic;
using System.Linq;
using Eagle.Core.Settings;
using Eagle.Entities.Contents.Tags;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Contents
{
    public class TagIntegrationRepository : RepositoryBase<TagIntegration>, ITagIntegrationRepository
    {
        public TagIntegrationRepository(IDataContext dataContext): base(dataContext){ }
        public IEnumerable<TagIntegration> GetTagIntegrationsByKey(int tagKey)
        {
            var queryable = DataContext.Get<TagIntegration>().Where(t => t.TagKey == tagKey);
            return queryable.AsEnumerable();
        }

        public TagIntegration GetTagIntegrationByTagId(int tagId)
        {
            return DataContext.Get<TagIntegration>().FirstOrDefault(t => t.TagId == tagId);
        }
    }
}
