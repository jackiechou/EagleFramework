using System.Collections.Generic;
using Eagle.Core.Configuration;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.SystemManagement.Settings;
using Eagle.Services.SystemManagement;
using Eagle.Services.Validations;
using PayPal.Api;

namespace Eagle.Services.Business
{
    public class PayPalService : BaseService, IPayPalService
    {
        public IApplicationService ApplicationService { get; set; }
        public PaypalSettingDetail PaypalSetting { get; set; }

        public PayPalService(IUnitOfWork unitOfWork, IApplicationService applicationService) : base(unitOfWork)
        {
            ApplicationService = applicationService;
            PaypalSetting = GetPaypalSetting();
        }

        public PaypalSettingDetail GetPaypalSetting()
        {
            var violations = new List<RuleViolation>();
            var applicationId = GlobalSettings.DefaultApplicationId;
            var paypalSetting = ApplicationService.GetActivePaypalSetting(applicationId);
            if (paypalSetting == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundPaypalSetting, "Paypal", null, ErrorMessage.Messages[ErrorCode.NotFoundPaypalSetting]));
                throw new ValidationError(violations);
            }
            return paypalSetting;
        }

        // Create the configuration map that contains mode and other optional configuration details.
        public Dictionary<string, string> GetConfig()
        {
            var endpoint = PaypalSetting.Mode.ToLower() == "sandbox" ? "https://api.sandbox.paypal.com" : "https://api.paypal.com";

            var config = new Dictionary<string, string>
            {
                {"model", PaypalSetting.Mode},
                {"clientId", PaypalSetting.ClientId},
                {"clientSecret", PaypalSetting.ClientSecret},
                {"connectionTimeout", PaypalSetting.ConnectionTimeout},
                {"requestRetries", PaypalSetting.RequestRetries},
                {"endpoint", endpoint}
            };

            return config;
        }

        // Create accessToken
        public string GetAccessToken()
        {
            var clientId = PaypalSetting.ClientId;
            var clientSecret = PaypalSetting.ClientSecret;
            var config = GetConfig();

            //Basically, apiContext object has a accesstoken which is sent by the paypal
            //to authenticate the payment to facilitator account.
            //An access token could be an alphanumeric string
            var credential = new OAuthTokenCredential(clientId, clientSecret, config);
            string accessToken = credential.GetAccessToken();
            
            return accessToken;
        }

        // Returns APIContext object
        public APIContext GetApiContext()
        {
            var violations = new List<RuleViolation>();
            string accessToken = GetAccessToken();
            if (string.IsNullOrEmpty(accessToken))
            {
                violations.Add(new RuleViolation(ErrorCode.UnableToAccessToken, "AccessToken", accessToken, ErrorMessage.Messages[ErrorCode.UnableToAccessToken]));
                throw new ValidationError(violations);
            }

            var config = GetConfig();

            var apiContext = new APIContext(accessToken) { Config = config };
            return apiContext;
        }
    }
}
