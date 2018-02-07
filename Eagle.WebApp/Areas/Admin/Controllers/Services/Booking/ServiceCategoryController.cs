using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Services.Booking;
using Eagle.Services.Service;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Services.Booking
{
    public class ServiceCategoryController : BaseController
    {
        private IBookingService BookingService { get; set; }

        public ServiceCategoryController(IBookingService bookingService) : base(new IBaseService[] { bookingService })
        {
            BookingService = bookingService;
        }

        // GET: Admin/ServiceCategory
        public ActionResult Index()
        {
            return View("../Services/Booking/ServiceCategory/Index");
        }

        [HttpGet]
        public ActionResult LoadTreeList()
        {
            return PartialView("../Services/Booking/ServiceCategory/_TreeList");
        }

        // GET: Hierachical List 
        [HttpGet]
        public ActionResult GetServiceCategorySelectTree(ServiceType typeId = ServiceType.Single, int? selectedId = null, bool? isRootShowed = true)
        {
            var list = BookingService.GetServiceCategorySelectTree(typeId, null, selectedId, isRootShowed);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //// GET: Hierachical List 
        //[HttpGet]
        //public ActionResult GetServiceCategorySelectTree(int? selectedId = null, bool? isRootShowed = true)
        //{
        //    var list = BookingService.GetServiceCategorySelectTree(null, selectedId, isRootShowed);
        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}

        //
        // GET: /Admin/ServiceCategory/Create       
        [HttpGet]
        public ActionResult Create()
        {
            return PartialView("../Services/Booking/ServiceCategory/_Create");
        }
        //
        // GET: /Admin/ServiceCategory/Details/5        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var item = BookingService.GetServiceCategoryDetail(id);

            var entry = new ServiceCategoryEditEntry
            {
                TypeId = item.TypeId,
                CategoryId = item.CategoryId,
                CategoryName = item.CategoryName,
                ParentId = item.ParentId,
                Status = item.Status
            };
            return PartialView("../Services/Booking/ServiceCategory/_Edit", entry);
        }


        //
        // POST: /Admin/ServiceCategory/Create-
        [HttpPost]
        public ActionResult Create(ServiceCategoryEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BookingService.InsertServiceCategory(UserId, entry);
                    return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.CreateSuccess }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var modeStateErrors = ModelState.GetModelStateErrors();
                    return Json(new FailResult { Status = ResultStatusType.Fail, Errors = modeStateErrors }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        ////
        //// PUT: /Admin/ServiceCategory/Edit/5
        [HttpPut]
        public ActionResult Edit(ServiceCategoryEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BookingService.UpdateServiceCategory(UserId, entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.CategoryId
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var modeStateErrors = ModelState.GetModelStateErrors();
                    return Json(new FailResult { Status = ResultStatusType.Fail, Errors = modeStateErrors }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        ////
        //// PUT: /Admin/ServiceCategory/UpdateListOrder
        [HttpPut]
        public ActionResult UpdateServiceCategoryListOrder(int id, int listOrder)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    BookingService.UpdateServiceCategoryListOrder(UserId, id, listOrder);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = id
                        }
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var modeStateErrors = ModelState.GetModelStateErrors();
                    return Json(new FailResult { Status = ResultStatusType.Fail, Errors = modeStateErrors }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: /Admin/ServiceCategory/UpdateStatus/5
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                BookingService.UpdateServiceCategoryStatus(UserId, id, status ? ServiceCategoryStatus.Active : ServiceCategoryStatus.InActive);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // PUT: /Admin/ServiceCategory/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, ServiceCategoryStatus status)
        {
            try
            {
                BookingService.UpdateServiceCategoryStatus(UserId, id, status);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.DeleteSuccess }, JsonRequestBehavior.AllowGet);

            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // POST: /Admin/ServiceCategory/Delete/5
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            try
            {
                BookingService.DeleteServiceCategory(id);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.DeleteSuccess }, JsonRequestBehavior.AllowGet);

            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }


        #region Dispose

        private bool _disposed;

        [NonAction]
        protected override void Dispose(bool isDisposing)
        {
            if (!_disposed)
            {
                if (isDisposing)
                {
                    BookingService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}