using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Business.Transaction;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Validations;

namespace Eagle.Services.Business.Validation
{
    public class OrderProductEditEntryValidator : SpecificationBase<OrderProductEditEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; set; }

        public OrderProductEditEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel,
            ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(OrderProductEditEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundOrderProductEditEntry, "OrderProductEditEntry", null, ErrorMessage.Messages[ErrorCode.NotFoundOrderProductEditEntry]));
                return false;
            }

            ISpecification<OrderProductEditEntry> hasValidOrderNo = new HasValidOrderNo();
            ISpecification<OrderProductEditEntry> hasValidProductId = new HasValidProductId();
            ISpecification<OrderProductEditEntry> hasValidQuantity = new HasValidQuantity();
            ISpecification<OrderProductEditEntry> hasValidUnitPrice = new HasValidUnitPrice();
            var result = hasValidOrderNo.And(hasValidProductId).And(hasValidQuantity).And(hasValidUnitPrice)
                .IsSatisfyBy(data, violations);
            return result;
        }
        private class HasValidOrderNo : SpecificationBase<OrderProductEditEntry>
        {
            protected override bool IsSatisfyBy(OrderProductEditEntry data, IList<RuleViolation> violations = null)
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
        private class HasValidProductId : SpecificationBase<OrderProductEditEntry>
        {
            protected override bool IsSatisfyBy(OrderProductEditEntry data, IList<RuleViolation> violations = null)
            {
                if (data.ProductId <= 0 && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidProductId, "ProductId", null, ErrorMessage.Messages[ErrorCode.InvalidProductId]));
                    return false;
                }
                return true;
            }
        }
        private class HasValidQuantity : SpecificationBase<OrderProductEditEntry>
        {
            protected override bool IsSatisfyBy(OrderProductEditEntry data, IList<RuleViolation> violations = null)
            {
                if (data.Quantity < 0 && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidQuantity, "Quantity", data.Quantity, ErrorMessage.Messages[ErrorCode.InvalidQuantity]));
                    return false;
                }

                return true;
            }
        }

        private class HasValidUnitPrice : SpecificationBase<OrderProductEditEntry>
        {
            protected override bool IsSatisfyBy(OrderProductEditEntry data, IList<RuleViolation> violations = null)
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
