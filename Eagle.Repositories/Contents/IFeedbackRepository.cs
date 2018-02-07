using System.Collections.Generic;
using Eagle.Entities.Contents.Feedbacks;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Contents
{
    public interface IFeedbackRepository : IRepositoryBase<Feedback>
    {
        IEnumerable<Feedback> GetFeedbacks(ref int? recordCount, bool? status = null,
        string orderBy = null, int? page = null, int? pageSize = null);
    }
}
