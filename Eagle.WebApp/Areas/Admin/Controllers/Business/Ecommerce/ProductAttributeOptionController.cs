using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Resources;
using Eagle.Services;
using Eagle.Services.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Validations;
using Eagle.WebApp.Areas.Admin.Controllers.Common;

namespace Eagle.WebApp.Areas.Admin.Controllers.Business.Ecommerce
{
    public class ProductAttributeOptionController : BaseController
    {
        private IProductService ProductService { get; set; }
        public ProductAttributeOptionController(IProductService productService) : base(new IBaseService[] { productService })
        {
            ProductService = productService;
        }

        //
        // DELETE: /Admin/ProductAttributeOption/Delete/5
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            try
            {
                ProductService.UpdateProductAttributeOptionStatus(id, ProductAttributeOptionStatus.InActive);
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
                    ProductService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}