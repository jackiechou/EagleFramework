using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Services.Booking;
using Eagle.Services.Validations;

namespace Eagle.Services.Service.Validation
{
    public class ServiceCategoryEditEntryValidator : SpecificationBase<ServiceCategoryEditEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        public ServiceCategoryEditEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel, ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(ServiceCategoryEditEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullServiceCategoryEditEntry, "ServiceCategoryEditEntry", null, ErrorMessage.Messages[ErrorCode.NullServiceCategoryEditEntry]));
                return false;
            }

            ISpecification<ServiceCategoryEditEntry> isValidName = new HasValidName();
            ISpecification<ServiceCategoryEditEntry> isValidParentId = new HasValidParentId();
            return isValidName.And(isValidParentId).IsSatisfyBy(data, violations);
        }

        private class HasValidName : SpecificationBase<ServiceCategoryEditEntry>
        {
            protected override bool IsSatisfyBy(ServiceCategoryEditEntry data, IList<RuleViolation> violations = null)
            {
                var isValid = (!string.IsNullOrEmpty(data.CategoryName) || data.CategoryName.Length > 300);
                if (!isValid && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidCategoryName, "CategoryName", null, ErrorMessage.Messages[ErrorCode.InvalidCategoryName]));
                    return false;
                }
                else
                {
                    bool isDuplicate = UnitOfWork.NewsCategoryRepository.HasDataExisted(data.CategoryName, data.ParentId);
                    if (isDuplicate)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicateCategoryName, "CategoryName", data.CategoryName, ErrorMessage.Messages[ErrorCode.DuplicateCategoryName]));
                            return false;
                        }
                    }
                    return true;
                }
            }
        }

        public class HasValidParentId : SpecificationBase<ServiceCategoryEditEntry>
        {
            protected override bool IsSatisfyBy(ServiceCategoryEditEntry data, IList<RuleViolation> violations = null)
            {
                if (data.ParentId != null && data.ParentId >= int.MaxValue && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidParentId, "ParentId", data.ParentId, ErrorMessage.Messages[ErrorCode.InvalidParentId]));
                    return false;
                }
                return true;
            }
        }


    }
}
