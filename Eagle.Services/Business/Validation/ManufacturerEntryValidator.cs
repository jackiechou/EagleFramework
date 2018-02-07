using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Validations;

namespace Eagle.Services.Business.Validation
{
    public class ManufacturerEntryValidator : SpecificationBase<ManufacturerEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; set; }

        public ManufacturerEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel, ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(ManufacturerEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundManufacturerEntry, "ManufacturerEntry", null, ErrorMessage.Messages[ErrorCode.NotFoundManufacturerEntry]));
                return false;
            }

            //ISpecification<ManufacturerEntry> validPermission = new PermissionValidator<ManufacturerEntry>(CurrentClaimsIdentity, ModuleDefinition.Company, PermissionLevel.View);
            ISpecification<ManufacturerEntry> hasValidManufacturerName = new HasValidManufacturerName();
            ISpecification<ManufacturerEntry> hasValidManufacturerCategoryId = new HasValidManufacturerCategoryId();

            var result = hasValidManufacturerName.And(hasValidManufacturerCategoryId).IsSatisfyBy(data, violations);
            return result;
        }
      
        private class HasValidManufacturerName : SpecificationBase<ManufacturerEntry>
        {
            protected override bool IsSatisfyBy(ManufacturerEntry data, IList<RuleViolation> violations = null)
            {
                if (!string.IsNullOrEmpty(data.ManufacturerName) && data.ManufacturerName.Length > 300)
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidManufacturerName, "ManufacturerName", data.ManufacturerName, ErrorMessage.Messages[ErrorCode.InvalidManufacturerName]));
                        return false;
                    }
                }
                return true;
            }
        }

        private class HasValidManufacturerCategoryId : SpecificationBase<ManufacturerEntry>
        {
            protected override bool IsSatisfyBy(ManufacturerEntry data, IList<RuleViolation> violations = null)
            {
                var category = UnitOfWork.ManufacturerCategoryRepository.FindById(data.CategoryId);
                if (category==null && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidCategoryId, "CategoryId", data.CategoryId, ErrorMessage.Messages[ErrorCode.InvalidCategoryId]));
                    return false;
                }
                return true;
            }
        }
    }
}
