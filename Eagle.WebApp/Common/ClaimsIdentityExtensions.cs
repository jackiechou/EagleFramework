using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Eagle.Common.Extensions;
using Eagle.Core.Configuration;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Validations;
using System.IdentityModel.Services;

namespace Eagle.WebApp.Common
{
    public static class ClaimsIdentityExtensions
    {
        public static List<Claim> GetCurrentClaims(this ClaimsPrincipal claimPrincipal)
        {
            if (claimPrincipal == null)
            {
                FederatedAuthentication.SessionAuthenticationModule.SignOut();
                return null;
            }
             
            var claims = claimPrincipal.Claims.ToList();
            if (!claims.Any())
            {
                FederatedAuthentication.SessionAuthenticationModule.SignOut();
                return null;
            }

            return claims;
        }

        public static string GetUserName(this ClaimsPrincipal identity)
        {
            var claims = GetCurrentClaims(identity);
            
            var claim = claims.FirstOrDefault(c => c.Type == SettingKeys.UserName);
            if (claim == null)
            {
                var violations = new List<RuleViolation>
                {
                    new RuleViolation(ErrorCode.NotFoundUserName, "UserName", null,
                        ErrorMessage.Messages[ErrorCode.NotFoundUserName])
                };
                throw new ValidationError(violations);
            }
            return claim.Value ?? string.Empty;
        }
        public static Guid GetUserId(this ClaimsPrincipal identity)
        {
            var claims = GetCurrentClaims(identity);
            var claim = claims.FirstOrDefault(c => c.Type == SettingKeys.UserId);
            if (claim == null)
            {
                var violations = new List<RuleViolation>
                {
                    new RuleViolation(ErrorCode.NotFoundUserId, "UserId", null,
                        ErrorMessage.Messages[ErrorCode.NotFoundUserId])
                };
                throw new ValidationError(violations);
            }
            return claim.Value.ToGuid();
        }
        public static Guid GetRoleId(this ClaimsPrincipal identity)
        {
            var claims = GetCurrentClaims(identity);
            var claim = claims.FirstOrDefault(c => c.Type == SettingKeys.RoleId);
            if (claim == null)
            {
                var violations = new List<RuleViolation>
                {
                    new RuleViolation(ErrorCode.NotFoundRoleId, "RoleId", null,
                        ErrorMessage.Messages[ErrorCode.NotFoundRoleId])
                };
                throw new ValidationError(violations);
            }
            return claim.Value.ToGuid();
        }
        public static int GetVendorId(this ClaimsPrincipal identity)
        {
            var claims = GetCurrentClaims(identity);
            var claim = claims.FirstOrDefault(c => c.Type == SettingKeys.VendorId);
            if (claim == null)
            {
                var violations = new List<RuleViolation>
                {
                    new RuleViolation(ErrorCode.NotFoundVendorId, "VendorId", null,
                        ErrorMessage.Messages[ErrorCode.NotFoundVendorId])
                };
                throw new ValidationError(violations);
            }
            return claim.Value.ToInt();
        }
        public static Guid GetApplicationId(this ClaimsPrincipal identity)
        {
            var claims = GetCurrentClaims(identity);
            var claim = claims.FirstOrDefault(c => c.Type == SettingKeys.ApplicationId);
            if (claim == null)
            {
                var violations = new List<RuleViolation>
                {
                    new RuleViolation(ErrorCode.NotFoundApplicationId, "ApplicationId", null,
                        ErrorMessage.Messages[ErrorCode.NotFoundApplicationId])
                };
                throw new ValidationError(violations);
            }
            return claim.Value.ToGuid();
        }
        public static string GetLanguageCode(this ClaimsPrincipal identity)
        {
            var claims = GetCurrentClaims(identity);
            var claim = claims.FirstOrDefault(c => c.Type == SettingKeys.LanguageCode);
            if (claim == null)
            {
                var violations = new List<RuleViolation>
                {
                    new RuleViolation(ErrorCode.NotFoundLanguageCode, "LanguageCode", null,
                        ErrorMessage.Messages[ErrorCode.NotFoundLanguageCode])
                };
                throw new ValidationError(violations);
            }
            return claim.Value;
        }
    }
}
