using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Validations;

namespace Eagle.Services.Business.Validation
{
    public class CustomerTypeEntryValidator : SpecificationBase<CustomerTypeEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; set; }

        public CustomerTypeEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel, ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(CustomerTypeEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundCustomerTypeEntry, "Customer Type Entry", null, ErrorMessage.Messages[ErrorCode.NotFoundCustomerTypeEntry]));
                return false;
            }

            //ISpecification<CustomerTypeEntry> validPermission = new PermissionValidator<CustomerTypeEntry>(CurrentClaimsIdentity, ModuleDefinition.Company, PermissionLevel.View);
            ISpecification<CustomerTypeEntry> hasValidCustomerTypeName = new HasValidCustomerTypeName();
            return hasValidCustomerTypeName.IsSatisfyBy(data, violations);
        }

        private class HasValidCustomerTypeName : SpecificationBase<CustomerTypeEntry>
        {
            protected override bool IsSatisfyBy(CustomerTypeEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.CustomerTypeName))
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidCustomerTypeName, "CustomerName", data.CustomerTypeName, ErrorMessage.Messages[ErrorCode.InvalidCustomerTypeName]));
                        return false;
                    }
                }
                else
                {
                    if (data.CustomerTypeName.Length > 200)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidCustomerTypeName, "CustomerTypeName", data.CustomerTypeName, ErrorMessage.Messages[ErrorCode.InvalidCustomerTypeName]));
                            return false;
                        }
                    }
                    else
                    {
                        bool isDuplicate = UnitOfWork.CustomerTypeRepository.HasDataExisted(data.CustomerTypeName);
                        if (isDuplicate)
                        {
                            if (violations != null)
                            {
                                violations.Add(new RuleViolation(ErrorCode.DuplicateCustomerTypeName, "CustomerName", data.CustomerTypeName, ErrorMessage.Messages[ErrorCode.DuplicateCustomerTypeName]));
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
        }
    }
}
