using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Business.Transaction;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Validations;

namespace Eagle.Services.Business.Validation
{
    public class OrderEntryValidator : SpecificationBase<OrderEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; set; }

        public OrderEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel,
            ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(OrderEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundOrderEntry, "Order entry",null, ErrorMessage.Messages[ErrorCode.NotFoundOrderEntry]));
                return false;
            }

            ISpecification<OrderEntry> hasValidCustomerId = new HasValidCustomerId();

            var result = hasValidCustomerId.IsSatisfyBy(data, violations);
            return result;
        }

        private class HasValidCustomerId : SpecificationBase<OrderEntry>
        {
            protected override bool IsSatisfyBy(OrderEntry data, IList<RuleViolation> violations = null)
            {
                if (data.CustomerId < 0 && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidCustomerId, "CustomerId", data.CustomerId, ErrorMessage.Messages[ErrorCode.InvalidCustomerId]));
                    return false;
                }
                return true;
            }
        }
    }
}
