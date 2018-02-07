using System.Collections.Generic;
using System.Linq;
using Eagle.Core.Common;
using Eagle.Entities.Contents.Feedbacks;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Contents
{
    public class FeedbackRepository : RepositoryBase<Feedback>, IFeedbackRepository
    {
        public FeedbackRepository(IDataContext dataContext) : base(dataContext)
        {
        }

        public IEnumerable<Feedback> GetFeedbacks(ref int? recordCount, bool? status = null,
        string orderBy = null, int? page = null, int? pageSize = null)
        {
            var query = (from x in DataContext.Get<Feedback>()
                    where (status == null || x.IsActive == status)
                    select x);
            return query.WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);

        }

    }
}
