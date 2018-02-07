using System.Collections.Generic;
using Eagle.Services.Dtos.SystemManagement.Settings;
using PayPal.Api;

namespace Eagle.Services.Business
{
    public interface IPayPalService : IBaseService
    {
        PaypalSettingDetail GetPaypalSetting();
        Dictionary<string, string> GetConfig();
        APIContext GetApiContext();
    }
}
