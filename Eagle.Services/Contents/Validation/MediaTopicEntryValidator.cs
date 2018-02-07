using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Contents.Media;
using Eagle.Services.Validations;

namespace Eagle.Services.Contents.Validation
{
    public class MediaTopicEntryValidator : SpecificationBase<MediaTopicEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }
        public MediaTopicEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel, ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(MediaTopicEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundMediaTopicEntry, "MediaTopicEntry"));
                return false;
            }

            ISpecification<MediaTopicEntry> isValidName = new HasValidName();
            ISpecification<MediaTopicEntry> isValidParentId = new HasValidParentId();
            return isValidName.And(isValidParentId).IsSatisfyBy(data, violations);
        }

        private class HasValidName : SpecificationBase<MediaTopicEntry>
        {
            protected override bool IsSatisfyBy(MediaTopicEntry data, IList<RuleViolation> violations = null)
            {
                var isValid = (!string.IsNullOrEmpty(data.TopicName) || data.TopicName.Length > 300);
                if (!isValid && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidTopicName, "TopicName"));
                    return false;
                }
                else
                {
                    bool isDuplicate = UnitOfWork.MediaTopicRepository.HasDataExisted(data.TopicName, data.ParentId);
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

        public class HasValidParentId : SpecificationBase<MediaTopicEntry>
        {
            protected override bool IsSatisfyBy(MediaTopicEntry data, IList<RuleViolation> violations = null)
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
