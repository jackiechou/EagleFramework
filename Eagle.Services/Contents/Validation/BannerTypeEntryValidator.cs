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
    public class BannerTypeEntryValidator : SpecificationBase<BannerTypeEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        public BannerTypeEntryValidator(ClaimsPrincipal currentClaimsIdentity, PermissionLevel permissionLevel)
        {
            CurrentClaimsIdentity = currentClaimsIdentity;
            PermissionLevel = permissionLevel;
        }

        protected override bool IsSatisfyBy(BannerTypeEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundBannerTypeEntry, "BannerTypeEntry"));
                return false;
            }

            ISpecification<BannerTypeEntry> isValidName = new IsValidName();
            ISpecification<BannerTypeEntry> isValidDescription = new IsValidDescription();
            ISpecification<BannerTypeEntry> isValidStatus = new IsValidStatus();

            var result = isValidName.And(isValidDescription).And(isValidStatus).IsSatisfyBy(data, violations);
            return result;
        }

        private class IsValidName : SpecificationBase<BannerTypeEntry>
        {
            protected override bool IsSatisfyBy(BannerTypeEntry data, IList<RuleViolation> violations = null)
            {
                var result = !string.IsNullOrEmpty(data.TypeName) && data.TypeName.Length > 100;
                if (result && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidName, "TypeName"));
                    return false;
                }
                return true;
            }
        }

        private class IsValidDescription : SpecificationBase<BannerTypeEntry>
        {
            protected override bool IsSatisfyBy(BannerTypeEntry data, IList<RuleViolation> violations = null)
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

        private class IsValidStatus : SpecificationBase<BannerTypeEntry>
        {
            protected override bool IsSatisfyBy(BannerTypeEntry data, IList<RuleViolation> violations = null)
            {
                if (!Enum.IsDefined(typeof(BannerTypeStatus), data.Status))
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "DomainType"));
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
