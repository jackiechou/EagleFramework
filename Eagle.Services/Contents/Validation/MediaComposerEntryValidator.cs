using System;
using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Core.Permission;
using Eagle.Core.Settings;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Contents.Media;
using Eagle.Services.Validations;

namespace Eagle.Services.Contents.Validation
{
    public class MediaComposerEntryValidator : SpecificationBase<MediaComposerEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }

        public MediaComposerEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel,
            ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(MediaComposerEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundMediaComposerEntry, "MediaComposerEntry"));
                return false;
            }

            ISpecification<MediaComposerEntry> isValidName = new IsValidName();
            ISpecification<MediaComposerEntry> isValidDescription = new IsValidDescription();
            ISpecification<MediaComposerEntry> isValidStatus = new IsValidStatus();

            return
                isValidName.And(isValidDescription)
                    .And(isValidStatus)
                    .IsSatisfyBy(data, violations);
        }

        private class IsValidName : SpecificationBase<MediaComposerEntry>
        {
            protected override bool IsSatisfyBy(MediaComposerEntry data, IList<RuleViolation> violations = null)
            {
                var result = !string.IsNullOrEmpty(data.ComposerName) && data.ComposerName.Length > 250;
                if (result && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidName, "TypeName"));
                    return false;
                }
                return true;
            }
        }

       
        private class IsValidDescription : SpecificationBase<MediaComposerEntry>
        {
            protected override bool IsSatisfyBy(MediaComposerEntry data, IList<RuleViolation> violations = null)
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

        private class IsValidStatus : SpecificationBase<MediaComposerEntry>
        {
            protected override bool IsSatisfyBy(MediaComposerEntry data, IList<RuleViolation> violations = null)
            {
                if (!Enum.IsDefined(typeof(MediaComposerStatus), data.Status))
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "MediaComposerStatus"));
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
