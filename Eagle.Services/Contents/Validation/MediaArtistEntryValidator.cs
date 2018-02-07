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
    public class MediaArtistEntryValidator : SpecificationBase<MediaArtistEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }

        public MediaArtistEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel,
            ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(MediaArtistEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundMediaArtistEntry, "MediaArtistEntry"));
                return false;
            }

            ISpecification<MediaArtistEntry> isValidName = new IsValidName();
            ISpecification<MediaArtistEntry> isValidDescription = new IsValidDescription();
            ISpecification<MediaArtistEntry> isValidStatus = new IsValidStatus();

            return
                isValidName.And(isValidDescription)
                    .And(isValidStatus)
                    .IsSatisfyBy(data, violations);
        }

        private class IsValidName : SpecificationBase<MediaArtistEntry>
        {
            protected override bool IsSatisfyBy(MediaArtistEntry data, IList<RuleViolation> violations = null)
            {
                var result = !string.IsNullOrEmpty(data.ArtistName) && data.ArtistName.Length > 250;
                if (result && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidName, "TypeName"));
                    return false;
                }
                return true;
            }
        }


        private class IsValidDescription : SpecificationBase<MediaArtistEntry>
        {
            protected override bool IsSatisfyBy(MediaArtistEntry data, IList<RuleViolation> violations = null)
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

        private class IsValidStatus : SpecificationBase<MediaArtistEntry>
        {
            protected override bool IsSatisfyBy(MediaArtistEntry data, IList<RuleViolation> violations = null)
            {
                if (!Enum.IsDefined(typeof(MediaArtistStatus), data.Status))
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "MediaArtistStatus"));
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
