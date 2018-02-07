using System;
using System.Collections.Generic;
using System.Linq;
using Eagle.Core.Common;
using Eagle.Core.Settings;
using Eagle.Entities.Contents.Tags;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Contents
{
    public class TagRepository : RepositoryBase<Tag>, ITagRepository
    {
        public TagRepository(IDataContext dataContext): base(dataContext){}
        public IEnumerable<Tag> GetTags(string tagName, TagType? type, ref int? recordCount, int? page = null, int? pageSize = null)
        {
            var queryable = DataContext.Get<Tag>();

            if (type != null)
                queryable = queryable.Where(m => m.TagType == type);
           
            if (!string.IsNullOrEmpty(tagName))
            {
                queryable = queryable.Where(x =>
                    string.Equals(x.TagName, tagName, StringComparison.CurrentCultureIgnoreCase));
            }

            if (recordCount != null)
            {
                recordCount = queryable.Count();
            }

            queryable = queryable.OrderBy(m => m.TagName);

            if (page != null && pageSize != null)
            {
                queryable = queryable.ApplyPaging(page.Value, pageSize.Value);
            }

            return queryable.AsEnumerable();
        }
        public IEnumerable<Tag> GetTagsByTagIds(IEnumerable<int> tagIds)
        {
            var queryable = DataContext.Get<Tag>().Where(t => tagIds.Contains(t.TagId));
            return queryable.AsEnumerable();
        }
        public Tag GetTagsByName(TagType type, string tagName)
        {
            return DataContext.Get<Tag>().FirstOrDefault(x => x.TagType == type && string.Equals(x.TagName, tagName, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
