using System;
using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Services.Booking;
using Eagle.Services.Validations;

namespace Eagle.Services.Service.Validation
{
    public class ServiceDiscountEntryValidator : SpecificationBase<ServiceDiscountEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; set; }

        public ServiceDiscountEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel,
            ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(ServiceDiscountEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullServiceDiscountEntry, "ServiceDiscountEntry",null, ErrorMessage.Messages[ErrorCode.NullServiceDiscountEntry]));
                return false;
            }
            ISpecification<ServiceDiscountEntry> hasValidDiscountRate = new HasValidDiscountRate();
            ISpecification<ServiceDiscountEntry> hasValidDate = new HasValidDate();

            return hasValidDiscountRate.And(hasValidDate).IsSatisfyBy(data, violations);
        }

        private class HasValidDiscountRate : SpecificationBase<ServiceDiscountEntry>
        {
            protected override bool IsSatisfyBy(ServiceDiscountEntry data, IList<RuleViolation> violations = null)
            {
                if (data.DiscountRate < 0 && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidDiscountRate, "TaxRate", data.DiscountRate, ErrorMessage.Messages[ErrorCode.InvalidDiscountRate]));
                    return false;
                }
                return true;
            }
        }

        private class HasValidDate : SpecificationBase<ServiceDiscountEntry>
        {
            protected override bool IsSatisfyBy(ServiceDiscountEntry data, IList<RuleViolation> violations = null)
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
