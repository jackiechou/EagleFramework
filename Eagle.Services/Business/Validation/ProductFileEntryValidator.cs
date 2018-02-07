using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Validations;

namespace Eagle.Services.Business.Validation
{
    public class ProductFileEntryValidator : SpecificationBase<ProductFileEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; set; }

        public ProductFileEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel,
            ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(ProductFileEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundProductFileEntry,"ProductFileEntry",null, ErrorMessage.Messages[ErrorCode.NotFoundProductFileEntry]));
                return false;
            }
            ISpecification<ProductFileEntry> hasValidVendorId = new HasValidVendorId();
            ISpecification<ProductFileEntry> hasValidProductId = new HasValidProductId();
            ISpecification<ProductFileEntry> hasValidFileName = new HasValidFileName();

            var result = hasValidVendorId.And(hasValidProductId).And(hasValidFileName).IsSatisfyBy(data, violations);
            return result;
        }
        private class HasValidVendorId : SpecificationBase<ProductFileEntry>
        {
            protected override bool IsSatisfyBy(ProductFileEntry data, IList<RuleViolation> violations = null)
            {
                if (data.VendorId < 0 && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullVendorId, "VendorId", null, ErrorMessage.Messages[ErrorCode.NullVendorId]));
                    return false;
                }
                else
                {
                    var item = UnitOfWork.VendorRepository.FindById(data.VendorId);
                    if (item == null && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidVendorId, "VendorId", data.VendorId, ErrorMessage.Messages[ErrorCode.InvalidVendorId]));
                        return false;
                    }
                    return true;
                }
            }
        }
        private class HasValidProductId : SpecificationBase<ProductFileEntry>
        {
            protected override bool IsSatisfyBy(ProductFileEntry data, IList<RuleViolation> violations = null)
            {
                var item = UnitOfWork.ProductRepository.FindById(data.ProductId);
                if (item == null && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidProductId, "ProductId", data.VendorId, ErrorMessage.Messages[ErrorCode.InvalidProductId]));
                    return false;
                }
                return true;
            }
        }

        private class HasValidFileName : SpecificationBase<ProductFileEntry>
        {
            protected override bool IsSatisfyBy(ProductFileEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.FileName) && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NullFileName, "FileName", data.FileName, ErrorMessage.Messages[ErrorCode.NullFileName]));
                    return false;
                }
                return true;
            }
        }
    }
}
