using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Validations;

namespace Eagle.Services.Business.Validation
{
    public class VendorEntryValidator : SpecificationBase<VendorEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; set; }

        public VendorEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel,
            ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(VendorEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundVendorEntry,"VendorEntry", null, ErrorMessage.Messages[ErrorCode.NotFoundVendorEntry]));
                return false;
            }

            ISpecification<VendorEntry> hasValidVendorName = new HasValidVendorName();
            var result = hasValidVendorName.IsSatisfyBy(data, violations);
            return result;
        }
      
        private class HasValidVendorName : SpecificationBase<VendorEntry>
        {
            protected override bool IsSatisfyBy(VendorEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.VendorName) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidVendorName, "VendorName", null,
                        ErrorMessage.Messages[ErrorCode.InvalidVendorName]));
                    return false;
                }
                else
                {
                    var item = UnitOfWork.VendorRepository.FindByVendorName(data.VendorName);
                    if (item != null && violations !=null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.DuplicateVendorName, "VendorName", data.VendorName));
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
