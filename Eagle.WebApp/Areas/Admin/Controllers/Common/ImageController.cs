using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Eagle.WebApp.Areas.Admin.Controllers.Common
{
    public class ImageController : Controller
    {
       

       //[HttpPost]
       ////[ValidateAntiForgeryToken]
       //public JsonResult Create(ImageCreateViewModel model)
       //{
       //    string imageUrl = string.Empty;
       //    var validImageTypes = new string[]
       //     {
       //         "image/gif",
       //         "image/jpeg",
       //         "image/pjpeg",
       //         "image/png"
       //     };

       //    if (model.ImageUpload == null || model.ImageUpload.ContentLength == 0)
       //    {
       //        ModelState.AddModelError("ImageUpload", "This field is required");
       //    }
       //    else if (!validImageTypes.Contains(model.ImageUpload.ContentType))
       //    {
       //        ModelState.AddModelError("ImageUpload", "Please choose either a GIF, JPG or PNG image.");
       //    }

       //    if (ModelState.IsValid)
       //    {
       //        var image = new ImageModel
       //        {
       //            Title = model.Title,
       //            AltText = model.AltText,
       //            Caption = model.Caption
       //        };

       //        if (model.ImageUpload != null && model.ImageUpload.ContentLength > 0)
       //        {
       //            var uploadDir = "~/Uploads";
       //            var imagePath = Path.Combine(Server.MapPath(uploadDir), model.ImageUpload.FileName);
       //            imageUrl = Path.Combine(uploadDir, model.ImageUpload.FileName);
       //            model.ImageUpload.SaveAs(imagePath);
       //            image.ImageUrl = imageUrl;
       //        }               
       //    }

       //    return Json(imageUrl, JsonRequestBehavior.AllowGet);
       //}

      
    }
}
