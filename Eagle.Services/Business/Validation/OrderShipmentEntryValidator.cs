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
    public class OrderShipmentEntryValidator : SpecificationBase<OrderShipmentEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; set; }

        public OrderShipmentEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel,
            ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(OrderShipmentEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullOrderShipmentEntry, "OrderShipmentEntry", null, ErrorMessage.Messages[ErrorCode.NullOrderShipmentEntry]));
                return false;
            }

            ISpecification<OrderShipmentEntry> hasValidOrderNo = new HasValidOrderNo();
            ISpecification<OrderShipmentEntry> hasValidProductId = new HasValidShippingMethodId();
            ISpecification<OrderShipmentEntry> hasValidReceiverAddress = new HasValidReceiverAddress();
            ISpecification<OrderShipmentEntry> hasValidCountryId = new HasValidCountryId();
            var result = hasValidOrderNo.And(hasValidProductId).And(hasValidReceiverAddress).And(hasValidCountryId)
                .IsSatisfyBy(data, violations);
            return result;
        }

        private class HasValidOrderNo : SpecificationBase<OrderShipmentEntry>
        {
            protected override bool IsSatisfyBy(OrderShipmentEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.OrderNo.ToString()) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullOrderNo, "OrderNo", null, ErrorMessage.Messages[ErrorCode.NullOrderNo]));
                    return false;
                }
                else
                {
                    var item = UnitOfWork.OrderRepository.FindByOrderNo(data.OrderNo);
                    if (item == null && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidOrderNo, "OrderNo", data.OrderNo, ErrorMessage.Messages[ErrorCode.InvalidOrderNo]));
                        return false;
                    }
                }
                return true;
            }
        }
        private class HasValidShippingMethodId : SpecificationBase<OrderShipmentEntry>
        {
            protected override bool IsSatisfyBy(OrderShipmentEntry data, IList<RuleViolation> violations = null)
            {
                if (data.ShippingMethodId <= 0 && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidShippingMethodId, "ShippingMethodId", null, ErrorMessage.Messages[ErrorCode.InvalidShippingMethodId]));
                    return false;
                }
                return true;
            }
        }
        private class HasValidReceiverAddress : SpecificationBase<OrderShipmentEntry>
        {
            protected override bool IsSatisfyBy(OrderShipmentEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.ReceiverAddress) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullReceiverAddress, "ReceiverAddress", null, ErrorMessage.Messages[ErrorCode.NullReceiverAddress]));
                    return false;
                }

                return true;
            }
        }

        private class HasValidCountryId : SpecificationBase<OrderShipmentEntry>
        {
            protected override bool IsSatisfyBy(OrderShipmentEntry data, IList<RuleViolation> violations = null)
            {
                if (data.CountryId < 0 && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidCountryId, "UnitPrice", data.CountryId, ErrorMessage.Messages[ErrorCode.InvalidCountryId]));
                    return false;
                }

                return true;
            }
        }
    }
}
