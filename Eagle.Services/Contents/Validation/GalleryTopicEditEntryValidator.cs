using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Contents.Galleries;
using Eagle.Services.Validations;

namespace Eagle.Services.Contents.Validation
{
    public class GalleryTopicEditEntryValidator : SpecificationBase<GalleryTopicEditEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }
        public GalleryTopicEditEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel, ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(GalleryTopicEditEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundGalleryTopicEditEntry, "GalleryTopicEditEntry"));
                return false;
            }

            ISpecification<GalleryTopicEditEntry> isValidName = new HasValidName();
            ISpecification<GalleryTopicEditEntry> isValidParentId = new HasValidParentId();
            return isValidName.And(isValidParentId).IsSatisfyBy(data, violations);
        }

        private class HasValidName : SpecificationBase<GalleryTopicEditEntry>
        {
            protected override bool IsSatisfyBy(GalleryTopicEditEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.TopicName))
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.NullTopicName, "TopicName"));
                        return false;
                    }
                }
                else
                {
                    if (data.TopicName.Length > 300)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidTopicName, "TopicName"));
                            return false;
                        }
                    }
                }
                return true;
            }
        }

        public class HasValidParentId : SpecificationBase<GalleryTopicEditEntry>
        {
            protected override bool IsSatisfyBy(GalleryTopicEditEntry data, IList<RuleViolation> violations = null)
            {
                if (data.ParentId != null && data.ParentId >= int.MaxValue)
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidParentId, "ParentId"));
                        return false;
                    }
                }
                return true;
            }
        }

    }
}
