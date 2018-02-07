using System.Collections.Generic;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.SystemManagement.Identity;
using Eagle.Services.Validations;

namespace Eagle.Services.SystemManagement.Validation
{
    public class RoleEntryValidator : SpecificationBase<RoleEntry>
    {
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        public RoleEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
        }

        protected override bool IsSatisfyBy(RoleEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundRoleEntry, "RoleEntry", null, ErrorMessage.Messages[ErrorCode.NotFoundRoleEntry]));
                return false;
            }

            ISpecification<RoleEntry> hasValidRoleName = new HasValidRoleName();
            return hasValidRoleName.IsSatisfyBy(data, violations);
        }

        internal class HasValidRoleName : SpecificationBase<RoleEntry>
        {
            protected override bool IsSatisfyBy(RoleEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.RoleName) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidRoleName, null, ErrorMessage.Messages[ErrorCode.InvalidRoleName]));
                    return false;
                }

                if (data.RoleName.Length > 250 && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidRoleName, data.RoleName, ErrorMessage.Messages[ErrorCode.InvalidRoleName]));
                    return false;
                }
                return true;
            }
        }
    }
}
