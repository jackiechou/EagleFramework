using System.Linq;
using System.Web.Mvc;
using Eagle.Common.Utilities;
using Eagle.Services;
using Eagle.Services.Dtos.SystemManagement;
using Eagle.Services.SystemManagement;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;
using Eagle.WebApp.Attributes.Session;

namespace Eagle.WebApp.Areas.Admin.Controllers.Sys
{
    public class TaxonomyController : BaseController
    {
        private IContentService ContentService { get; set; }

        public TaxonomyController(IContentService contentService) : base(new IBaseService[] { contentService })
        {
            ContentService = contentService;
        }
        //
        // GET: /Admin/ContentItem/
        [SessionExpiration]
        public ActionResult Index(int contentTypeId)
        {
            return View("../Sys/Taxonomy/Index");
        }

        [SessionExpiration]
        [HttpGet]
        public ActionResult List(int contentTypeId)
        {
            var lst = ContentService.GetContentItems(contentTypeId);
           // ViewBag.ContentType = ContentTypeRepository.PopulateContentTypesToDropDownList(contentTypeId.ToString());
            return PartialView("../Sys/Taxonomy/_List", lst);
        }

        // GET: /Admin/ContentItem/Edit/5        
        [HttpGet]
         [SessionExpiration]
         public ActionResult _Create(ContentItemEntry entry)
         {
             return PartialView("../Sys/Taxonomy/_Edit", null);
         }

         // GET: /Admin/ContentItem/Edit/5        
         [HttpGet]
         [SessionExpiration]
         public ActionResult _Edit(int contenItemId)
         {
             var item = ContentService.GetContentItemDetails(contenItemId);
             return PartialView("../Sys/Taxonomy/_Edit", item);
        }

        //#region autocomplete DropDownList ============================================================================================
        ///// <summary>
        ///// Dùng cho viec binding du lieu cho dropdownlist autocomplete
        ///// </summary>
        ///// <param name="searchTerm"></param>
        ///// <param name="pageSize"></param>
        ///// <param name="pageNum"></param>
        ///// <returns></returns>
        //[HttpGet]
        //public ActionResult DropDownListByPage(string searchTerm, int pageSize, int pageNum)
        //{
        //    List<AutoCompleteModel> sources = ContentItemRepository.GetContentDropDownListByPage(searchTerm, pageSize, pageNum).ToList();
        //    int iTotal = sources.Count;

        //    //Translate the list into a format the select2 dropdown expects
        //    Select2PagedResultViewModel pagedList = BaseRepository.ConvertAutoListToSelect2Format(sources, iTotal);

        //    //Return the data as a jsonp result
        //    return new JsonpResult
        //    {
        //        Data = pagedList,
        //        JsonRequestBehavior = JsonRequestBehavior.AllowGet
        //    };
        //}

        //public ActionResult DropDownListByModule(string searchTerm, int pageSize, int pageNum)
        //{
        //    List<AutoCompleteModel> sources = ContentItemRepository.GetContentDropDownListByModule(searchTerm, pageSize, pageNum).ToList();
        //    int iTotal = sources.Count;

        //    //Translate the list into a format the select2 dropdown expects
        //    Select2PagedResultViewModel pagedList = BaseRepository.ConvertAutoListToSelect2Format(sources, iTotal);

        //    //Return the data as a jsonp result
        //    return new JsonpResult
        //    {
        //        Data = pagedList,
        //        JsonRequestBehavior = JsonRequestBehavior.AllowGet
        //    };
        //}

        //#endregion ===================================================================================================================



        // POST: /Admin/Contract/Create
        [AcceptVerbs(HttpVerbs.Post)]
         public ActionResult Create(ContentItemEntry entry)
        {
            bool flag = false;
            string message = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    ContentService.InsertContentItem(entry);
                    flag = true;
                }
                else
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    message = errors.Aggregate(message, (current, modelError) => current + (modelError.ErrorMessage + "<br/>"));
                }
            }
            catch (ValidationError ex)
            {
                message = ValidationExtension.ConvertValidateErrorToString(ex);
            }
            return Json(JsonUtils.SerializeResult(flag, message), JsonRequestBehavior.AllowGet);
        }
        //
        // POST: 

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(ContentItemEditEntry entry)
        {
            bool flag = false;
            string message = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    var contentItemEntry = new ContentItemEntry
                    {
                        ContentTypeId = entry.ContentTypeId,
                        ItemKey = entry.ItemKey,
                        ItemContent = entry.ItemContent,
                        IsActive = entry.IsActive
                    };

                    ContentService.UpdateContentItem(entry.ContentItemId, contentItemEntry);
                    flag = true;
                }
                else
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    message = errors.Aggregate(message, (current, modelError) => current + (modelError.ErrorMessage + "<br/>"));
                }
            }
            catch (ValidationError ex)
            {
                message = ValidationExtension.ConvertValidateErrorToString(ex);
            }
            return Json(JsonUtils.SerializeResult(flag, message), JsonRequestBehavior.AllowGet);
        }


        //
        // POST: /Admin/Contract/Delete/5

        [HttpPost]
        public ActionResult Delete(int id)
        {
            bool flag = false;
            string message = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    ContentService.DeleteContentItem(id);
                    flag = true;
                }
                else
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    message = errors.Aggregate(message, (current, modelError) => current + (modelError.ErrorMessage + "<br/>"));
                }
            }
            catch (ValidationError ex)
            {
                message = ValidationExtension.ConvertValidateErrorToString(ex);
            }
            return Json(JsonUtils.SerializeResult(flag, message), JsonRequestBehavior.AllowGet);
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
                    ContentService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}
