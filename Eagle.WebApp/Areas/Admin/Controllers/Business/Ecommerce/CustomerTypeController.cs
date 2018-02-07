using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Core.Pagination;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services.Business;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;
using System;
using Eagle.Services;

namespace Eagle.WebApp.Areas.Admin.Controllers.Business.Ecommerce
{
    [ValidateAntiForgeryTokenOnAllPosts]
    public class CustomerTypeController : BaseController
    {
        private ICustomerService CustomerService { get; set; }
        public CustomerTypeController(ICustomerService customerService) : base(new IBaseService[] { customerService })
        {
            CustomerService = customerService;
        }
        #region GET METHODS =========================================================================
        //
        // GET: /Admin/CustomerType/
        public ActionResult Index()
        {
            return View("../Business/Ecommerce/CustomerType/Index");
        }

        // GET: /Admin/CustomerType/Details/5        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var entity = CustomerService.GetCustomerTypeDetail(id);

            var editModel = new CustomerTypeEditEntry
            {
                CustomerTypeId = entity.CustomerTypeId,
                CustomerTypeName = entity.CustomerTypeName,
                PromotionalRate = entity.PromotionalRate,
                Note = entity.Note,
                IsActive = entity.IsActive
            };
            return PartialView("../Business/Ecommerce/CustomerType/_Edit", editModel);
        }

        // GET: /Admin/CustomerType/Search
        [HttpGet]
        public ActionResult LoadSearchForm()
        {
            return PartialView("../Business/Ecommerce/CustomerType/_SearchForm");
        }

        // GET: /Admin/CustomerType/Search
        [HttpGet]
        public ActionResult Search(CustomerTypeSearchEntry filter, string sourceEvent, int? page = 1)
        {
            if (String.IsNullOrEmpty(sourceEvent))
            {
                TempData["CustomerTypeSearchRequest"] = filter;
            }
            else
            {
                if (TempData["CustomerTypeSearchRequest"] != null)
                {
                    filter = (CustomerTypeSearchEntry)TempData["CustomerTypeSearchRequest"];
                }
            }
            TempData.Keep();

            int? recordCount = 0;
            var sources = CustomerService.GetCustomerTypes(VendorId, filter, ref recordCount, "CustomerTypeId DESC", page, GlobalSettings.DefaultPageSize);
            int currentPageIndex = page - 1 ?? 0;
            var lst = sources.ToPagedList(currentPageIndex, GlobalSettings.DefaultPageSize, recordCount);
            return PartialView("../Business/Ecommerce/CustomerType/_SearchResult", lst);
        }

        #endregion =====================================================================================

        #region INSERT - UPDATE - DELETE METHODS =======================================================

        // POST: /Admin/CustomerType/Create
        [HttpPost]
        public ActionResult Create(CustomerTypeEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CustomerService.InsertCustomerType(UserId, VendorId, entry);
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

        //
        // PUT - /Admin/CustomerType/Edit
        [HttpPut]
        public ActionResult Edit(CustomerTypeEditEntry entry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CustomerService.UpdateCustomerType(UserId, entry);
                    return Json(new SuccessResult
                    {
                        Status = ResultStatusType.Success,
                        Data = new
                        {
                            Message = LanguageResource.UpdateSuccess,
                            Id = entry.CustomerTypeId
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

        //
        // POST: /Admin/CustomerType/UpdateStatus/5
        [HttpPut]
        public ActionResult UpdateStatus(int id, bool status)
        {
            try
            {
                CustomerService.UpdateCustomerTypeStatus(UserId, id, status?CustomerTypeStatus.Active : CustomerTypeStatus.InActive);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.UpdateStatusSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // DELETE: /Admin/CustomerType/Delete/5
        [HttpDelete]
        public ActionResult Delete([System.Web.Http.FromBody]int id)
        {
            try
            {
                CustomerService.UpdateCustomerTypeStatus(UserId, id, CustomerTypeStatus.InActive);
                return Json(new SuccessResult { Status = ResultStatusType.Success, Data = LanguageResource.DeleteSuccess }, JsonRequestBehavior.AllowGet);
            }
            catch (ValidationError ex)
            {
                var errors = ValidationExtension.GetException(ex);
                return Json(new FailResult { Status = ResultStatusType.Fail, Errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion ==============================================================================

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
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}