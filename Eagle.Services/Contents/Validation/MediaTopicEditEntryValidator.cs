using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Contents.Media;
using Eagle.Services.Validations;

namespace Eagle.Services.Contents.Validation
{
    public class MediaTopicEditEntryValidator : SpecificationBase<MediaTopicEditEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }
        public MediaTopicEditEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel, ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(MediaTopicEditEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundMediaTopicEditEntry, "MediaTopicEditEntry"));
                return false;
            }

            ISpecification<MediaTopicEditEntry> isValidName = new HasValidName();
            ISpecification<MediaTopicEditEntry> isValidParentId = new HasValidParentId();
            return isValidName.And(isValidParentId).IsSatisfyBy(data, violations);
        }

        private class HasValidName : SpecificationBase<MediaTopicEditEntry>
        {
            protected override bool IsSatisfyBy(MediaTopicEditEntry data, IList<RuleViolation> violations = null)
            {
                var isValid = (!string.IsNullOrEmpty(data.TopicName) || data.TopicName.Length > 300);
                if (!isValid && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidTopicName, "TopicName"));
                    return false;
                }
               return true;
            }
        }

        public class HasValidParentId : SpecificationBase<MediaTopicEditEntry>
        {
            protected override bool IsSatisfyBy(MediaTopicEditEntry data, IList<RuleViolation> violations = null)
            {
                if (data.ParentId != null && data.ParentId > 0 && data.ParentId <= int.MaxValue)
                {
                    var entityByParentId = UnitOfWork.MediaTopicRepository.FindById(data.ParentId);
                    if (entityByParentId == null && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.NullReferenceForParentId, "ParentId", null,
                        ErrorMessage.Messages[ErrorCode.NullReferenceForParentId]));
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
