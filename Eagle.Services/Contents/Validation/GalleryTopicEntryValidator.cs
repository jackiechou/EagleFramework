using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Contents.Galleries;
using Eagle.Services.Validations;

namespace Eagle.Services.Contents.Validation
{
    public class GalleryTopicEntryValidator : SpecificationBase<GalleryTopicEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }
        public GalleryTopicEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel, ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(GalleryTopicEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundGalleryTopicEntry, "GalleryTopicEntry"));
                return false;
            }

            ISpecification<GalleryTopicEntry> hasValidName = new HasValidName();
            ISpecification<GalleryTopicEntry> hasisValidCode = new HasValidCode();
            ISpecification<GalleryTopicEntry> hasValidParentId = new HasValidParentId();
            return hasValidName.And(hasisValidCode).And(hasValidParentId).IsSatisfyBy(data, violations);
        }

        private class HasValidName : SpecificationBase<GalleryTopicEntry>
        {
            protected override bool IsSatisfyBy(GalleryTopicEntry data, IList<RuleViolation> violations = null)
            {
                var isValid = (!string.IsNullOrEmpty(data.TopicName) || data.TopicName.Length > 300);
                if (!isValid && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidTopicName, "TopicName"));
                    return false;
                }
                else
                {
                    bool isDuplicate = UnitOfWork.GalleryTopicRepository.HasTopicNameExisted(data.TopicName, data.ParentId);
                    if (isDuplicate)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicateTopicName, "TopicName", data.TopicName));
                            return false;
                        }
                    }
                    return true;
                }
            }
        }

        private class HasValidCode : SpecificationBase<GalleryTopicEntry>
        {
            protected override bool IsSatisfyBy(GalleryTopicEntry data, IList<RuleViolation> violations = null)
            {
                var isValid = (!string.IsNullOrEmpty(data.TopicCode) || data.TopicCode.Length > 300);
                if (!isValid && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidTopicCode, "TopicCode"));
                    return false;
                }
                else
                {
                    bool isDuplicate = UnitOfWork.GalleryTopicRepository.HasTopicCodeExisted(data.TopicCode);
                    if (isDuplicate)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicateTopicCode, "TopicCode", data.TopicCode));
                            return false;
                        }
                    }
                    return true;
                }
            }
        }

        public class HasValidParentId : SpecificationBase<GalleryTopicEntry>
        {
            protected override bool IsSatisfyBy(GalleryTopicEntry data, IList<RuleViolation> violations = null)
            {
                if (data.ParentId != null && data.ParentId >= int.MaxValue)
                {
                    if (violations != null)
                        violations.Add(new RuleViolation(ErrorCode.InvalidParentId, "ParentId"));
                    return false;
                }
                return true;
            }
        }

    }
}
