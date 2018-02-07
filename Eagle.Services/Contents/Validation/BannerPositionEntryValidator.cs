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
    public class BannerPositionEntryValidator : SpecificationBase<BannerPositionEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        public BannerPositionEntryValidator(ClaimsPrincipal currentClaimsIdentity, PermissionLevel permissionLevel)
        {
            CurrentClaimsIdentity = currentClaimsIdentity;
            PermissionLevel = permissionLevel;
        }

        protected override bool IsSatisfyBy(BannerPositionEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundBannerPositionEntry, "BannerPositionEntry"));
                return false;
            }

            ISpecification<BannerPositionEntry> isValidName = new IsValidName();
            ISpecification<BannerPositionEntry> isValidStatus = new IsValidStatus();
            ISpecification<BannerPositionEntry> isValidDescription = new IsValidDescription();
            
            var result = isValidName.And(isValidDescription).And(isValidStatus).IsSatisfyBy(data, violations);
            return result;
        }

        private class IsValidName : SpecificationBase<BannerPositionEntry>
        {
            protected override bool IsSatisfyBy(BannerPositionEntry data, IList<RuleViolation> violations = null)
            {
                var result = !string.IsNullOrEmpty(data.PositionName) && data.PositionName.Length > 250;
                if (result && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidName, "PositionName"));
                    return false;
                }
                return true;
            }
        }

        private class IsValidDescription : SpecificationBase<BannerPositionEntry>
        {
            protected override bool IsSatisfyBy(BannerPositionEntry data, IList<RuleViolation> violations = null)
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

        private class IsValidStatus : SpecificationBase<BannerPositionEntry>
        {
            protected override bool IsSatisfyBy(BannerPositionEntry data, IList<RuleViolation> violations = null)
            {
                if (!Enum.IsDefined(typeof(BannerPositionStatus), data.Status))
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
