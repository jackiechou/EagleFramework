using System;
using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Core.Permission;
using Eagle.Core.Settings;
using Eagle.Repositories;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Validations;

namespace Eagle.Services.Business.Validation
{
    public class PromotionEntryValidator : SpecificationBase<PromotionEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; set; }

        public PromotionEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel,
            ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(PromotionEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullReferenceForPromotionEntry,"PromotionEntry",null, ErrorMessage.Messages[ErrorCode.NullReferenceForPromotionEntry]));
                return false;
            }
           
            ISpecification<PromotionEntry> hasValidPromotionValue = new HasValidPromotionValue();
            ISpecification<PromotionEntry> hasValidPromotionType = new HasValidPromotionType();
            var result = hasValidPromotionValue.And(hasValidPromotionType).IsSatisfyBy(data, violations);
            return result;
        }

        private class HasValidPromotionType : SpecificationBase<PromotionEntry>
        {
            protected override bool IsSatisfyBy(PromotionEntry data, IList<RuleViolation> violations = null)
            {
                var isValid = Enum.IsDefined(typeof(PromotionType), data.PromotionType);
                if (!isValid && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidPromotionType, "PromotionType", data.PromotionType, ErrorMessage.Messages[ErrorCode.InvalidPromotionType]));
                    return false;
                }
                return true;
            }
        }

        private class HasValidPromotionValue : SpecificationBase<PromotionEntry>
        {
            protected override bool IsSatisfyBy(PromotionEntry data, IList<RuleViolation> violations = null)
            {
                if (data.PromotionValue < 0 && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidPromotionValue, "PromotionValue",null, ErrorMessage.Messages[ErrorCode.InvalidPromotionValue]));
                    return false;
                }
                return true;
            }
        }
       
    }
}
