using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Validations;

namespace Eagle.Services.Business.Validation
{
    public class ProductCategoryEntryValidator : SpecificationBase<ProductCategoryEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; set; }

        public ProductCategoryEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel,
            ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }
        protected override bool IsSatisfyBy(ProductCategoryEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundProductCategoryEntry, "ProductCategoryEntry"));
                return false;
            }

            ISpecification<ProductCategoryEntry> hasValidCategoryName = new HasValidCategoryName();
            ISpecification<ProductCategoryEntry> hasValidParentId = new HasValidParentId();
            return hasValidCategoryName.And(hasValidParentId).IsSatisfyBy(data, violations);
        }
        private class HasValidCategoryName : SpecificationBase<ProductCategoryEntry>
        {
            protected override bool IsSatisfyBy(ProductCategoryEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.CategoryName) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullCategoryName, "CategoryName",null, ErrorMessage.Messages[ErrorCode.InvalidUnitPrice]));
                    return false;
                }
                else
                {
                    if (!string.IsNullOrEmpty(data.CategoryName) && data.CategoryName.Length > 250 && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidCategoryName, "CategoryName", data.CategoryName, ErrorMessage.Messages[ErrorCode.InvalidCategoryName]));
                        return false;
                    }
                    else
                    {
                        bool isDuplicate = UnitOfWork.ProductCategoryRepository.HasDataExisted(data.CategoryName, data.ParentId);
                        if (isDuplicate)
                        {
                            if (violations != null)
                            {
                                violations.Add(new RuleViolation(ErrorCode.DuplicateCategoryName, "CategoryName", data.CategoryName, ErrorMessage.Messages[ErrorCode.DuplicateCategoryName]));
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
        }

        public class HasValidParentId : SpecificationBase<ProductCategoryEntry>
        {
            protected override bool IsSatisfyBy(ProductCategoryEntry data, IList<RuleViolation> violations = null)
            {
                if (data.ParentId != null && data.ParentId >= int.MaxValue)
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidParentId, "ParentId", data.ParentId, ErrorMessage.Messages[ErrorCode.InvalidParentId]));
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
