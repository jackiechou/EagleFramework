using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Validations;

namespace Eagle.Services.Business.Validation
{
    public class ProductTaxRateEntryValidator : SpecificationBase<ProductTaxRateEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; set; }

        public ProductTaxRateEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel,
            ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(ProductTaxRateEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundProductTaxRateEntry,"ProductTaxRateEntry",null, ErrorMessage.Messages[ErrorCode.NotFoundProductTaxRateEntry]));
                return false;
            }

            ISpecification<ProductTaxRateEntry> hasValidProductTaxRate = new HasValidProductTaxRate();
            var result = hasValidProductTaxRate.IsSatisfyBy(data, violations);
            return result;
        }

        private class HasValidProductTaxRate : SpecificationBase<ProductTaxRateEntry>
        {
            protected override bool IsSatisfyBy(ProductTaxRateEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.TaxRate.ToString(CultureInfo.InvariantCulture)) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullProductTaxRate,"TaxRate",null, ErrorMessage.Messages[ErrorCode.NullProductTaxRate]));
                    return false;
                }
                else
                {
                    if (data.TaxRate < 0 && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidProductTaxRate, "TaxRate", data.TaxRate, ErrorMessage.Messages[ErrorCode.InvalidProductTaxRate]));
                        return false;
                    }
                    return true;
                }
            }
        }
    }
}
