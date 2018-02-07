using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Common.Extensions;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Validations;

namespace Eagle.Services.Business.Validation
{
    public class BrandEntryValidator : SpecificationBase<BrandEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; set; }

        public BrandEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel,
            ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(BrandEntry data, IList<RuleViolation> violations = null)
        {
            ISpecification<BrandEntry> hasValidBrandName = new HasValidBrandName();
            return hasValidBrandName.IsSatisfyBy(data, violations);
        }

        private class HasValidBrandName : SpecificationBase<BrandEntry>
        {
            protected override bool IsSatisfyBy(BrandEntry data, IList<RuleViolation> violations = null)
            {
                if (data.BrandName.IsNullOrEmpty())
                {
                    violations?.Add(new RuleViolation(ErrorCode.InvalidBrandName, "BrandName", null, ErrorMessage.Messages[ErrorCode.InvalidBrandName]));
                    return false;
                }
                else
                {
                    if (data.BrandName.Length > 500)
                    {
                        violations?.Add(new RuleViolation(ErrorCode.InvalidBrandName, "BrandName", null, ErrorMessage.Messages[ErrorCode.InvalidBrandName]));
                        return false;
                    }

                    if (UnitOfWork.BrandRepository.CheckExistedName(data.BrandName))
                    {
                        violations?.Add(new RuleViolation(ErrorCode.DuplicatedBrandName, "BrandName", null, ErrorMessage.Messages[ErrorCode.DuplicatedBrandName]));
                        return false;
                    }
                }
                return true;
            }
        }
    }

    public class BrandEditEntryValidator : SpecificationBase<BrandEditEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; set; }

        public BrandEditEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel,
            ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(BrandEditEntry data, IList<RuleViolation> violations = null)
        {
            ISpecification<BrandEditEntry> hasValidBrandName = new HasValidBrandName();
            return hasValidBrandName.IsSatisfyBy(data, violations);
        }

        private class HasValidBrandName : SpecificationBase<BrandEditEntry>
        {
            protected override bool IsSatisfyBy(BrandEditEntry data, IList<RuleViolation> violations = null)
            {
                if (data.BrandName.IsNullOrEmpty())
                {
                    violations?.Add(new RuleViolation(ErrorCode.InvalidBrandName, "BrandName", null, ErrorMessage.Messages[ErrorCode.InvalidBrandName]));
                    return false;
                }
                else
                {
                    if (data.BrandName.Length > 500)
                    {
                        violations?.Add(new RuleViolation(ErrorCode.InvalidBrandName, "BrandName", null,
                            ErrorMessage.Messages[ErrorCode.InvalidBrandName]));
                        return false;
                    }
                    else
                    {
                        var entity = UnitOfWork.BrandRepository.FindById(data.BrandId);
                        if (entity == null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.NotFoundBrand, "BrandEditEntry", data.BrandId,
                                ErrorMessage.Messages[ErrorCode.NotFoundBrand]));
                            return false;
                        }
                        else
                        {
                            if (entity.BrandName.ToLower() != data.BrandName.ToLower())
                            {
                                bool isDuplicate = UnitOfWork.BrandRepository.CheckExistedName(data.BrandName);
                                if (isDuplicate)
                                {
                                    violations?.Add(new RuleViolation(ErrorCode.DuplicatedBrandName, "BrandName", data.BrandName, ErrorMessage.Messages[ErrorCode.DuplicatedBrandName]));
                                    return false;
                                }
                            }
                            return true;
                        }
                    }
                }
            }
        }
    }
}
