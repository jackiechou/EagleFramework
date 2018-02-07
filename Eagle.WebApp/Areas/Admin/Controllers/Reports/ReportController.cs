using System.Web.Mvc;
using Eagle.Services;
using Eagle.Services.Business;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Reports
{
    public class ReportController : BaseController
    {
        private ICustomerService CustomerService { get; set; }
        private IOrderService OrderService { get; set; }

        public ReportController(ICustomerService customerService, IOrderService orderService) : base(new IBaseService[] { customerService, orderService })
        {
            CustomerService = customerService;
            OrderService = orderService;
        }

        // GET: Admin/Report
        public ActionResult Index()
        {
            return View("../Reports/Index");
        }

        /*Home page - Dashboard - Biểu đồ cơ cấu nhân viên */
        //[SessionExpiration]
        //public JsonResult PieChart(int? LSCompanyID)
        //{
        //    List<chart> EmployeeList = new List<chart>();
        //    var result = db.Common_spGetTotalEmployee(LSCompanyID).ToList();
        //    if (result != null && result.Count > 0)
        //    {
        //        int? tmpTotal = result.Sum(p => p.Total);
        //        if (tmpTotal != null)
        //        {
        //            double Total = (double)tmpTotal.Value;
        //            foreach (var item in result)
        //            {
        //                EmployeeList.Add(new chart() { label = LanguageId == 124 ? item.Name : item.VNName, data = (((double)item.Total) * 1000 / Total).ToString("#,##0.0"), total = item.Total });
        //            }
        //        }
        //    }

        //    return base.Json(EmployeeList, JsonRequestBehavior.AllowGet);
        //}


        /*Home page - Dashboard - Biều đồ tăng giảm nhân viên */
        //[SessionExpiration]
        //public JsonResult LineChart(int? Year,int? Type)
        //{
        //    //Nếu type = 1: Lấy theo các tháng trong năm
        //    //Nếu type = 2: lấy tất cả theo từng năm
        //    if (Type == null)
        //    {
        //        Type = 1;
        //    }
        //    if (Year == null)
        //    {
        //        Year = DateTime.UtcNow.Year;
        //    }

        //    List<lineChart> lineChartList = new List<lineChart>();
        //    var result = db.Common_spDashboard_StaffMovement(Year, Type).ToList();


        //    foreach (var item in result.Select(p => new { p.CompanyName, p.LSCompanyID }).Distinct())
        //    {
        //        //Tạo note cha
        //        lineChart c = new lineChart()
        //        {
        //            label = item.CompanyName,
        //            data = new List<double[]>()
        //        };
        //        //Tạo dữ liệu
        //        foreach (var data in result.Where(p => p.LSCompanyID == item.LSCompanyID))
        //        {
        //            double[] tmp = new double[2];
        //            tmp[0] = (double)data.Time;
        //            tmp[1] = (double)data.TotalStaff;

        //            c.data.Add(tmp);
        //        }
        //        lineChartList.Add(c);
        //    }
        //    return base.Json(lineChartList, JsonRequestBehavior.AllowGet);
        //}

        #region Dispose

        private bool _disposed;

        [NonAction]
        protected override void Dispose(bool isDisposing)
        {
            if (!_disposed)
            {
                if (isDisposing)
                {
                    CustomerService = null;
                    OrderService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}