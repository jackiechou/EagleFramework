using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Validations;

namespace Eagle.Services.Business.Validation
{
    public class CompanyEditEntryValidator : SpecificationBase<CompanyEditEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }
        public CompanyEditEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel, ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(CompanyEditEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundCompanyEditEntry, "Company entry"));
                return false;
            }

            //ISpecification<CompanyEditEntry> validPermission = new PermissionValidator<CompanyEditEntry>(CurrentClaimsIdentity, ModuleDefinition.Company, PermissionLevel.View);
            ISpecification<CompanyEditEntry> hasValidCompanyName = new HasValidCompanyName();
            var result = hasValidCompanyName.IsSatisfyBy(data, violations);
            return true;
        }

        private class HasValidCompanyName : SpecificationBase<CompanyEditEntry>
        {
            protected override bool IsSatisfyBy(CompanyEditEntry data, IList<RuleViolation> violations = null)
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
                        var company = UnitOfWork.CompanyRepository.FindById(data.CompanyId);
                        if (company == null)
                        {
                            if (violations != null)
                            {
                                violations.Add(new RuleViolation(ErrorCode.NullCompany, "Company", null, ErrorMessage.Messages[ErrorCode.NullCompany]));
                                return false;
                            }
                        }
                        else
                        {
                            if (company.CompanyName != data.CompanyName)
                            {
                                var isDuplicated = UnitOfWork.CompanyRepository.HasDataExisted(data.CompanyName, data.ParentId);
                                if (isDuplicated)
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
                    }
                }
                return true;
            }
        }

        public class HasValidParentId : SpecificationBase<CompanyEditEntry>
        {
            protected override bool IsSatisfyBy(CompanyEditEntry data, IList<RuleViolation> violations = null)
            {
                if (data.ParentId != null && data.ParentId >= int.MaxValue)
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidParentId, "ParentId", data.ParentId, ErrorMessage.Messages[ErrorCode.InvalidParentId]));
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
