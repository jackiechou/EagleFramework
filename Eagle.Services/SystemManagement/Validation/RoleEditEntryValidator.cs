using System.Collections.Generic;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.SystemManagement.Identity;
using Eagle.Services.Validations;

namespace Eagle.Services.SystemManagement.Validation
{
    public class RoleEditEntryValidator : SpecificationBase<RoleEditEntry>
    {
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        public RoleEditEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
        }

        protected override bool IsSatisfyBy(RoleEditEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundRoleEditEntry, "RoleEditEntry"));
                return false;
            }

            ISpecification<RoleEditEntry> hasValidRoleName = new HasValidRoleName();
            return hasValidRoleName.IsSatisfyBy(data, violations);
        }

        internal class HasValidRoleName : SpecificationBase<RoleEditEntry>
        {
            protected override bool IsSatisfyBy(RoleEditEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.RoleName))
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidRoleName));
                    }
                    return false;
                }

                if (data.RoleName.Length > 250)
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidRoleName));
                    }
                    return false;
                }
                return true;
            }
        }
    }
}
