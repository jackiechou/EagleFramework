using System.Collections.Generic;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Resources;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.SystemManagement.Identity;
using Eagle.Services.Validations;

namespace Eagle.Services.SystemManagement.Validation
{
    public class RoleGroupEntryValidator : SpecificationBase<GroupEntry>
    {
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        public RoleGroupEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
        }

        protected override bool IsSatisfyBy(GroupEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundRoleGroupEntry, "RoleGroupEntry"));
                return false;
            }

            ISpecification<GroupEntry> isValidRoleGroupName = new IsValidRoleGroupName();
            return isValidRoleGroupName.IsSatisfyBy(data, violations);
        }

        internal class IsValidRoleGroupName : SpecificationBase<GroupEntry>
        {
            protected override bool IsSatisfyBy(GroupEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.GroupName) || data.GroupName.Length > 250)
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidGroupName, "GroupName", data.GroupName, LanguageResource.InvalidGroupName));
                    }
                    return false;
                }
                return true;
            }
        }
    }
}
