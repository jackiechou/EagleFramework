using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Web.Mvc;
using Eagle.Core.Configuration;
using Eagle.Services.Dtos.Common;
using Eagle.Services.SystemManagement;
using Newtonsoft.Json;

namespace Eagle.WebApp.Controllers
{
    public class CaptchaController : BasicController
    {
        private IApplicationService ApplicationService { get; set; }
        public CaptchaController(IApplicationService applicationService)
        {
            ApplicationService = applicationService;
        }
        
        [HttpGet]
        public ActionResult CreateCaptcha()
        {
            var item = ApplicationService.GetRecaptchaSetting(GlobalSettings.DefaultApplicationId, GoogleReCaptcha.SiteKey);
            string reCaptchaSiteKey = item.Setting.KeyValue;

            var captchaRequest = new CaptchaRequest {PublicKey = reCaptchaSiteKey };
            return PartialView("_Captcha", captchaRequest);
        }

        [HttpPost]
        public ActionResult ValidateCaptcha(CaptchaRequest request)
        {
            var responses = new List<string>();
            var response = Request["g-recaptcha-response"];
            var item = ApplicationService.GetRecaptchaSetting(GlobalSettings.DefaultApplicationId, GoogleReCaptcha.SecretKey);
            string secret = item.Setting.KeyValue;

            string captchaUrl = $"http://www.google.com/recaptcha/api/siteverify?secret={secret}&response={request.Response}";
            var client = new WebClient();
            var reply = client.DownloadString(captchaUrl);
            var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);

            //when response is false check for the error message
            if (!captchaResponse.Success)
            {
                if (captchaResponse.ErrorCodes.Count <= 0) return Json(new { Success = false });

                var error = captchaResponse.ErrorCodes[0].ToLower();
                switch (error)
                {
                    case ("missing-input-secret"):
                        responses.Add("The secret parameter is missing.");
                        break;
                    case ("invalid-input-secret"):
                        responses.Add("The secret parameter is invalid or malformed.");
                        break;

                    case ("missing-input-response"):
                        responses.Add("The response parameter is missing.");
                        break;
                    case ("invalid-input-response"):
                        responses.Add("The response parameter is invalid or malformed.");
                        break;

                    default:
                        responses.Add("Error occured. Please try again");
                        break;
                }
            }
            else
            {
                responses.Add("Valid");
            }
            var responseLines = string.Join("\n", responses.ToArray());
            var success = responses[0].Equals("true");

            return Json(new { Success = success, ErrorCodes = responseLines });
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
                    ApplicationService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}