using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Contents.Articles;
using Eagle.Services.Validations;

namespace Eagle.Services.Contents.Validation
{
    public class NewsCategoryEntryValidator : SpecificationBase<NewsCategoryEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }
        public NewsCategoryEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel, ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(NewsCategoryEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundNewsCategoryEntry, "NewsCategoryEntry"));
                return false;
            }

            ISpecification<NewsCategoryEntry> isValidName = new HasValidName();
            ISpecification<NewsCategoryEntry> isValidParentId = new HasValidParentId();
            return isValidName.And(isValidParentId).IsSatisfyBy(data, violations);
        }

        private class HasValidName : SpecificationBase<NewsCategoryEntry>
        {
            protected override bool IsSatisfyBy(NewsCategoryEntry data, IList<RuleViolation> violations = null)
            {
                var isValid = (!string.IsNullOrEmpty(data.CategoryName) || data.CategoryName.Length > 300);
                if (!isValid && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidCategoryName, "CategoryName"));
                    return false;
                }
                else
                {
                    bool isDuplicate = UnitOfWork.NewsCategoryRepository.HasDataExisted(data.CategoryName, data.ParentId);
                    if (isDuplicate)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicateCategoryName, "CategoryName", data.CategoryName));
                            return false;
                        }
                    }
                    return true;
                }
            }
        }

        public class HasValidParentId : SpecificationBase<NewsCategoryEntry>
        {
            protected override bool IsSatisfyBy(NewsCategoryEntry data, IList<RuleViolation> violations = null)
            {
                if (data.ParentId !=null && data.ParentId >= int.MaxValue)
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
