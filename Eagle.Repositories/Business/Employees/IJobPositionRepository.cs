using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Entities.Business.Employees;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Business.Employees
{
    public interface IJobPositionRepository : IRepositoryBase<JobPosition>
    {
        IEnumerable<JobPosition> GetJobPositions(string searchText, bool? status,
            ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null);

        IEnumerable<JobPosition> GetJobPositions(bool? status);
        SelectList PopulateJobPositionSelectList(bool? status = null, int? selectedValue = null, bool? isShowSelectText = true);
        SelectList PopulateJobPositionStatus(bool? selectedValue = true, bool isShowSelectText = false);
        bool HasDataExisted(string positionName);
    }
}
