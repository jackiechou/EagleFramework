using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Business.Personnel;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Validations;

namespace Eagle.Services.Business.Validation
{
    public class ContractEditEntryValidator : SpecificationBase<ContractEditEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }

        public ContractEditEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel, ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(ContractEditEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundContractEditEntry, "Contract entry for editing", null, ErrorMessage.Messages[ErrorCode.NotFoundContractEditEntry]));
                return false;
            }

            //ISpecification<ContractEditEntry> validPermission = new PermissionValidator<CompanyEntry>(CurrentClaimsIdentity, ModuleDefinition.Contract, PermissionLevel.View);
            ISpecification<ContractEditEntry> hasValidContractNo = new HasValidContractNo();
            ISpecification<ContractEditEntry> hasValidCompanyId = new HasValidCompanyId();
            ISpecification<ContractEditEntry> hasValidEmployeeId = new HasValidEmployeeId();
            ISpecification<ContractEditEntry> hasValidPositionId = new HasValidPositionId();

            return hasValidContractNo.And(hasValidCompanyId).And(hasValidEmployeeId).And(hasValidPositionId).IsSatisfyBy(data, violations);
        }

        private class HasValidContractNo : SpecificationBase<ContractEditEntry>
        {
            protected override bool IsSatisfyBy(ContractEditEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.ContractNo))
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.NullContractNo, "ContractNo", null, ErrorMessage.Messages[ErrorCode.NullContractNo]));
                        return false;
                    }
                }
                else
                {
                    if (data.ContractNo.Length > 50)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidContractNo, "ContractNo", data.ContractNo, ErrorMessage.Messages[ErrorCode.InvalidContractNo]));
                            return false;
                        }
                    }
                }
                return true;
            }
        }

        private class HasValidCompanyId : SpecificationBase<ContractEditEntry>
        {
            protected override bool IsSatisfyBy(ContractEditEntry data, IList<RuleViolation> violations = null)
            {
                if (data.CompanyId <= 0 && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidCompanyId, "CompanyId", data.CompanyId,
                        ErrorMessage.Messages[ErrorCode.InvalidCompanyId]));
                    return false;
                }
                else
                {
                    var company = UnitOfWork.CompanyRepository.FindById(data.CompanyId);
                    if (company == null)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidCompanyId, "CompanyId", data.CompanyId,
                                ErrorMessage.Messages[ErrorCode.InvalidCompanyId]));
                            return false;
                        }
                    }
                }
                return true;
            }
        }

        private class HasValidEmployeeId : SpecificationBase<ContractEditEntry>
        {
            protected override bool IsSatisfyBy(ContractEditEntry data, IList<RuleViolation> violations = null)
            {
                if (data.EmployeeId <= 0 && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidEmployeeId, "EmployeeId", data.EmployeeId,
                        ErrorMessage.Messages[ErrorCode.InvalidEmployeeId]));
                    return false;
                }
                else
                {
                    var employee = UnitOfWork.EmployeeRepository.FindById(data.EmployeeId);
                    if (employee == null)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidEmployeeId, "EmployeeId", data.EmployeeId,
                                ErrorMessage.Messages[ErrorCode.InvalidEmployeeId]));
                            return false;
                        }
                    }
                }
                return true;
            }
        }

        private class HasValidPositionId : SpecificationBase<ContractEditEntry>
        {
            protected override bool IsSatisfyBy(ContractEditEntry data, IList<RuleViolation> violations = null)
            {
                if (data.EmployeeId <= 0 && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidEmployeeId, "EmployeeId", data.EmployeeId,
                        ErrorMessage.Messages[ErrorCode.InvalidEmployeeId]));
                    return false;
                }
                else
                {
                    var position = UnitOfWork.JobPositionRepository.FindById(data.PositionId);
                    if (position == null)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidPositionId, "PositionId", data.PositionId,
                                ErrorMessage.Messages[ErrorCode.InvalidPositionId]));
                            return false;
                        }
                    }
                }
                return true;
            }
        }
    }
}
