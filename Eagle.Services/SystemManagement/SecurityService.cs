using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using Eagle.Common.Security.Cryptography;
using Eagle.Common.Utilities;
using Eagle.Core.Configuration;
using Eagle.Core.Settings;
using Eagle.Entities.SystemManagement;
using Eagle.Repositories;
using Eagle.Resources;
using Eagle.Services.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.SystemManagement;
using Eagle.Services.Dtos.SystemManagement.Identity;
using Eagle.Services.EntityMapping.Common;
using Eagle.Services.Validations;
using System.IdentityModel.Services;
using System.Security.Principal;
using System.Web.Security;
using Eagle.Common.Extensions;
using Eagle.Core.Cookie;

namespace Eagle.Services.SystemManagement
{
    public class SecurityService : BaseService, ISecurityService
    {
        private IApplicationService ApplicationService { get; set; }
        private IContactService ContactService { get; set; }
        private ICurrencyService CurrencyService { get; set; }
        private IPermissionService PermissionService { get; set; }
        private IRoleService RoleService { get; set; }
        private IUserService UserService { get; set; }

        public SecurityService(IUnitOfWork unitOfWork, IApplicationService applicationService,
            ICurrencyService currencyService, IContactService contactService, IRoleService roleService, 
            IPermissionService permissionService, IUserService userService) : base(unitOfWork)
        {
            ApplicationService = applicationService;
            ContactService = contactService;
            CurrencyService = currencyService;
            RoleService = roleService;
            PermissionService = permissionService;
            UserService = userService;
        }

        #region CLAIMS

        public ClaimsIdentity FindByUserId(Guid userId)
        {
            var claims = new ClaimsIdentity();
            var listOfClaim = UnitOfWork.UserClaimRepository.GetUserClaimsByUserId(userId);
            if (listOfClaim == null) return claims;
            foreach (var userClaim in listOfClaim)
            {
                claims.AddClaim(new Claim(userClaim.ClaimType, userClaim.ClaimValue));
            }
            return claims;
        }
        public void DeleteAllClaimsOfUser(Guid userId)
        {
            var claims = UnitOfWork.UserClaimRepository.GetUserClaimsByUserId(userId).ToList();
            if (claims.Any())
            {
                foreach (var claim in claims)
                {
                    UnitOfWork.UserClaimRepository.Delete(claim);
                }
            }
        }
        #endregion

        public UserInfoDetail GetUserProfile(Guid userId)
        {
            var dataViolations = new List<RuleViolation>();
            var userItem = UnitOfWork.UserRepository.GetDetails(userId);
            if (userItem == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NotFoundUser, "UserId", LanguageResource.NotFoundUser + " : " + string.Join(", ", userId)));
                throw new ValidationError(dataViolations);
            }

