using System;
using System.Collections.Generic;
using System.Linq;
using Eagle.Common.Settings;
using Eagle.Services.Dtos.SystemManagement.Identity;
using Eagle.Services.SystemManagement;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Eagle.WebApi.Models;
using Microsoft.Owin.Security;
using Eagle.WebApi.Filters;

namespace Eagle.WebApi
{
    public partial class Startup
    {
        //public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            //app.UseCookieAuthentication(new CookieAuthenticationOptions
            //{
            //    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
            //    //LoginPath = new PathString("/Admin/Login"),
            //    //LogoutPath = new PathString("/Admin/LogOut"),
            //    //ExpireTimeSpan = TimeSpan.FromMinutes(180),
            //    //ReturnUrlParameter = "/Admin/Index"
            //});
            //app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            //// Configure the db context and user manager to use a single instance per request
            //app.CreatePerOwinContext(ApplicationDbContext.Create);
            //app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            //app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
            //// Enable the application to use a cookie to store information for the signed in user
            //// and to use a cookie to temporarily store information about a user logging in with a third party login provider
            //app.UseCookieAuthentication(new CookieAuthenticationOptions());
            //app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            //// Configure the application for OAuth based flow
            //PublicClientId = "self";
            //OAuthOptions = new OAuthAuthorizationServerOptions
            //{
            //    TokenEndpointPath = new PathString("/Token"),
            //    Provider = new ApplicationOAuthProvider(PublicClientId),
            //    AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
            //    AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
            //    // In production mode set AllowInsecureHttp = false
            //    AllowInsecureHttp = true
            //};

            //// Enable the application to use bearer tokens to authenticate users
            //app.UseOAuthBearerTokens(OAuthOptions);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //    consumerKey: "",
            //    consumerSecret: "");

            //app.UseFacebookAuthentication(
            //    appId: "",
            //    appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }

        #region Security Start Up
        //public void ConfigureCookieAuth(IAppBuilder app)
        //{
        //    app.CreatePerOwinContext(ApplicationDbContext.Create);
        //    app.CreatePerOwinContext<ApplicationUserManagerCookie>(ApplicationUserManagerCookie.Create);
        //    app.CreatePerOwinContext<ApplicationSignInManagerCookie>(ApplicationSignInManagerCookie.Create);

        //    app.UseCookieAuthentication(new CookieAuthenticationOptions
        //    {
        //        AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
        //        LoginPath = new PathString(BaseConstants.COOKIE_PATH),
        //        Provider = new CookieAuthenticationProvider
        //        {
        //            OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManagerCookie, ApplicationUser>(
        //                validateInterval: TimeSpan.FromMinutes(30),
        //                regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
        //        }
        //    });
        //}

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
        #endregion
    }
}
