using Eagle.Repositories;

namespace Eagle.Services.Business
{
    public class ReportService : BaseService, IReportService
    {
        public ReportService(IUnitOfWork unitOfWork) : base(unitOfWork){ }


    }
}