            if (userItem.Profile == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NotFoundProfile, "Profile", LanguageResource.NotFoundProfile + " : " + string.Join(", ", userId)));
                throw new ValidationError(dataViolations);
            }

            var contactDetail = ContactService.GetContactInfoDetails(userItem.Profile.ContactId);
            var addressres = ContactService.GetUserAddresses(userItem.User.UserId);

            var application = userItem.Application.ToDto<ApplicationEntity, ApplicationDetail>();
            var user = userItem.User.ToDto<User, UserDetail>();
            var profile = new UserProfileInfoDetail
            {
                ProfileId = userItem.Profile.ProfileId,
                UserId = userItem.Profile.UserId,
                ContactId = userItem.Profile.ContactId,
                Contact = contactDetail,
                Addresses = addressres
            };

            return new UserInfoDetail
            {
                Application = application,
                User = user,
                Profile = profile,
            };
        }
        public string GetPassword(string userName)
        {
            var entity = UnitOfWork.UserRepository.FindByUserName(userName);
            return entity.Password;
        }
        public void ChangePassword(ChangePasswordModel entry)
        {
            var violations = new List<RuleViolation>();
            if (entry.NewPassword != entry.ConfirmedPassword)
            {
                violations.Add(new RuleViolation(ErrorCode.PasswordMismatch, "ConfirmedPassword",
                    $"{entry.NewPassword} vs {entry.ConfirmedPassword}", ErrorMessage.Messages[ErrorCode.PasswordMismatch]));
                throw new ValidationError(violations);
            }
           
            var entity = UnitOfWork.UserRepository.FindByUserAndPassword(entry.OldPassword, entry.UserName);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidPassword, "OldPassword",
                    entry.OldPassword, ErrorMessage.Messages[ErrorCode.InvalidPassword]));
                throw new ValidationError(violations);
            }

            entity.Password = Md5Crypto.GetMd5Hash(entry.NewPassword);
            entity.PasswordSalt = entry.NewPassword;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.UserRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public bool ResetPassword(string email, out string newPassword)
        {
            bool result = false;
            newPassword = string.Empty;
            if (!string.IsNullOrEmpty(email))
            {
                var item = UnitOfWork.UserRepository.FindByEmail(email);
                if (item != null)
                {
                    newPassword = RandomPassword.Generate(8);

                    var entity = new User
                    {
                        PasswordSalt = newPassword,
                        Password = Md5Crypto.GetMd5Hash(newPassword),
                        LastModifiedDate = DateTime.UtcNow,
                        LastUpdatedIp = NetworkUtils.GetIP4Address()
                    };

                    UnitOfWork.UserRepository.Update(entity);
                    //Hashtable templateVariables = new Hashtable {{"FullName", fullName}, {"NewPassword", newPassword}};
                    //int mailTemplateId = 33;
                    //if (UnitOfWork.MailTemplateRespository.SendGMailByTemplate(templateVariables, mailTemplateId, email, null, null))
                    result = true;

                }
            }
            return result;
        }
        public bool CheckLogin(string userName, string password)
        {
            var dataViolations = new List<RuleViolation>();
            var violations = new List<RuleViolation>();
            if (string.IsNullOrEmpty(userName))
            {
                violations.Add(new RuleViolation(ErrorCode.NullUserName, "UserName", null, ErrorMessage.Messages[ErrorCode.NullUserName]));
                throw new ValidationError(violations);
            }
            if (string.IsNullOrEmpty(password))
            {
                violations.Add(new RuleViolation(ErrorCode.NullPassword, "Password", null, ErrorMessage.Messages[ErrorCode.NullPassword]));
                throw new ValidationError(violations);
            }

           // if (CurrentClaimsIdentity != null) return CurrentClaimsIdentity;
            var userItem = UnitOfWork.UserRepository.FindByUserAndPassword(userName, password);
            if (userItem == null)
            {
                dataViolations.Add(new RuleViolation(ErrorCode.NotFoundUser, "UserId",null,
                    LanguageResource.NotFoundUser + " : " + string.Join(", ", userName)));
                throw new ValidationError(dataViolations);
            }
            else
            {
                if (userItem.IsApproved == null)
                {
                    dataViolations.Add(new RuleViolation(ErrorCode.UnApproved, "IsApproved", null, LanguageResource.UnApproved));
                    throw new ValidationError(dataViolations);
                }

                if (userItem.IsApproved != null && userItem.IsApproved == false)
                {
                    dataViolations.Add(new RuleViolation(ErrorCode.UnApproved, "IsApproved", userItem.IsApproved, LanguageResource.UnApproved));
                    throw new ValidationError(dataViolations);
                }

                if (userItem.IsLockedOut != null && userItem.IsLockedOut == true)
                {
                    dataViolations.Add(new RuleViolation(ErrorCode.IsLockedOut, "IsLockedOut", userItem.IsLockedOut, LanguageResource.IsLockedOut));
                    throw new ValidationError(dataViolations);
                }

                return true;
            }
        }

        public UserInfoDetail GetUserDetail(string userName)
        {
            var userItem = UnitOfWork.UserRepository.GetUserDetails(userName);
            var addresses = ContactService.GetUserAddresses(userItem.Profile.UserId).ToList();
            var contact = ContactService.GetContactInfoDetails(userItem.Profile.ContactId);
            var roles = RoleService.GetUserRolesByUserName(userItem.User.ApplicationId, userName,true).ToList();
        
            string firstName = contact.FirstName;
            string lastName = contact.LastName;
            string languageCode = (!string.IsNullOrEmpty(userItem.Application.DefaultLanguage)) ? userItem.Application.DefaultLanguage : GlobalSettings.DefaultLanguageCode;
            
            switch (languageCode)
            {
                case LanguageType.English:
                    contact.FullName = firstName + " " + lastName;
                    break;
                case LanguageType.Vietnamese:
                    contact.FullName = lastName + " " + firstName;
                    break;
                default:
                    contact.FullName = firstName + " " + lastName;
                    break;
            }

            var accountInfo = new UserInfoDetail
            {
                Application = userItem.Application.ToDto<ApplicationEntity, ApplicationDetail>(),
                User = new UserDetail
                {
                    ApplicationId = userItem.User.ApplicationId,
                    SeqNo = userItem.User.SeqNo,
                    UserId = userItem.User.UserId,
                    UserName = userItem.User.UserName,
                    LoweredUserName = userItem.User.LoweredUserName,
                    Password = userItem.User.Password,
                    PasswordSalt = userItem.User.PasswordSalt,
                    PasswordQuestion = userItem.User.PasswordQuestion,
                    PasswordAnswer = userItem.User.PasswordAnswer,
                    IsSuperUser = userItem.User.IsSuperUser,
                    IsApproved = userItem.User.IsApproved,
                    IsLockedOut = userItem.User.IsLockedOut,
                    UpdatePassword = userItem.User.UpdatePassword,
                    EmailConfirmed = userItem.User.EmailConfirmed,
                    LastPasswordChangedDate = userItem.User.LastPasswordChangedDate,
                    FailedPasswordAttemptCount = userItem.User.FailedPasswordAttemptCount,
                    FailedPasswordAttemptTime = userItem.User.FailedPasswordAttemptTime,
                    FailedPasswordAnswerAttemptCount = userItem.User.FailedPasswordAnswerAttemptCount,
                    FailedPasswordAnswerAttemptTime = userItem.User.FailedPasswordAnswerAttemptTime,
                    StartDate = userItem.User.StartDate,
                    ExpiredDate = userItem.User.ExpiredDate,
                    LastLoginDate = userItem.User.LastLoginDate,
                    LastLockoutDate = userItem.User.LastLockoutDate,
                    LastActivityDate = userItem.User.LastActivityDate,
                    Ip = userItem.User.Ip,
                    LastUpdatedIp = userItem.User.LastUpdatedIp,
                    CreatedDate = userItem.User.CreatedDate,
                    LastModifiedDate = userItem.User.LastModifiedDate,
                    CreatedByUserId = userItem.User.CreatedByUserId,
                    LastModifiedByUserId = userItem.User.LastModifiedByUserId
                },
                Profile = new UserProfileInfoDetail
                {
                    ProfileId = userItem.Profile.ProfileId,
                    ContactId = userItem.Profile.ContactId,
                    UserId = userItem.Profile.UserId,
                    Contact = contact,
                    Addresses = addresses
                },
                Roles = roles
            };

            return accountInfo;
        }


        /// <summary>
        /// https://itq.nl/mixing-forms-authentication-with-claims-based-authorisation-in-asp-net/
        /// </summary>
        /// <param name="userData"></param>
        /// <param name="isPersistent"></param>
        /// <returns></returns>
        public ClaimsPrincipal CreateUserClaims(UserInfoDetail userData, string redirectUri, bool isPersistent)
        {
            //Set up application setting with language and date time format
            string vendorId = GlobalSettings.DefaultVendorId.ToString();
            var applicationId = userData.User.ApplicationId;
            ApplicationService.SetUpAppConfig(applicationId);
            
            //Create generic claims
            var currency = CurrencyService.GetSelectedCurrency();
            var defaultAddress = userData.Profile.Addresses.FirstOrDefault();
            string userId = Convert.ToString(userData.User.UserId);
            string userName = userData.User.UserName;
            string displayName = userData.Profile.Contact.DisplayName;
            string fullName = userData.Profile.Contact.FullName;
            string languageCode = userData.Application.DefaultLanguage ?? GlobalSettings.DefaultLanguageCode;
            string email = userData.Profile.Contact.Email;

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.PrimarySid, userId),
                    new Claim(ClaimTypes.Sid, userId),
                    new Claim(ClaimTypes.NameIdentifier, userName),
                    new Claim(ClaimTypes.Name, userName),
                    new Claim(ClaimTypes.GivenName, userData.Profile.Contact.FirstName),
                    new Claim(ClaimTypes.Surname, userData.Profile.Contact.LastName),
                    new Claim(ClaimTypes.Hash, userData.User.Password),
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Country, defaultAddress != null? defaultAddress.Location: string.Empty),
                    //new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", userName),
                    //new Claim("urn:Custom:UserType", "AnonymousUser"),

                    //Custom claims
                    new Claim(SettingKeys.ApplicationId, applicationId.ToString()),
                    new Claim(SettingKeys.LanguageCode, languageCode),
                    new Claim(SettingKeys.UserId, userId),
                    new Claim(SettingKeys.UserName, userName),
                    new Claim(SettingKeys.Password, userData.User.PasswordSalt),
                    new Claim(SettingKeys.FullName, fullName),
                    new Claim(SettingKeys.DisplayName, displayName),
                    new Claim(SettingKeys.CurrencyCode, currency.CurrencyCode),
                    //new Claim(SettingKeys.UserProfile, userData.Profile.ToJson()), //Custom entity with user info
                    new Claim(SettingKeys.VendorId, vendorId, ClaimValueTypes.Integer),

                    //Custom claims vs ClaimsIdentity
                    //new Claim(ClaimsIdentity.DefaultNameClaimType, email),
                };

            var roleIds = new List<string>();
            var roles = userData.Roles.ToList();
            if (roles.Any())
            {
                foreach (var role in roles)
                {
                    string roleId = role.RoleId.ToString();

                    if (roles.Count == 1 || role.IsDefaultRole!=null)
                    {
                        var defaultRole = role.Role.RoleName;
                        claims.Add(new Claim(SettingKeys.RoleId, roleId));
                        claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, defaultRole));
                    }
                    else
                    {
                        claims.Add(new Claim(ClaimTypes.Role, roleId));
                    }
                    roleIds.Add(roleId);
                }
            }

            //Save Identity to cookie
            var authenticationTicket = SaveIdentityToCookie(userData, isPersistent);
            var formsIdentity = new FormsIdentity(authenticationTicket);
            formsIdentity.AddClaims(claims);
            formsIdentity.Label = fullName;
            //Attach the new principal object to the current HttpContext object
            var claimsPrincipal = new GenericPrincipal(formsIdentity, roleIds.ToArray());

           // var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            // var claimsIdentity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie, userName, defaultRoleType)
            // {
            //     Label = fullName
            // };
            //// claimsIdentity.Name = userName;
            // claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, userName));
            // claimsIdentity.AddClaim(new Claim("role", defaultRoleType));

            // get context of the authentication manager
            //var authenticationProperties = new AuthenticationProperties
            //{
            //    IsPersistent = isPersistent,
            //    RedirectUri = redirectUri,
            //    IssuedUtc = DateTime.UtcNow,
            //    ExpiresUtc = DateTime.UtcNow.AddMinutes(CookieSetting.Expires),
            //    AllowRefresh=true
            //};
            //var identity = new[]{ claimsIdentity };

            // var ticket = new AuthenticationTicket(claimsIdentity, authenticationProperties);

            //var tokenResponse = new JObject(
            //    new JProperty("userName", userName),
            //    new JProperty("access_token", accessToken),
            //    new JProperty("token_type", "bearer"),
            //    new JProperty("expires_in", tokenExpiration.TotalSeconds.ToString()),
            //    new JProperty(".issued", ticket.Properties.IssuedUtc.ToString()),
            //    new JProperty(".expires", ticket.Properties.ExpiresUtc.ToString())
            //);

            //var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
            //authenticationManager.SignIn(authenticationProperties, identity);
            //authenticationManager.AuthenticationResponseGrant = new AuthenticationResponseGrant(claimsPrincipal, authenticationProperties);

            //var customPrincipal = new UserPrincipal(formsIdentity, user.RolesList.ToArray());

            //var authProperties = new AuthenticationProperties
            //{
            //    ExpiresUtc = userData.Expires,
            //    IsPersistent = userData.RememberMe,
            //    IssuedUtc = userData.Issued,
            //    RedirectUri = redirectUrl
            //};
            //var authTicket = new AuthenticationTicket(claimsIdentity, authProperties);

            ////ClaimsAuthenticationManager authManager = FederatedAuthentication.FederationConfiguration.IdentityConfiguration.ClaimsAuthenticationManager;
            ////authManager.Authenticate(GlobalSettings.ResourceName, claimsPrincipal);

            //EstablishSession
            var sam = FederatedAuthentication.SessionAuthenticationModule;
            if (sam != null)
            {
                //var token = new SessionSecurityToken(claimsPrincipal, TimeSpan.FromHours(8));
                //sam.AuthenticateSessionSecurityToken(token, true);
                var token = sam.CreateSessionSecurityToken(claimsPrincipal, GlobalSettings.ApplicationName, DateTime.UtcNow, DateTime.UtcNow.AddHours(8), false);
                sam.WriteSessionTokenToCookie(token);
                sam.CookieHandler.RequireSsl = false;
                sam.IsReferenceMode = true;
            }

            // Make sure the Principal's are in sync
            HttpContext.Current.User = claimsPrincipal;
            Thread.CurrentPrincipal = claimsPrincipal;
            SetIdentity(claimsPrincipal);
            return claimsPrincipal;
        }

        private FormsAuthenticationTicket SaveIdentityToCookie(UserInfoDetail userData, bool isPersistent)
        {
            var expiration = DateTime.Now.AddMinutes(CookieSetting.Expires);

            var newTicket = new FormsAuthenticationTicket(
                1, //A dummy ticket version
                userData.User.UserName, //User name for whom the ticket is issued
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
                encryptedTicket)
            {
                HttpOnly = CookieSetting.HttpOnly,
                Expires = newTicket.IsPersistent ? newTicket.Expiration : DateTime.Now.AddMinutes(180)
            }; // Hashed ticket
            //cookie.Value = encryptedTicket;

            // Set the cookie's expiration time to the tickets expiration time

            // Add the cookie to the list for outgoing response
            HttpContext.Current.Response.Cookies.Add(cookie);
            return newTicket;
        }

        public bool CheckUserPermissioned(Guid userId, int capabilityId)
        {
            var roleIds = UnitOfWork.UserRoleRepository.GetRoles(userId, true).Select(x => x.RoleId).ToList();
            if (!roleIds.Any()) return false;

            var permissions = new List<ModulePermission>();
            foreach (var modulePermissions in roleIds.Select(roleId => UnitOfWork.ModulePermissionRepository.GetModulePermissionsByRoleId(roleId).ToList()).Where(modulePermissions => modulePermissions.Any()))
            {
                permissions.AddRange(modulePermissions);
            }

            return permissions.FirstOrDefault(permission => permission.CapabilityId == capabilityId) != null;
        }

        /// <summary>
        /// Check in claims pricipals whether there is capabilityName and capabilityId in claim list
        /// </summary>
        /// <param name="capabilityName">ModuleCapabilitySetting.CAPABILITYID_NEWS_CREATE</param>
        /// <returns></returns>
        public bool HasPermission(string capabilityName)
        {
            var permission = CurrentClaimsIdentity.FindFirst(capabilityName);
            if (permission != null)
            {
                return true;
            }
            return false;
        }

        ///// <summary>
        /////  Check in claims pricipals whether there is capabilityName and capabilityId in claim list
        ///// </summary>
        ///// <param name="capabilityName">ModuleCapabilitySetting.CAPABILITYID_NEWS_CREATE => set up name for module capability return string</param>
        ///// <param name="permissionLevel"></param>
        ///// <returns></returns>
        //public bool HasPermission(string capabilityName, PermissionLevel permissionLevel)
        //{
        //    var permission = CurrentClaimsIdentity.FindFirst(capabilityName);
        //    if (permission != null && permission.Value.ToPermission() == permissionLevel)
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        public List<PageDetail> GetPagesForUser(Guid userId, bool? allowAccess=null)
        {
            var pages = new List<PageDetail>();
            var roleIds = UnitOfWork.UserRoleRepository.GetRoles(userId, allowAccess).Select(x => x.RoleId).ToList();
            if (roleIds.Any())
            {
                foreach (var pagelst in roleIds.Select(roleId => UnitOfWork.PagePermissionRepository.GetPagePermissionsByRoleId(roleId, allowAccess)
                    .Select(x => x.Page)))
                {
                    pages.AddRange(pagelst.ToDtos<Page, PageDetail>());
                }
            }
            return pages;
        }

        public List<ModuleDetail> GetModulesForUser(Guid userId, bool? allowAccess = null)
        {
            var lst = new List<ModuleDetail>();
            var pageIds = GetPagesForUser(userId, allowAccess).Select(x=>x.PageId).ToList();
            if (!pageIds.Any()) return lst;
            foreach (var modules in pageIds.Select(pageId => UnitOfWork.PageModuleRepository.GetModules(pageId, allowAccess)))
            {
                lst.AddRange(modules.ToDtos<Module, ModuleDetail>());
            }
            return lst;
        }

        public List<ModuleCapabilityInfoDetail> GetModuleCapabilitiesForUser(Guid userId, bool? allowAccess = null)
        {
            var lst = new List<ModuleCapabilityInfoDetail>();
            var modules = GetModulesForUser(userId, allowAccess).Select(x=>x.ModuleId).ToList();
            if (!modules.Any()) return lst;
            foreach (var moduleId in modules)
            {
                var capabilities = UnitOfWork.ModuleCapabilityRepository.GetModuleCapabilities(moduleId,
                    (allowAccess != null && allowAccess.Value == true)
                        ? ModuleCapabilityStatus.Active
                        : ModuleCapabilityStatus.InActive);
                lst.AddRange(capabilities.Select(capability => new ModuleCapabilityInfoDetail
                {
                    CapabilityId = capability.CapabilityId,
                    CapabilityCode = capability.CapabilityCode,
                    CapabilityName = capability.CapabilityName,
                    Description = capability.Description,
                    DisplayOrder = capability.DisplayOrder,
                    IsActive = capability.IsActive,
                    ModuleId = capability.ModuleId,
                    Module = capability.Module.ToDto<Module, ModuleDetail>()
                }));
            }
            return lst;
        }

        public List<UserRoleInfoDetail> GetRolesForUser(Guid userId, bool? status = null)
        {
            var lst = UnitOfWork.UserRoleRepository.GetRoles(userId, status).ToList();
            var userRoles = new List<UserRoleInfoDetail>();
            if (lst.Any())
            {
                userRoles.AddRange(from item in lst
                                   select new UserRoleInfoDetail
                                   {
                                       UserRoleId = item.UserRoleId,
                                       UserId = item.UserId,
                                       RoleId = item.RoleId,
                                       EffectiveDate = item.EffectiveDate,
                                       ExpiryDate = item.ExpiryDate,
                                       IsTrialUsed = item.IsTrialUsed,
                                       IsDefaultRole = item.IsDefaultRole ?? false,
                                       User = item.User.ToDto<User, UserDetail>(),
                                       Role = item.Role.ToDto<Role, RoleDetail>()
                                   });
            }
            return userRoles;
        }

        public IEnumerable<ModulePermissionDetail> GetModulePermissionCapabilityAccesses(Guid userId)
        {
            var roleIds = UnitOfWork.UserRoleRepository.GetRoles(userId, true).Select(x=>x.RoleId).ToList();
            if (!roleIds.Any()) return null;

            var permissions = new List<ModulePermission>();
            foreach (var modulePermissions in roleIds.Select(roleId => UnitOfWork.ModulePermissionRepository.GetModulePermissionsByRoleId(roleId).ToList()).Where(modulePermissions => modulePermissions.Any()))
            {
                permissions.AddRange(modulePermissions);
            }

            return permissions.Select(permission => permission.ToDto<ModulePermission, ModulePermissionDetail>());
        }
        public IEnumerable<ModulePermissionDetail> GetModulePermissionCapabilityAccessesByRole(Guid roleId)
        {
            var permissions = UnitOfWork.ModulePermissionRepository.GetModulePermissionsByRoleId(roleId);
            return permissions.ToDtos<ModulePermission, ModulePermissionDetail>();
        }
        public IEnumerable<ModulePermissionDetail> GetModulePermissionCapabilityAccessesByModule(int moduleId)
        {
            var permissions = UnitOfWork.ModulePermissionRepository.GetModulePermissions(moduleId);
            return permissions.ToDtos<ModulePermission, ModulePermissionDetail>();
        }
        //public List<Claim> SetClaims(Guid userId, int? permissionSetId, int? roleId)
        //{
        //    var claims = new List<Claim>
        //                    {
        //                        new Claim(SettingKeys.UserId, userId.ToString(), "http://www.ww3.org/2001/XMLSchema#string"),
        //                    };

        //    var permissionClaims = SetPermissions(permissionSetId, roleId, networkId);
        //    claims.AddRange(permissionClaims);
        //    return claims;
        //}

        //public List<Claim> SetPermissions(int? permissionSetId, int? roleId)
        //{
        //    List<Claim> claims = new List<Claim>();
        //    var permissions = SecurityRepository.GetRolePermissionCapabilityAccess(roleId);
        //    foreach (var permissionCapabilityAccess in permissions)
        //    {
        //        var capability = SecurityRepository.GetPermissionCapabilitiesById(permissionCapabilityAccess.CapabilityId);
        //        Claim claim = new Claim(capability.Name, permissionCapabilityAccess.Permissions.ToString());
        //        claims.Add(claim);
        //        if (capability.Name == "News")
        //        {
        //            if (permissionCapabilityAccess.Permissions >= capability.AllowedPermissions)
        //            {
        //                claim = new Claim(SherpaClaims.CanDeleteNews, "1");
        //                claims.Add(claim);
        //            }
        //            else if (permissionCapabilityAccess.Permissions >= capability.AllowedPermissions - 1)
        //            {
        //                claim = new Claim(SherpaClaims.CanEditNews, "1");
        //                claims.Add(claim);
        //            }
        //        }
        //    }

        //    permissions = SecurityRepository.GetUserPermissionCapabilityAccess(permissionSetId);
        //    foreach (var permissionCapabilityAccess in permissions)
        //    {
        //        var capability = SecurityRepository.GetPermissionCapabilitiesById(permissionCapabilityAccess.CapabilityId);
        //        Claim claim = new Claim(capability.Name, permissionCapabilityAccess.Permissions.ToString());
        //        claims.Remove(claims.Find(c => c.Type == claim.Type));
        //        claims.Add(claim);
        //        if (capability.Name == "News")
        //        {
        //            if (permissionCapabilityAccess.Permissions >= capability.AllowedPermissions)
        //            {
        //                claim = new Claim(SherpaClaims.CanDeleteNews, "1");
        //                claims.Add(claim);
        //            }
        //            else if (permissionCapabilityAccess.Permissions >= capability.AllowedPermissions - 1)
        //            {
        //                claim = new Claim(SherpaClaims.CanEditNews, "1");
        //                claims.Add(claim);
        //            }
        //        }
        //    }

        //    permissions = SecurityRepository.GetNetworkPermissionCapabilityAccess(networkId);
        //    foreach (var permissionCapabilityAccess in permissions)
        //    {

        //        if (permissionCapabilityAccess.FunctionalitySet != null && !permissionCapabilityAccess.FunctionalitySet.Contains("l"))
        //        {
        //            if (claims.Any(c => c.Type == "Attach Link"))
        //                claims.Remove(claims.FirstOrDefault(c => c.Type == "Attach Link"));
        //        }

        //        if (permissionCapabilityAccess.FunctionalitySet != null && !permissionCapabilityAccess.FunctionalitySet.Contains("f"))
        //        {
        //            if (claims.Any(c => c.Type == "Attach File"))
        //                claims.Remove(claims.FirstOrDefault(c => c.Type == "Attach File"));
        //        }


        //        if (permissionCapabilityAccess.FunctionalitySet != null && !permissionCapabilityAccess.FunctionalitySet.Contains("p"))
        //        {
        //            if (claims.Any(c => c.Type == "Attach Photo"))
        //                claims.Remove(claims.FirstOrDefault(c => c.Type == "Attach Photo"));
        //        }

        //        if (permissionCapabilityAccess.CapabilityId == 24)
        //        {
        //            var capability = SecurityRepository.GetPermissionCapabilitiesById(permissionCapabilityAccess.CapabilityId);
        //            Claim claim = new Claim(capability.Name, permissionCapabilityAccess.Permissions.ToString());
        //            claims.Add(claim);
        //        }
        //    }

        //    return claims;
        //}

        #region Page Permission

        public IEnumerable<PagePermissionDetail> GetPagePermissionsByRoleId(Guid roleId)
        {
            var lst = UnitOfWork.PagePermissionRepository.GetPagePermissionsByRoleId(roleId);
            return lst.ToDtos<PagePermission, PagePermissionDetail>();
        }
        public IEnumerable<PagePermissionDetail> GetPagePermissionsByPageId(int pageId)
        {
            var lst = UnitOfWork.PagePermissionRepository.GetPagePermissionsByPageId(pageId);
            return lst.ToDtos<PagePermission, PagePermissionDetail>();
        }
        #endregion

        #region Dipose

        private bool _disposed = false;
        protected override void Dispose(bool isDisposing)
        {
            if (!this._disposed)
            {
                if (isDisposing)
                {
                    ApplicationService = null;
                    ContactService = null;
                    CurrencyService = null;
                    PermissionService = null;
                    RoleService = null;
                    UserService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}