using System.Collections.Generic;
using System.Security.Claims;
using Eagle.Core.Permission;
using Eagle.Repositories;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Business.Personnel;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Validations;

namespace Eagle.Services.Business.Validation
{
    public class ContractEntryValidator : SpecificationBase<ContractEntry>
    {
        private ClaimsPrincipal CurrentClaimsIdentity { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        private static IUnitOfWork UnitOfWork { get; set; }

        public ContractEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel, ClaimsPrincipal currentClaimsIdentity)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
            CurrentClaimsIdentity = currentClaimsIdentity;
        }

        protected override bool IsSatisfyBy(ContractEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundContractEntry, "Contract entry", null, ErrorMessage.Messages[ErrorCode.NotFoundContractEntry]));
                return false;
            }

            //ISpecification<ContractEntry> validPermission = new PermissionValidator<CompanyEntry>(CurrentClaimsIdentity, ModuleDefinition.Contract, PermissionLevel.View);
            ISpecification<ContractEntry> hasValidContractNo = new HasValidContractNo();
            ISpecification<ContractEntry> hasValidCompanyId = new HasValidCompanyId();
            ISpecification<ContractEntry> hasValidEmployeeId = new HasValidEmployeeId();
            ISpecification<ContractEntry> hasValidPositionId = new HasValidPositionId();

            return hasValidContractNo.And(hasValidCompanyId).And(hasValidEmployeeId).And(hasValidPositionId).IsSatisfyBy(data, violations);
        }

        private class HasValidContractNo : SpecificationBase<ContractEntry>
        {
            protected override bool IsSatisfyBy(ContractEntry data, IList<RuleViolation> violations = null)
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
                    else
                    {
                        bool isDuplicate = UnitOfWork.ContractRepository.HasContractNoExisted(data.ContractNo);
                        if (isDuplicate)
                        {
                            if (violations != null)
                            {
                                violations.Add(new RuleViolation(ErrorCode.DuplicateContractNo, "ContractNo", data.ContractNo, ErrorMessage.Messages[ErrorCode.DuplicateContractNo]));
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
        }
        
        private class HasValidCompanyId : SpecificationBase<ContractEntry>
        {
            protected override bool IsSatisfyBy(ContractEntry data, IList<RuleViolation> violations = null)
            {
                if (data.CompanyId <=0 && violations != null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidCompanyId, "CompanyId", data.CompanyId, ErrorMessage.Messages[ErrorCode.InvalidCompanyId]));
                    return false;
                }
                else
                {
                    var company = UnitOfWork.CompanyRepository.FindById(data.CompanyId);
                    if (company == null && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.InvalidCompanyId, "CompanyId", data.CompanyId, ErrorMessage.Messages[ErrorCode.InvalidCompanyId]));
                        return false;
                    }
                }
                return true;
            }
        }

        private class HasValidEmployeeId : SpecificationBase<ContractEntry>
        {
            protected override bool IsSatisfyBy(ContractEntry data, IList<RuleViolation> violations = null)
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

        private class HasValidPositionId : SpecificationBase<ContractEntry>
        {
            protected override bool IsSatisfyBy(ContractEntry data, IList<RuleViolation> violations = null)
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
