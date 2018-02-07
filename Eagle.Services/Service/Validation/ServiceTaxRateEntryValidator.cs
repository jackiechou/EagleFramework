using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Services.Booking;
using Eagle.Services.Validations;

namespace Eagle.Services.Service.Validation
{
    public class ServiceTaxRateEntryValidator : SpecificationBase<ServiceTaxRateEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; set; }

        public ServiceTaxRateEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel,
            ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(ServiceTaxRateEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullServiceTaxRateEntry, "ServiceTaxRateEntry", null, ErrorMessage.Messages[ErrorCode.NullServiceTaxRateEntry]));
                return false;
            }

            ISpecification<ServiceTaxRateEntry> hasValidServiceTaxRate = new HasValidServiceTaxRate();
            var result = hasValidServiceTaxRate.IsSatisfyBy(data, violations);
            return result;
        }

        private class HasValidServiceTaxRate : SpecificationBase<ServiceTaxRateEntry>
        {
            protected override bool IsSatisfyBy(ServiceTaxRateEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.TaxRate.ToString(CultureInfo.InvariantCulture)) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullServiceTaxRate, "TaxRate", null, ErrorMessage.Messages[ErrorCode.NullServiceTaxRate]));
                    return false;
                }
                else
                {
                    if (data.TaxRate < 0 && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidTaxRate, "TaxRate", data.TaxRate, ErrorMessage.Messages[ErrorCode.InvalidTaxRate]));
                        return false;
                    }
                    return true;
                }
            }
        }
    }
}
