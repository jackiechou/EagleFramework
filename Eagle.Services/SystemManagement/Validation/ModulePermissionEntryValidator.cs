using System.Collections.Generic;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.SystemManagement;
using Eagle.Services.Validations;

namespace Eagle.Services.SystemManagement.Validation
{
    public class ModulePermissionEntryValidator : SpecificationBase<ModulePermissionEntry>
    {
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        public ModulePermissionEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
        }
        protected override bool IsSatisfyBy(ModulePermissionEntry data, IList<RuleViolation> violations = null)
        {
            ISpecification<ModulePermissionEntry> hasValidModuleId = new HasValidModuleId();
            ISpecification<ModulePermissionEntry> hasValidRoleId = new HasValidRoleId();
            //ISpecification<ModulePermissionEntry> hasValidCapability = new HasValidCapability();
            //            return hasValidModuleId.And(hasValidRoleId).And(hasValidCapability).IsSatisfyBy(data, violations);
            return hasValidModuleId.And(hasValidRoleId).IsSatisfyBy(data, violations);
        }

        internal class HasValidModuleId : SpecificationBase<ModulePermissionEntry>
        {
            protected override bool IsSatisfyBy(ModulePermissionEntry data, IList<RuleViolation> violations = null)
            {
                var entity = UnitOfWork.ModuleRepository.FindById(data.ModuleId);
                if (entity == null)
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidModuleId, "ModuleId"));
                    }
                    return false;
                }
                return true;
            }
        }
        internal class HasValidRoleId : SpecificationBase<ModulePermissionEntry>
        {
            protected override bool IsSatisfyBy(ModulePermissionEntry data, IList<RuleViolation> violations = null)
            {
                var entity = UnitOfWork.RoleRepository.FindById(data.RoleId);
                if (entity == null)
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidRoleId, "RoleId"));
                    }
                    return false;
                }
                return true;
            }
        }

        //internal class HasValidCapability : SpecificationBase<ModulePermissionEntry>
        //{
        //    protected override bool IsSatisfyBy(ModulePermissionEntry data, IList<RuleViolation> violations = null)
        //    {
        //        if (!data.Capabilities.Any())
        //        {
        //            if (violations != null)
        //            {
        //                violations.Add(new RuleViolation(ErrorCodeType.NotFoundCapability, "RoleId"));
        //            }
        //            return false;
        //        }
        //        return true;
        //    }
        //}
    }
}
