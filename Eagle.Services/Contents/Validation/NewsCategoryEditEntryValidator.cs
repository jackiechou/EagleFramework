using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Contents.Articles;
using Eagle.Services.Validations;

namespace Eagle.Services.Contents.Validation
{
    public class NewsCategoryEditEntryValidator : SpecificationBase<NewsCategoryEditEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }
        public NewsCategoryEditEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel, ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(NewsCategoryEditEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundNewsCategoryEditEntry, "NewsCategoryEditEntry"));
                return false;
            }

            ISpecification<NewsCategoryEditEntry> isValidName = new HasValidName();
            ISpecification<NewsCategoryEditEntry> isValidParentId = new HasValidParentId();
            return isValidName.And(isValidParentId).IsSatisfyBy(data, violations);
        }

        private class HasValidName : SpecificationBase<NewsCategoryEditEntry>
        {
            protected override bool IsSatisfyBy(NewsCategoryEditEntry data, IList<RuleViolation> violations = null)
            {
                var isValid = (!string.IsNullOrEmpty(data.CategoryName) || data.CategoryName.Length > 300);
                if (!isValid && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidCategoryName, "CategoryName"));
                    return false;
                }

                return true;
            }
        }

        public class HasValidParentId : SpecificationBase<NewsCategoryEditEntry>
        {
            protected override bool IsSatisfyBy(NewsCategoryEditEntry data, IList<RuleViolation> violations = null)
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
