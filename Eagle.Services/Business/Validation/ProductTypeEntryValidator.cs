using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Validations;

namespace Eagle.Services.Business.Validation
{
    public class ProductTypeEntryValidator : SpecificationBase<ProductTypeEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; set; }

        public ProductTypeEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel,
            ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(ProductTypeEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundProductTypeEntry,"ProductTypeEntry",null, ErrorMessage.Messages[ErrorCode.NotFoundProductTypeEntry]));
                return false;
            }
            ISpecification<ProductTypeEntry> hasValidProductTypeName = new HasValidProductTypeName();
            ISpecification<ProductTypeEntry> hasValidProductCategoryId = new HasValidProductCategoryId();
            var result = hasValidProductTypeName.And(hasValidProductCategoryId).IsSatisfyBy(data, violations);
            return result;
        }
       
        private class HasValidProductCategoryId : SpecificationBase<ProductTypeEntry>
        {
            protected override bool IsSatisfyBy(ProductTypeEntry data, IList<RuleViolation> violations = null)
            {
                if (data.CategoryId > 0) { 
                    var item = UnitOfWork.ProductCategoryRepository.FindById(data.CategoryId);
                    if (item == null && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidProductCategoryId, "CategoryId", data.CategoryId, ErrorMessage.Messages[ErrorCode.InvalidProductCategoryId]));
                        return false;
                    }
                }
                return true;
            }
        }
        private class HasValidProductTypeName : SpecificationBase<ProductTypeEntry>
        {
            protected override bool IsSatisfyBy(ProductTypeEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.TypeName) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidProductTypeName,"TypeName", data.TypeName, ErrorMessage.Messages[ErrorCode.InvalidProductTypeName]));
                    return false;
                }
                return true;
            }
        }
    }
}
