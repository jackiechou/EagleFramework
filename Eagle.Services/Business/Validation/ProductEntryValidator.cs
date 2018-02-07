using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Validations;

namespace Eagle.Services.Business.Validation
{
    public class ProductEntryValidator : SpecificationBase<ProductEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; set; }

        public ProductEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel,
            ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }
        protected override bool IsSatisfyBy(ProductEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundProductEntry, "ProductEntry",null, ErrorMessage.Messages[ErrorCode.NotFoundProductEntry]));
                return false;
            }

            ISpecification<ProductEntry> hasValidProductCode = new HasValidProductCode();
            ISpecification<ProductEntry> hasValidProductName = new HasValidProductName();
            ISpecification<ProductEntry> hasValidCategoryId = new HasValidCategoryId();
            ISpecification<ProductEntry> hasValidProductTypeId = new HasValidProductTypeId();
            ISpecification<ProductEntry> hasValidTaxRateId = new HasValidTaxRateId();
           
            return hasValidProductCode.And(hasValidProductName).And(hasValidCategoryId).And(hasValidProductTypeId).And(hasValidTaxRateId).IsSatisfyBy(data, violations);
        }
        private class HasValidProductCode : SpecificationBase<ProductEntry>
        {
            protected override bool IsSatisfyBy(ProductEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.ProductCode))
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.NullProductCode, "ProductCode",null, ErrorMessage.Messages[ErrorCode.NullProductCode]));
                        return false;
                    }
                }
                else
                {
                    if (data.ProductCode.Length > 50)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidProductCode, "ProductCode", data.ProductCode, ErrorMessage.Messages[ErrorCode.InvalidProductCode]));
                            return false;
                        }
                    }
                    else
                    {
                        var isProductCodeDuplicated = UnitOfWork.ProductRepository.HasProductCodeExisted(data.ProductCode);
                        if (isProductCodeDuplicated)
                        {
                            if (violations != null)
                            {
                                violations.Add(new RuleViolation(ErrorCode.DuplicatedProductCode, "ProductCode", data.ProductCode, ErrorMessage.Messages[ErrorCode.DuplicatedProductCode]));
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
        }
        private class HasValidProductName : SpecificationBase<ProductEntry>
        {
            protected override bool IsSatisfyBy(ProductEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.ProductName))
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidProductName, "ProductName",null, ErrorMessage.Messages[ErrorCode.InvalidProductName]));
                        return false;
                    }
                }
                else
                {
                    if (data.ProductName.Length > 500)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidProductName, "ProductName", data.ProductName, ErrorMessage.Messages[ErrorCode.InvalidProductName]));
                            return false;
                        }
                    }
                    else
                    {
                        var isNameDuplicated = UnitOfWork.ProductRepository.HasProductNameExisted(data.ProductName);
                        if (isNameDuplicated)
                        {
                            if (violations != null)
                            {
                                violations.Add(new RuleViolation(ErrorCode.DuplicatedProductName, "ProductName", data.ProductName, ErrorMessage.Messages[ErrorCode.DuplicatedProductName]));
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
        }
      
        private class HasValidCategoryId : SpecificationBase<ProductEntry>
        {
            protected override bool IsSatisfyBy(ProductEntry data, IList<RuleViolation> violations = null)
            {
                if (data.CategoryId <= 0 && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullProductCategoryId, "CategoryId", data.CategoryId, ErrorMessage.Messages[ErrorCode.NullProductCategoryId]));
                    return false;
                }
                else
                {
                    var category = UnitOfWork.ProductCategoryRepository.FindById(data.CategoryId);
                    if (category == null && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidProductCategoryId, "CategoryId", data.CategoryId, ErrorMessage.Messages[ErrorCode.InvalidProductCategoryId]));
                        return false;
                    }
                }
                return true;
            }
        }

        private class HasValidProductTypeId : SpecificationBase<ProductEntry>
        {
            protected override bool IsSatisfyBy(ProductEntry data, IList<RuleViolation> violations = null)
            {
                if (data.ProductTypeId == null && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullProductTypeId, "ProductTypeId", data.ProductTypeId, ErrorMessage.Messages[ErrorCode.NullProductTypeId]));
                    return false;
                }
                else
                {
                    var productType = UnitOfWork.ProductTypeRepository.FindById(data.ProductTypeId);
                    if (productType == null && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidProductTypeId, "ProductTypeId", data.ProductTypeId, ErrorMessage.Messages[ErrorCode.InvalidProductTypeId]));
                        return false;
                    }
                }
                return true;
            }
        }

        private class HasValidTaxRateId : SpecificationBase<ProductEntry>
        {
            protected override bool IsSatisfyBy(ProductEntry data, IList<RuleViolation> violations = null)
            {
                if (data.TaxRateId != null && data.TaxRateId > 0)
                {
                    var productType = UnitOfWork.ProductTaxRateRepository.FindById(data.TaxRateId);
                    if (productType == null && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidProductTaxRateId, "ProductTaxRateId", data.TaxRateId, ErrorMessage.Messages[ErrorCode.InvalidProductTaxRateId]));
                        return false;
                    }
                }
                return true;
            }
        }

    }
}
