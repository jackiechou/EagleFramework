using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using Eagle.Common.Settings;
using Eagle.Core.Configuration;
using Eagle.Services.Business;
using Eagle.Services.Dtos.SystemManagement.Identity;
using Eagle.Services.SystemManagement;

namespace Eagle.Services.Common
{
    public class AccountIdenttity : System.Security.Principal.GenericIdentity
    {
        private ICurrencyService CurrencyService { get; set; }
        public AccountIdenttity(ICurrencyService currencyService, UserInfoDetail authicatedUser) : base(authicatedUser.User.UserName)
        {
            CurrencyService = currencyService;

            var currency = CurrencyService.GetSelectedCurrency();
            var defaultAddress = authicatedUser.Profile.Addresses.FirstOrDefault();

            List<Claim> claims = new List<Claim>{
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", authicatedUser.User.UserName),
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", authicatedUser.User.UserId.ToString()),
                new Claim(ClaimTypes.PrimarySid, authicatedUser.User.UserId.ToString()),
                new Claim(ClaimTypes.Sid, Convert.ToString(authicatedUser.User.UserId)),
                new Claim(ClaimTypes.Hash, authicatedUser.User.Password),
                new Claim(ClaimTypes.NameIdentifier, Convert.ToString(authicatedUser.User.UserId)),
                new Claim(ClaimTypes.GivenName, authicatedUser.Profile.Contact.FirstName),
                new Claim(ClaimTypes.Surname, authicatedUser.Profile.Contact.LastName),
                new Claim(ClaimTypes.Name, authicatedUser.User.UserName),
                new Claim(ClaimTypes.Email, authicatedUser.Profile.Contact.Email),
                new Claim(ClaimTypes.Country, (defaultAddress != null)? defaultAddress.Location: string.Empty),

                //Custom claims
                new Claim(SettingKeys.ApplicationId, authicatedUser.Application.ApplicationId.ToString()),
                new Claim(SettingKeys.LanguageCode, authicatedUser.Application.DefaultLanguage ?? GlobalSettings.DefaultLanguageCode),
                new Claim(SettingKeys.UserId, authicatedUser.User.UserId.ToString()),
                new Claim(SettingKeys.UserName, authicatedUser.User.UserName),
                new Claim(SettingKeys.Password, authicatedUser.User.PasswordSalt),
                new Claim(SettingKeys.FullName, authicatedUser.Profile.Contact.FullName),
                new Claim(SettingKeys.DisplayName, authicatedUser.Profile.Contact.DisplayName),
                new Claim(SettingKeys.CurrencyCode, currency.CurrencyCode),
                new Claim(SettingKeys.VendorId, GlobalSettings.DefaultVendorId.ToString(), ClaimValueTypes.Integer),

                //Custom claims vs ClaimsIdentity
                new Claim(ClaimsIdentity.DefaultNameClaimType, authicatedUser.Profile.Contact.Email),
            };
            AddClaims(claims);
        }
    }

    public class AccountPrincipal : System.Security.Principal.IPrincipal
    {
        private ISecurityService SecurityService { get; set; }
        private UserInfoDetail User { get; }
        private HttpContextBase Context { get; }

        public AccountPrincipal(ISecurityService securityService, HttpContextBase context, UserInfoDetail user, System.Security.Principal.IIdentity identity)
        {
            User = user;
            Identity = identity;
            Context = context;
            SecurityService = securityService;
        }

        private IList<string> RoleCodes
        {
            get
            {
                return SecurityService.GetRolesForUser(User.User.UserId, true).Select(x => x.Role.RoleCode).ToList();
            }
        }

        private IList<string> PageCodes
        {
            get { return SecurityService.GetPagesForUser(User.User.UserId, true).Select(x => x.PageCode.ToString()).ToList(); }
        }

        private IList<string> ModuleCodes
        {
            get { return SecurityService.GetPagesForUser(User.User.UserId, true).Select(x => x.PageCode.ToString()).ToList(); }
        }

        private IList<string> CapabilityCodes
        {
            get { return SecurityService.GetModuleCapabilitiesForUser(User.User.UserId, true).Select(x => x.CapabilityName).ToList(); }
        }
        
        public System.Security.Principal.IIdentity Identity { get; }

        public bool IsInRole(string roleCode) { return RoleCodes.Contains(roleCode); }

        public bool IsInPages(string pageCode) {return PageCodes.Contains(pageCode);}

        public bool IsInModules(string moduleCode){return ModuleCodes.Contains(moduleCode);}

        public bool IsInCapabilities(string capabilityCode){return CapabilityCodes.Contains(capabilityCode);}

    }
}
