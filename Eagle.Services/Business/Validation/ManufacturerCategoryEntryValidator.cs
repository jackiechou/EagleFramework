using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Validations;

namespace Eagle.Services.Business.Validation
{
    public class ManufacturerCategoryEntryValidator : SpecificationBase<ManufacturerCategoryEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; set; }

        public ManufacturerCategoryEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel, ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(ManufacturerCategoryEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundManufacturerCategoryEntry, "ManufacturerCategoryEntry",null, ErrorMessage.Messages[ErrorCode.NotFoundManufacturerCategoryEntry]));
                return false;
            }

            //ISpecification<ManufacturerCategoryEntry> validPermission = new PermissionValidator<ManufacturerCategoryEntry>(CurrentClaimsIdentity, ModuleDefinition.Company, PermissionLevel.View);
            ISpecification<ManufacturerCategoryEntry> hasValidManufacturerCategoryName = new HasValidManufacturerCategoryName();

            var result = hasValidManufacturerCategoryName.IsSatisfyBy(data, violations);
            return result;
        }
   
        private class HasValidManufacturerCategoryName : SpecificationBase<ManufacturerCategoryEntry>
        {
            protected override bool IsSatisfyBy(ManufacturerCategoryEntry data, IList<RuleViolation> violations = null)
            {
                if (!string.IsNullOrEmpty(data.CategoryName) && data.CategoryName.Length > 300)
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidManufacturerCategoryName, "CategoryName", data.CategoryName, ErrorMessage.Messages[ErrorCode.InvalidManufacturerCategoryName]));
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
