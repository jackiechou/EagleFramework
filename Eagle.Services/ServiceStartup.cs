using System;
using System.Security.Claims;
using Eagle.Core.Cookie;
using Eagle.Services.EntityMapping;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Microsoft.Owin;

namespace Eagle.Services
{
    public class ServiceStartup
    {
        public void ConfigureCookieAuth(IAppBuilder app)
        {
            app.Use((owinContext, next) =>
            {
                //HttpContext.Current.User = principal; //this will work as well if you are hosting in IIS, 
                //but if you are using owin, might as well use the owin to set the principal
                owinContext.Authentication.User = ClaimsPrincipal.Current;
                return next.Invoke();
            });
            
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString(CookieSetting.LoginPath),  //sign page
                CookieName = CookieSetting.CookieName,
                CookieHttpOnly = CookieSetting.HttpOnly,
                ExpireTimeSpan = TimeSpan.FromMinutes(CookieSetting.Expires), // allows you to set how long the issued cookie is valid
                LogoutPath = new PathString(CookieSetting.LogoutPath), //sign out page
                ReturnUrlParameter = CookieSetting.ReturnUrlParameter,
                CookieSecure = CookieSecureOption.SameAsRequest, //Use CookieSecureOption.Always if you intend to serve cookie in SSL/TLS (Https)
                SlidingExpiration = true, //then the cookie would be re-issued on any request half way
                //Provider = new CookieAuthenticationProvider
                //{
                //    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                //    validateInterval: TimeSpan.FromMinutes(15),
                //    regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager)),
                //},
            });

            //// Use a cookie to temporarily store information about a 
            //// user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            //// Enables the application to temporarily store user information when 
            //// they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(
                DefaultAuthenticationTypes.TwoFactorCookie,
                TimeSpan.FromMinutes(180));

            //// Enables the application to remember the second login verification factor such 
            //// as phone or email. Once you check this option, your second step of 
            //// verification during the login process will be remembered on the device where 
            //// you logged in from. This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(
            DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            //Mapper must be initialized once per application domain/process
            MappingHelper.InitializeMapping();

            // Uncomment the following lines to enable logging 
            // in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");


            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");


            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");


            //app.UseGoogleAuthentication();
        }
        
        //public void ConfigureTokenAuthGeneration(IAppBuilder app)
        //{
        //    app.CreatePerOwinContext(ApplicationDbContext.Create);
        //    app.CreatePerOwinContext<ApplicationUserManagerToken>(ApplicationUserManagerToken.Create);
        //    app.CreatePerOwinContext<ApplicationRoleManagerToken>(ApplicationRoleManagerToken.Create);

        //    OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
        //    {
        //        AllowInsecureHttp = true,
        //        TokenEndpointPath = new PathString("/oauth/token"),
        //        AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(BaseConstants.ACCESSTOKEN_EXPIRE_TIMESPAN_MINUTES),
        //        Provider = new CustomOAuthProviderToken(),
        //        AccessTokenFormat = new CustomJwtFormat(Uri.UriSchemeHttp)
        //    };

        //    app.UseOAuthAuthorizationServer(OAuthServerOptions);
        //}

        //public void ConfigureOAuthTokenConsumption(IAppBuilder app)
        //{
        //    var issuer = Uri.UriSchemeHttp;
        //    string audienceId = BaseConstants.CONFIGURATION_AUDIENCE_ID;
        //    byte[] audienceSecret = TextEncodings.Base64Url.Decode(BaseConstants.CONFIGURATION_AUDIENCE_SECRET);

        //    app.UseJwtBearerAuthentication(
        //        new JwtBearerAuthenticationOptions
        //        {
        //            AuthenticationMode = AuthenticationMode.Active,
        //            AllowedAudiences = new[] { audienceId },
        //            IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
        //            {
        //                new SymmetricKeyIssuerSecurityTokenProvider(issuer, audienceSecret)
        //            }
        //        });
        //}
    }
}
