using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using Eagle.Common.Extensions;
using Eagle.Core.Cookie;
using Eagle.Repositories.SystemManagement.Security;
using Eagle.Services.Dtos.SystemManagement.Identity;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

namespace Eagle.Services.Common
{
    public static class HttpResponseBaseExtensions
    {
        public static FormsAuthenticationTicket SetAuthCookie<T>(this HttpResponseBase responseBase, string userName, bool isPersistent, T userData)
        {
            //In order to pickup the settings from config, we create a default cookie and use its values to create a  new one.
           //var cookie = FormsAuthentication.GetAuthCookie(FormsAuthentication.FormsCookieName, isPersistent);
           //var ticket = FormsAuthentication.Decrypt(cookie.Value);
           //if (ticket == null) return 0;

            var expiration = DateTime.Now.AddMinutes(CookieSetting.Expires);

            var newTicket = new FormsAuthenticationTicket(
                1, //A dummy ticket version
                userName, //User name for whom the ticket is issued
                DateTime.Now, //Current date and time
                expiration,  //Expiration date and time
                isPersistent, //Whether to persist cookie on client side. If true,
                //The authentication ticket will be issued for new sessions from the same client
                userData.ToJson(), // User-data, in this case the roles
                FormsAuthentication.FormsCookiePath); // Path cookie valid for

            // Encrypt the cookie using the machine key for secure transport
            var encryptedTicket = FormsAuthentication.Encrypt(newTicket);

            HttpCookie cookie = new HttpCookie(
                FormsAuthentication.FormsCookieName, // Name of auth cookie
                encryptedTicket); // Hashed ticket
            cookie.HttpOnly = CookieSetting.HttpOnly;
            //cookie.Value = encryptedTicket;

            // Set the cookie's expiration time to the tickets expiration time
            if (newTicket.IsPersistent)
            {
                cookie.Expires = newTicket.Expiration;
            }

            // Add the cookie to the list for outgoing response
            responseBase.Cookies.Add(cookie);
            return newTicket;
        }

        public static void VerifyAuthCookie(HttpContext context)
        {

            HttpCookie authCookie = context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie == null)
                return;

            FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            if (authTicket == null)
                return;

            if (authTicket.Expired)
                return;

            var userData = !string.IsNullOrEmpty(authTicket.UserData)
                ? JsonConvert.DeserializeObject<UserInfoDetail>(authTicket.UserData)
                : null;
            if (userData == null)
                return;

            // Create an Identity object
            UserIdentity id = new UserIdentity(userData.User.SeqNo,userData.User.UserName, userData.User.UserName, DefaultAuthenticationTypes.ApplicationCookie, false);

            // This principal will flow throughout the request.
            GenericPrincipal principal = new GenericPrincipal(id, new[] { "User" });
            context.User = principal;
        }

        /////Assign in sign in
            //public static int CreateFormsAuthentication<T>(HttpResponseBase responseBase, string userName, bool isPersistent, T userData)
            //{
            //    // Initialize Session Ticket
            //    var authTicket = new FormsAuthenticationTicket(
            //        1,                                      // version number
            //        userName,                               // name of the cookie
            //        DateTime.UtcNow,                           // issue date
            //        DateTime.UtcNow.AddDays(30),               // expiration
            //        isPersistent,                           // survives browser sessions = rememberMe
            //        userData.ToJson(),                      // custom data (serialized)
            //        FormsAuthentication.FormsCookiePath);

            //    // Encrypt the ticket.
            //    string encTicket = FormsAuthentication.Encrypt(authTicket);

            //    //// Create the cookie.
            //    HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            //    if (authTicket.IsPersistent)
            //        authCookie.Expires = authTicket.Expiration;

            //    responseBase.Cookies.Add(authCookie);
            //    if (encTicket != null) return encTicket.Length;
            //    return 0;
            //}

        }
}
