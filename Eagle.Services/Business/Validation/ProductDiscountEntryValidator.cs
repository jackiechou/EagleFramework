using System;
using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Validations;

namespace Eagle.Services.Business.Validation
{
    public class ProductDiscountEntryValidator : SpecificationBase<ProductDiscountEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; set; }

        public ProductDiscountEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel,
            ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(ProductDiscountEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundProductDiscountEntry,"ProductDiscountEntry",null, ErrorMessage.Messages[ErrorCode.NotFoundProductDiscountEntry]));
                return false;
            }
            ISpecification<ProductDiscountEntry> hasValidDiscountRate = new HasValidDiscountRate();
            ISpecification<ProductDiscountEntry> hasValidDate = new HasValidDate();

            return hasValidDiscountRate.And(hasValidDate).IsSatisfyBy(data, violations);
        }
    
        private class HasValidDiscountRate : SpecificationBase<ProductDiscountEntry>
        {
            protected override bool IsSatisfyBy(ProductDiscountEntry data, IList<RuleViolation> violations = null)
            {
                if (data.DiscountRate < 0 && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidDiscountRate, "TaxRate", data.DiscountRate, ErrorMessage.Messages[ErrorCode.InvalidDiscountRate]));
                    return false;
                }
                return true;
            }
        }

        private class HasValidDate : SpecificationBase<ProductDiscountEntry>
        {
            protected override bool IsSatisfyBy(ProductDiscountEntry data, IList<RuleViolation> violations = null)
            {
                if (data.StartDate.HasValue && data.EndDate.HasValue)
                {
                    if (DateTime.Compare(data.EndDate.Value, data.EndDate.Value) > 0)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.EndDateMustBeGreaterThanStartDate, "EndDate", data.EndDate, ErrorMessage.Messages[ErrorCode.EndDateMustBeGreaterThanStartDate]));
                            return false;
                        }
                    }
                }
                return true;
            }
        }
    }
}
