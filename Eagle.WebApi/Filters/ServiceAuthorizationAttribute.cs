using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Security;
using Eagle.Core.Common;
using Eagle.Core.Encryption;
using Eagle.WebApi.Controllers;
using Newtonsoft.Json;

namespace Eagle.WebApi.Filters
{
    /// <summary>
    /// Service Authorization Attribute
    /// </summary>
    public class ServiceAuthorizationAttribute : AuthorizationFilterAttribute
    {
        /// <summary>
        /// Service Authorization Attribute Constructor
        /// </summary>
        public ServiceAuthorizationAttribute() : this(null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceAuthorizationAttribute"/> class.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        public ServiceAuthorizationAttribute(IHttpContext httpContext)
        {
          //  HttpContext = httpContext ?? new ServiceHttpContext(System.Web.HttpContext.Current);
            ClaimsRestrictionKey = null;
        }

        /// <summary>
        /// Allows anonymous user
        /// </summary>
        public bool AllowAnonymous { get; set; }

        /// <summary>
        /// Configuration Key with a list if Restricted IPs and IP Ranges
        /// </summary>
        public string IpRangesRestrictionKey { get; set; }

        /// <summary>
        /// Configuration Key with a list of Claims to check
        /// </summary>
        public string ClaimsRestrictionKey { get; set; }

        /// <summary>
        /// Configuration Key with the Trusted Token
        /// </summary>
        public string TrustedTokenKey { get; set; }

        /// <summary>
        /// bool to check if cookie has valid token setup
        /// </summary>

        private bool IsCookieValid { get; set; }

        //        public override void OnAuthorization(HttpActionContext actionContext)
        //        {
        //#if DEBUG
        //            var bypass = ConfigurationManager.AppSettings["BypassAuthorization"];

        //            if (bypass == "True")
        //            {
        //                base.OnAuthorization(actionContext);
        //                return;
        //            }
        //#endif
        //            var currentController = actionContext.ControllerContext.Controller as ApiControllerBase;
        //            claimsPrincipal = (ClaimsPrincipal)actionContext.RequestContext.Principal;
        //            if (actionContext.RequestContext.Principal != null && actionContext.RequestContext.Principal.Identity != null && actionContext.RequestContext.Principal.Identity.IsAuthenticated)
        //            {
        //                Claim claim = new Claim(ClaimTypes.Authentication, NetworkProperties.PROPNAME_TRUE);
        //                ClaimsIdentity claimsIdentity = new ClaimsIdentity(NetworkProperties.PROPNAME_ISAUTHENTICATED);
        //                claimsIdentity.AddClaim(claim);
        //                claimsPrincipal.AddIdentity(claimsIdentity);
        //            }

        //            IsCookieValid = false;
        //            var authorizations = GetActionContextAuthorizationHandler(actionContext).ToList();
        //            var isAuthorized = true;

        //            foreach (var authorizeAction in authorizations)
        //            {
        //                isAuthorized &= authorizeAction(actionContext);
        //                if ((!isAuthorized && currentController != null && currentController.NumberOfAuthenticationsChecked > 0) || (!CheckBasicIdentityAuthorization(actionContext) && authorizations.Count() == 3))
        //                {
        //                    actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized User");
        //                }
        //            }

        //            if (currentController != null)
        //            {
        //                currentController.AuthenticationChecked();
        //                currentController.SetIdentity(claimsPrincipal);
        //            }

        //            base.OnAuthorization(actionContext);
        //        }

        //        private IEnumerable<Func<HttpActionContext, bool>> GetActionContextAuthorizationHandler(HttpActionContext actionContext)
        //        {
        //            HttpRequestMessage request = actionContext.Request;
        //            AuthenticationHeaderValue authorization = request.Headers.Authorization;

        //            var cookieName = ConfigurationManager.AppSettings["AuthCookieName"];
        //            if (request.Headers.Contains("Authorization"))
        //            {
        //                if (((string[])(request.Headers.GetValues("Authorization")))[0].Contains(cookieName))
        //                {
        //                    var authcookie = ((string[])(request.Headers.GetValues("Authorization")))[0];
        //                    request.Headers.Add("Cookie", authcookie);
        //                }
        //            }
        //            var cookieHeaders = request.Headers.GetCookies(cookieName);
        //            CookieState authCookie = null;
        //            if (cookieHeaders != null && cookieHeaders.Count > 0)
        //            {
        //                authCookie = cookieHeaders.First().Cookies.FirstOrDefault(c => c.Name == cookieName);
        //            }

        //            var scheme = authorization != null
        //                ? authorization.Scheme
        //                : authCookie != null ? Core.IdentityManagement.AuthenticationSchemes.Cookie : null;

        //            var authorizations = GetActionContextAuthorizationHandler(scheme);
        //            foreach (var authHandler in authorizations)
        //            {
        //                yield return authHandler;
        //            }
        //        }

        //        private IEnumerable<Func<HttpActionContext, bool>> GetActionContextAuthorizationHandler(string authenticationScheme)
        //        {
        //            switch (authenticationScheme)
        //            {
        //                case Core.IdentityManagement.AuthenticationSchemes.Cookie:
        //                    yield return CheckCookieAuthorization;            // Check the cookie
        //                    yield return LoadSecurityPrincipalFromCookie;    // Loads the member details
        //                    break;

        //                case Core.IdentityManagement.AuthenticationSchemes.Token:
        //                    yield return CheckTokenAuthorization;            // Check the token
        //                    yield return LoadSecurityPrincipalFromHeader;    // Loads the member details
        //                    break;
        //                case Core.IdentityManagement.AuthenticationSchemes.PublicKey:
        //                    yield return CheckPublicAuthorization;            // Check the Public Key                    
        //                    break;
        //                case Core.IdentityManagement.AuthenticationSchemes.NetworkPublicKey:
        //                    yield return CheckandLoadSecurityPrincipalForNetwork;    // Loads the member details
        //                    break;
        //            }

        //            yield return CheckBasicIdentityAuthorization;    // Check identity
        //            yield return CheckClaimTokenAuthorization;       // Check claims
        //            yield return CheckIpAuthorization;               // Check IP
        //        }

        //        private bool CheckBasicIdentityAuthorization(HttpActionContext actionContext)
        //        {
        //            var result = false;
        //            HttpRequestMessage request = actionContext.Request;
        //            AuthenticationHeaderValue authorization = request.Headers.Authorization;
        //            var currentController = actionContext.ControllerContext.Controller as ApiControllerBase;
        //            if (currentController != null && currentController.CurrentClaimsIdentity != null)
        //            {
        //                claimsPrincipal = currentController.CurrentClaimsIdentity;
        //            }

        //            var uri = actionContext.Request.RequestUri != null ? actionContext.Request.RequestUri.AbsolutePath : string.Empty;

        //            if (AllowAnonymous || (uri.Contains(NetworkProperties.PROPNAME_IDENTITY_AUTH) || uri.Contains(NetworkProperties.PROPNAME_PRESENTATION_THEMES) || uri.Contains(NetworkProperties.PROPNAME_FILESTORAGE)
        //                || uri.Contains(NetworkProperties.PROPNAME_PRESENTATION_BYID)))
        //            {
        //                return true;
        //            }

        //            if (IsCookieValid)
        //                return true;

        //            if (authorization == null)
        //            {
        //                return false;
        //            }

        //            if ((authorization.Scheme == Core.IdentityManagement.AuthenticationSchemes.Token || authorization.Scheme == Core.IdentityManagement.AuthenticationSchemes.NetworkPublicKey) && authorization != null)
        //            {
        //                result = true;
        //            }

        //            return result;
        //        }

        //        private bool CheckCookieAuthorization(HttpActionContext actionContext)
        //        {
        //            var result = false;

        //            var authCookieValue = GetAuthCookieValue(actionContext);

        //            if (authCookieValue != null)
        //            {
        //                int? memberId = null;
        //                var cookieTicket = FormsAuthentication.Decrypt(authCookieValue);
        //                var username = cookieTicket != null ? cookieTicket.Name : null;
        //                var authInfo = cookieTicket != null ? JsonConvert.DeserializeObject<AuthInfo>(cookieTicket.UserData) : null;
        //                var isTrusted = authInfo.TrustedToken == ConfigurationManager.AppSettings["TrustedTokenSherpa"];
        //                memberId = authInfo != null ? (int?)authInfo.MemberId : null;

        //                var InActive = InActiveMember(actionContext, memberId.Value);

        //                var host = authInfo != null ? authInfo.Host : "";
        //                //var isValidHost = host == request.Headers.Referrer.Host;
        //                var isValidHost = true;

        //                if (cookieTicket.Expiration >= System.DateTime.UtcNow)
        //                    result = !username.IsNullOrEmpty() && isTrusted && InActive && isValidHost;
        //            }

        //            IsCookieValid = result;
        //            return result;
        //        }

        //protected virtual bool InActiveMember(HttpActionContext actionContext, int memberId)
        //{
        //    bool InActive;
        //    using (var memberService = actionContext.Resolve<IMemberService>())
        //    {
        //        InActive = memberService.InActive(memberId);
        //    }
        //    return InActive;
        //}

        protected virtual string GetAuthCookieValue(HttpActionContext actionContext)
        {
            HttpRequestMessage request = actionContext.Request;

            var cookieName = ConfigurationManager.AppSettings["AuthCookieName"];
            string authCookieValue = null;
            if (request.Headers.Contains("Authorization"))
            {
                if (((string[])(request.Headers.GetValues("Authorization")))[0].Contains(cookieName))
                {
                    var authcookie = ((string[])(request.Headers.GetValues("Authorization")))[0];
                    request.Headers.Add("Cookie", authcookie);
                }
            }
            var cookieHeaders = request.Headers.GetCookies(cookieName);
            if (cookieHeaders != null && cookieHeaders.Count > 0)
            {
                var authCookie = cookieHeaders.First().Cookies.FirstOrDefault(c => c.Name == cookieName);
                if (authCookie != null)
                {
                    authCookieValue = authCookie.Value;
                }
            }

            return authCookieValue;
        }

        private bool CheckPublicAuthorization(HttpActionContext actionContext)
        {
            var decryptor = new DesEncryption();
            var privateKey = ConfigurationManager.AppSettings["WebApiPublicPrivateKey"];
            if (decryptor.Decrypt(actionContext.Request.Headers.Authorization.Parameter) == privateKey)
                return true;
            else
                return false;
        }

        private bool CheckTokenAuthorization(HttpActionContext actionContext)
        {
            HttpRequestMessage request = actionContext.Request;
            AuthenticationHeaderValue authorization = request.Headers.Authorization;
            var encoding = Encoding.GetEncoding("iso-8859-1");
            var byteCredentials = Convert.FromBase64String(authorization.Parameter);
            var credentials = encoding.GetString(byteCredentials);
            var parts = credentials.Split(':');
            if (parts.Length != 2)
            {
                return false;
            }

            var token = parts[1].Trim();

            var trustedToken = ConfigurationManager.AppSettings["TrustedTokenSherpa"];

            var result = trustedToken.Equals(token);

            return result;
        }

        //private bool CheckClaimTokenAuthorization(HttpActionContext actionContext)
        //{
        //    if (ClaimsRestrictionKey.IsNullOrEmpty()) return true; // Don't need to check
        //    var result = false;
        //    var controller = actionContext.ControllerContext.Controller as ApiControllerBase;
        //    var claimsPrincipal = controller?.CurrentClaimsIdentity;
        //    if (claimsPrincipal == null) return false; // Checked but not found

        //    var claimKeys = ClaimsRestrictionKey.Split(',');
        //    var capability = claimKeys[0];
        //    var permissionLevel = claimKeys[1];

        //    using (var security = actionContext.Resolve<ISecurityService>())
        //    {
        //        result = security.GetPermissions(claimsPrincipal, capability, permissionLevel);
        //    }
        //    return result;
        //}

        private bool CheckClaims(int claimValue, int level)
        {
            return claimValue.CompareTo(level) >= 0;
        }

        //private bool CheckDbClaims(HttpActionContext actionContext, IEnumerable<Claim> claims)
        //{
        //    bool result;
        //    var currentController = actionContext.ControllerContext.Controller as ApiControllerBase;
        //    if (currentController == null) return false;
        //    using (var claimService = actionContext.Resolve<IClaimService>())
        //    {
        //        var memberId = currentController.CurrentClaimsIdentity.GetMemberId();
        //        result = claimService.HasClaims(memberId, claims);
        //    }
        //    return result;
        //}

        //        private bool CheckIpAuthorization(HttpActionContext actionContext)
        //        {
        //            if (IpRangesRestrictionKey.IsNullOrEmpty()) return true;

        //            var ipRangeConfigKeys = IpRangesRestrictionKey.Split(',');

        //            var ipAddress = actionContext.Request.GetClientIpAddress();

        //            foreach (var ipRangeConfigKey in ipRangeConfigKeys)
        //            {
        //                if (ConfigurationManager.AppSettings.AllKeys.All(k => !k.ToLower().Equals(ipRangeConfigKey.ToLower())))
        //                {
        //                    return false;
        //                }

        //                var ipRanges = ConfigurationManager.AppSettings[ipRangeConfigKey].Split(',');

        //                foreach (var ipRange in ipRanges)
        //                {
        //                    var ips = ipRange.Split('-');

        //                    // Exact match
        //                    if (ips.Any(i => IPAddress.Parse(i).Equals(ipAddress)))
        //                    {
        //                        return true;
        //                    }

        //                    if (ips.Length != 2) continue; // Check next range

        //                    if (ipAddress.IsInRange(IPAddress.Parse(ips[0]), IPAddress.Parse(ips[1])))
        //                    {
        //                        return true;
        //                    }
        //                }
        //            }

        //            return false;
        //        }

        //        private bool LoadSecurityPrincipalFromCookie(HttpActionContext actionContext)
        //        {
        //            HttpRequestMessage request = actionContext.Request;
        //            AuthenticationHeaderValue authorization = request.Headers.Authorization;

        //            var cookieName = ConfigurationManager.AppSettings["AuthCookieName"];
        //            if (request.Headers.Contains("Authorization"))
        //            {
        //                if (((string[])(request.Headers.GetValues("Authorization")))[0].Contains(cookieName))
        //                {
        //                    var authcookie = ((string[])(request.Headers.GetValues("Authorization")))[0];
        //                    request.Headers.Add("Cookie", authcookie);
        //                }
        //            }

        //            int? memberId = null;
        //            string userId = null;

        //            var cookieHeaders = request.Headers.GetCookies(cookieName);
        //            CookieState authCookie = null;
        //            if (cookieHeaders != null && cookieHeaders.Count > 0)
        //            {
        //                authCookie = cookieHeaders.First().Cookies.FirstOrDefault(c => c.Name == cookieName);
        //                if (authCookie != null)
        //                {
        //                    var cookieTicket = FormsAuthentication.Decrypt(authCookie.Value);
        //                    userId = cookieTicket != null ? cookieTicket.Name : null;
        //                    var authInfo = cookieTicket != null ? JsonConvert.DeserializeObject<AuthInfo>(cookieTicket.UserData) : null;
        //                    memberId = authInfo != null ? (int?)authInfo.MemberId : null;
        //                }
        //            }

        //            if (authorization == null && authCookie == null)
        //            {
        //                return false;
        //            }

        //            if (memberId == null) return false;

        //            Member member;
        //            using (var memberService = actionContext.Resolve<IMemberService>())
        //            {
        //                member = memberService.GetById(memberId.Value);
        //            }

        //            var claims = new List<Claim>
        //                            {
        //                                new Claim(ClaimTypes.Name, userId),
        //                                new Claim(SherpaClaims.MemberId, member.MemberId.ToString(CultureInfo.InvariantCulture), "http://www.ww3.org/2001/XMLSchema#string", "LOCAL AUTHORITY", "LOCAL AUTHORITY"),
        //                                new Claim(ClaimTypes.AuthenticationMethod, AuthenticationMethods.Password + "Cookie")
        //                            };

        //            Claim claim;
        //            if (member.NetworkId != null)
        //            {
        //                claim = new Claim(SherpaClaims.NetworkId, member.NetworkId.Value.ToString(CultureInfo.InvariantCulture), "http://www.ww3.org/2001/XMLSchema#string", "LOCAL AUTHORITY", "LOCAL AUTHORITY");
        //                claims.Add(claim);
        //            }

        //            var authenticationScheme = authorization != null ? authorization.Scheme : Core.IdentityManagement.AuthenticationSchemes.Cookie;

        //            var principal = new ClaimsPrincipal(new[] { new ClaimsIdentity(claims, authenticationScheme) });
        //            principal.AddIdentities(claimsPrincipal.Identities);
        //            var currentController = actionContext.ControllerContext.Controller as ApiControllerBase;
        //            if (currentController != null)
        //            {
        //                currentController.SetIdentity(principal);
        //            }

        //            actionContext.RequestContext.Principal = principal;
        //            claim = new Claim(ClaimTypes.AuthenticationMethod, AuthenticationMethods.Password + "Cookie");

        //            return LoadSecurityPrincipal(memberId.Value, authenticationScheme, new[] { claim }, actionContext);
        //        }

        //        private bool LoadSecurityPrincipalFromHeader(HttpActionContext actionContext)
        //        {
        //            HttpRequestMessage request = actionContext.Request;
        //            AuthenticationHeaderValue authorization = request.Headers.Authorization;
        //            var authenticationScheme = authorization.Scheme;

        //            int memberId;

        //            switch (authenticationScheme)
        //            {
        //                case Core.IdentityManagement.AuthenticationSchemes.Basic:
        //                case Core.IdentityManagement.AuthenticationSchemes.Token:
        //                    var encoding = Encoding.GetEncoding("iso-8859-1");
        //                    var byteCredentials = Convert.FromBase64String(authorization.Parameter);
        //                    var credentials = encoding.GetString(byteCredentials);
        //                    var parts = credentials.Split(':');
        //                    var userId = parts[0].Trim();
        //                    memberId = int.Parse(userId);
        //                    break;

        //                default:
        //                    return true;
        //            }

        //            Claim claim = null;

        //            switch (authenticationScheme)
        //            {
        //                case Core.IdentityManagement.AuthenticationSchemes.Basic:
        //                    claim = new Claim(ClaimTypes.AuthenticationMethod, AuthenticationMethods.Password);
        //                    break;
        //                case Core.IdentityManagement.AuthenticationSchemes.Token:
        //                    claim = new Claim(ClaimTypes.AuthenticationMethod, AuthenticationMethods.Password + "Token");
        //                    break;
        //            }

        //            return LoadSecurityPrincipal(memberId, authenticationScheme, new[] { claim }, actionContext);
        //        }

        //        private bool LoadSecurityPrincipal(int memberId, string authenticationScheme, IEnumerable<Claim> includeClaims, HttpActionContext actionContext)
        //        {
        //            HttpRequestMessage request = actionContext.Request;
        //            Member member;
        //            using (var memberService = actionContext.Resolve<IMemberService>())
        //            {
        //                member = memberService.GetById(memberId);
        //            }

        //            if (member == null) return false;
        //            var claims = new List<Claim>
        //            {
        //                new Claim(ClaimTypes.Name, member.MemberLogin.LoginName),
        //                new Claim(SherpaClaims.MemberId, member.MemberId.ToString(CultureInfo.InvariantCulture), "http://www.ww3.org/2001/XMLSchema#string", "LOCAL AUTHORITY", "LOCAL AUTHORITY"),
        //            };

        //            if (member.NetworkId != null)
        //            {
        //                var claim = new Claim(SherpaClaims.NetworkId, member.NetworkId.Value.ToString(CultureInfo.InvariantCulture), "http://www.ww3.org/2001/XMLSchema#string", "LOCAL AUTHORITY", "LOCAL AUTHORITY");
        //                claims.Add(claim);
        //            }

        //            if (member.DefaultNetworkGroupId != null)
        //            {
        //                var claim = new Claim(SherpaClaims.MemberDefaultGroupId, member.DefaultNetworkGroupId.Value.ToString(CultureInfo.InvariantCulture), "http://www.ww3.org/2001/XMLSchema#string", "LOCAL AUTHORITY", "LOCAL AUTHORITY");
        //                claims.Add(claim);
        //            }

        //            Claim adminClaim;
        //            Claim networkAdmin;
        //            Claim systemAdmin;

        //            bool isSystemAccount;
        //            bool isNetworkAdmin;

        //            if (member.IsSystemAccount != null && member.IsSystemAccount == true)
        //            {
        //                isSystemAccount = (bool)member.IsSystemAccount;
        //                systemAdmin = new Claim(SherpaClaims.IsSystemAdmin, SherpaClaims.True);
        //            }
        //            else
        //            {
        //                isSystemAccount = false;
        //                systemAdmin = new Claim(SherpaClaims.IsSystemAdmin, SherpaClaims.False);
        //            }

        //            if (member.NetworkAdmin != null && member.NetworkAdmin == true)
        //            {
        //                isNetworkAdmin = (bool)member.NetworkAdmin;
        //                networkAdmin = new Claim(SherpaClaims.IsNetworkAdmin, SherpaClaims.True);
        //            }
        //            else
        //            {
        //                isNetworkAdmin = false;
        //                networkAdmin = new Claim(SherpaClaims.IsNetworkAdmin, SherpaClaims.False);
        //            }



        //            if (isSystemAccount || isNetworkAdmin)
        //            {
        //                adminClaim = new Claim(SherpaClaims.IsAdmin, SherpaClaims.True);
        //            }
        //            else
        //            {
        //                adminClaim = new Claim(SherpaClaims.IsAdmin, SherpaClaims.False);
        //            }
        //            claims.Add(adminClaim);
        //            claims.Add(networkAdmin);
        //            claims.Add(systemAdmin);

        //            claims.Add(member.Roaming.GetValueOrDefault(false)
        //                ? new Claim(SherpaClaims.IsRoamingMember, SherpaClaims.True)
        //                : new Claim(SherpaClaims.IsRoamingMember, SherpaClaims.False));


        //            if (request.Headers.Referrer != null)
        //            {
        //                var baseurl = request.Headers.Referrer.Scheme + "://" + request.Headers.Referrer.Authority;
        //                adminClaim = new Claim(NetworkProperties.PROPNAME_BASEURL, baseurl);
        //                claims.Add(adminClaim);
        //            }

        //            claims.AddRange(includeClaims);
        //            var permissionSetId = member.MemberProfile.PermissionSetId;
        //            var roleId = member.MemberProfile.RoleId;
        //            var networkId = member.NetworkId;

        //            if (permissionSetId != null)
        //                permissionSetId = (int)permissionSetId;
        //            if (roleId != null)
        //                roleId = (int)roleId;
        //            //Add User-Permission Claims
        //            List<Claim> permissions = new List<Claim>();
        //            using (var security = actionContext.Resolve<ISecurityService>())
        //            {
        //                permissions = security.SetPermissions(permissionSetId, roleId, networkId);
        //            }
        //            claims.AddRange(permissions);

        //            var principal = new ClaimsPrincipal(new[] { new ClaimsIdentity(claims, authenticationScheme) });
        //            principal.AddIdentities(claimsPrincipal.Identities);
        //            var currentController = actionContext.ControllerContext.Controller as ApiControllerBase;
        //            if (currentController != null)
        //            {
        //                currentController.SetIdentity(principal);
        //            }

        //            actionContext.RequestContext.Principal = principal;
        //            return true;
        //        }

        //        private bool CheckandLoadSecurityPrincipalForNetwork(HttpActionContext actionContext)
        //        {
        //            int networkId;
        //            AuthenticationHeaderValue authorization = actionContext.Request.Headers.Authorization;
        //            var publicKey = authorization.Parameter;
        //            var uri = actionContext.Request.RequestUri.AbsolutePath;
        //            using (var networkService = actionContext.Resolve<INetworkService>())
        //            {
        //                networkId = networkService.GetNetworkByPublicKey(publicKey);
        //            }
        //            if (networkId == null)
        //                return false;
        //            var claims = new List<Claim>
        //                            {
        //                                new Claim(authorization.Scheme, authorization.Parameter),
        //                                new Claim(SherpaClaims.NetworkId, networkId.ToString(CultureInfo.InvariantCulture), "http://www.ww3.org/2001/XMLSchema#string", "LOCAL AUTHORITY", "LOCAL AUTHORITY")
        //                            };
        //            var principal = new ClaimsPrincipal(new[] { new ClaimsIdentity(claims, authorization.Scheme) });
        //            var currentController = actionContext.ControllerContext.Controller as ApiControllerBase;
        //            if (currentController != null)
        //            {
        //                currentController.SetIdentity(principal);
        //            }
        //            if (uri.Contains(NetworkProperties.PROPNAME_INTEGRATIONUSERS))
        //                return true;
        //            else
        //                return false;
        //        }
        private IHttpContext HttpContext { get; set; }
        private ClaimsPrincipal claimsPrincipal { get; set; }
    }
}