using System;
using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Core.Permission;
using Eagle.Core.Settings;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Contents.Banners;
using Eagle.Services.Validations;

namespace Eagle.Services.Contents.Validation
{
    public class BannerScopeEntryValidator : SpecificationBase<BannerScopeEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        public BannerScopeEntryValidator(ClaimsPrincipal currentClaimsIdentity, PermissionLevel permissionLevel)
        {
            CurrentClaimsIdentity = currentClaimsIdentity;
            PermissionLevel = permissionLevel;
        }

        protected override bool IsSatisfyBy(BannerScopeEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullReferenceBannerScopeEntry, "BannerScopeEntry"));
                return false;
            }

            ISpecification<BannerScopeEntry> isValidName = new IsValidName();
            ISpecification<BannerScopeEntry> isValidStatus = new IsValidStatus();
            ISpecification<BannerScopeEntry> isValidDescription = new IsValidDescription();

            var result = isValidName.And(isValidDescription).And(isValidStatus).IsSatisfyBy(data, violations);
            return result;
        }

        private class IsValidName : SpecificationBase<BannerScopeEntry>
        {
            protected override bool IsSatisfyBy(BannerScopeEntry data, IList<RuleViolation> violations = null)
            {
                var result = !string.IsNullOrEmpty(data.ScopeName) && data.ScopeName.Length > 250;
                if (result && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidName, "ScopeName"));
                    return false;
                }
                return true;
            }
        }

        private class IsValidDescription : SpecificationBase<BannerScopeEntry>
        {
            protected override bool IsSatisfyBy(BannerScopeEntry data, IList<RuleViolation> violations = null)
            {
                var result = !string.IsNullOrEmpty(data.Description) && data.Description.Length > 500;
                if (result && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidDescription, "Description"));
                    return false;
                }
                return true;
            }
        }

        private class IsValidStatus : SpecificationBase<BannerScopeEntry>
        {
            protected override bool IsSatisfyBy(BannerScopeEntry data, IList<RuleViolation> violations = null)
            {
                if (!Enum.IsDefined(typeof(BannerScopeStatus), data.Status))
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status"));
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
