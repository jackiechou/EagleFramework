using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Validations;

namespace Eagle.Services.Business.Validation
{
    public class CompanyEntryValidator : SpecificationBase<CompanyEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }

        public CompanyEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel, ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(CompanyEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundCompanyEntry, "Company entry", null, ErrorMessage.Messages[ErrorCode.NotFoundCompanyEntry]));
                return false;
            }

            //ISpecification<CompanyEntry> validPermission = new PermissionValidator<CompanyEntry>(CurrentClaimsIdentity, ModuleDefinition.Company, PermissionLevel.View);
            ISpecification<CompanyEntry> hasValidCompanyName = new HasValidCompanyName();
            var result = hasValidCompanyName.IsSatisfyBy(data, violations);
            return true;
        }

        private class HasValidCompanyName : SpecificationBase<CompanyEntry>
        {
            protected override bool IsSatisfyBy(CompanyEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.CompanyName))
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.NullCompanyName, "CompanyName", null, ErrorMessage.Messages[ErrorCode.NullCompanyName]));
                        return false;
                    }
                }
                else
                {
                    if (data.CompanyName.Length > 300)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidCompanyName, "CompanyName",data.CompanyName, ErrorMessage.Messages[ErrorCode.InvalidCompanyName]));
                            return false;
                        }
                    }
                    else
                    {
                        bool isDuplicate = UnitOfWork.CompanyRepository.HasDataExisted(data.CompanyName, data.ParentId);
                        if (isDuplicate)
                        {
                            if (violations != null)
                            {
                                violations.Add(new RuleViolation(ErrorCode.DuplicateCompanyName, "CompanyName",
                                    data.CompanyName, ErrorMessage.Messages[ErrorCode.DuplicateCompanyName]));
                                return false;
                            }
                        }
                        return true;
                    }
                }
                return true;
            }
        }
    }
}
