using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Validations;
using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Services.Dtos.Business.Transaction;

namespace Eagle.Services.Business.Validation
{
    public class OrderProductTempEntryValidator : SpecificationBase<OrderProductTempEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; set; }

        public OrderProductTempEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel,
            ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(OrderProductTempEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundOrderProductTempEntry, "OrderProductTempEntry", null, ErrorMessage.Messages[ErrorCode.NotFoundOrderProductTempEntry]));
                return false;
            }

            ISpecification<OrderProductTempEntry> hasValidOrderNo = new HasValidOrderNo();
            ISpecification<OrderProductTempEntry> hasValidProductId = new HasValidProductId();
            ISpecification<OrderProductTempEntry> hasValidQuantity = new HasValidQuantity();
            ISpecification<OrderProductTempEntry> hasValidUnitPrice = new HasValidUnitPrice();
            var result = hasValidOrderNo.And(hasValidProductId).And(hasValidQuantity).And(hasValidUnitPrice)
                .IsSatisfyBy(data, violations);
            return result;
        }
        private class HasValidOrderNo : SpecificationBase<OrderProductTempEntry>
        {
            protected override bool IsSatisfyBy(OrderProductTempEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.OrderNo.ToString()) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullOrderNo, "OrderNo", null, ErrorMessage.Messages[ErrorCode.NullOrderNo]));
                    return false;
                }
                else
                {
                    var item = UnitOfWork.OrderTempRepository.FindByOrderNo(data.OrderNo);
                    if (item == null && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidOrderNo, "OrderNo", data.OrderNo, ErrorMessage.Messages[ErrorCode.InvalidOrderNo]));
                        return false;
                    }
                }
                return true;
            }
        }
        private class HasValidProductId : SpecificationBase<OrderProductTempEntry>
        {
            protected override bool IsSatisfyBy(OrderProductTempEntry data, IList<RuleViolation> violations = null)
            {
                if (data.ProductId <= 0 && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidProductId, "ProductId", null, ErrorMessage.Messages[ErrorCode.InvalidProductId]));
                    return false;
                }
                return true;
            }
        }
        private class HasValidQuantity : SpecificationBase<OrderProductTempEntry>
        {
            protected override bool IsSatisfyBy(OrderProductTempEntry data, IList<RuleViolation> violations = null)
            {
                if (data.Quantity < 0 && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidQuantity, "Quantity", data.Quantity, ErrorMessage.Messages[ErrorCode.InvalidQuantity]));
                    return false;
                }

                return true;
            }
        }

        private class HasValidUnitPrice : SpecificationBase<OrderProductTempEntry>
        {
            protected override bool IsSatisfyBy(OrderProductTempEntry data, IList<RuleViolation> violations = null)
            {
                if (data.NetPrice < 0 && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidUnitPrice, "UnitPrice", data.NetPrice, ErrorMessage.Messages[ErrorCode.InvalidUnitPrice]));
                    return false;
                }

                return true;
            }
        }
    }
}
