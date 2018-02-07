using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Services.Booking;
using Eagle.Services.Validations;

namespace Eagle.Services.Service.Validation
{
    public class ServiceCategoryEntryValidator : SpecificationBase<ServiceCategoryEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }
        public ServiceCategoryEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel, ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(ServiceCategoryEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundServiceCategoryEntry, "ServiceCategoryEntry"));
                return false;
            }

            ISpecification<ServiceCategoryEntry> isValidName = new HasValidName();
            ISpecification<ServiceCategoryEntry> isValidParentId = new HasValidParentId();
            return isValidName.And(isValidParentId).IsSatisfyBy(data, violations);
        }

        private class HasValidName : SpecificationBase<ServiceCategoryEntry>
        {
            protected override bool IsSatisfyBy(ServiceCategoryEntry data, IList<RuleViolation> violations = null)
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

        public class HasValidParentId : SpecificationBase<ServiceCategoryEntry>
        {
            protected override bool IsSatisfyBy(ServiceCategoryEntry data, IList<RuleViolation> violations = null)
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
