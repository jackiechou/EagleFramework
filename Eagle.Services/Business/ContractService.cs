using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Permission;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Companies;
using Eagle.Entities.Business.Employees;
using Eagle.Entities.SystemManagement;
using Eagle.Repositories;
using Eagle.Services.Business.Validation;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Business.Personnel;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.SystemManagement.Identity;
using Eagle.Services.EntityMapping.Common;
using Eagle.Services.Validations;

namespace Eagle.Services.Business
{
    public class ContractService : BaseService, IContractService
    {
        #region Contruct
        private ICurrencyService CurrencyService { get; set; }
        public ContractService(IUnitOfWork unitOfWork, ICurrencyService currencyService) : base(unitOfWork)
        {
            CurrencyService = currencyService;
        }

        #endregion

        #region Contract
        public IEnumerable<ContractInfoDetail> GetContracts(ContractSearchEntry filter, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = new List<ContractInfoDetail>();
            var contracts = UnitOfWork.ContractRepository.GetContracts(filter.Keywords, filter.ContractTypeId, filter.IsActive, ref recordCount, orderBy, page, pageSize);
            if (contracts != null)
            {
                lst.AddRange(contracts.Select(contract => new ContractInfoDetail
                {
                    ContractId = contract.ContractId,
                    ContractNo = contract.ContractNo,
                    ContractTypeId = contract.ContractTypeId,
                    CompanyId = contract.CompanyId,
                    EmployeeId = contract.EmployeeId,
                    PositionId = contract.PositionId,
                    CurrencyCode = contract.CurrencyCode,
                    ProbationSalary = contract.ProbationSalary,
                    InsuranceSalary = contract.InsuranceSalary,
                    ProbationFrom = contract.ProbationFrom,
                    ProbationTo = contract.ProbationTo,
                    SignedDate = contract.SignedDate,
                    EffectiveDate = contract.EffectiveDate,
                    ExpiredDate = contract.ExpiredDate,
                    Description = contract.Description,
                    IsActive = contract.IsActive,
                    Employee = contract.Employee.ToDto<Employee, EmployeeDetail>(),
                    Contact = contract.Contact.ToDto<Contact, ContactDetail>(),
                    Company = contract.Company.ToDto<Company, CompanyDetail>(),
                    JobPosition = contract.JobPosition.ToDto<JobPosition, JobPositionDetail>()
                }));
            }
            return lst;
        }

        public ContractDetail GetContractDetail(int id)
        {
            var entity = UnitOfWork.ContractRepository.Find(id);
            return entity.ToDto<Contract, ContractDetail>();
        }

        public ContractInfoDetail GetContractDetails(int id)
        {
            var contract = UnitOfWork.ContractRepository.GetDetails(id);
            return new ContractInfoDetail
            {
                ContractId = contract.ContractId,
                ContractNo = contract.ContractNo,
                ContractTypeId = contract.ContractTypeId,
                CompanyId = contract.CompanyId,
                EmployeeId = contract.EmployeeId,
                PositionId = contract.PositionId,
                CurrencyCode = contract.CurrencyCode,
                ProbationSalary = contract.ProbationSalary,
                InsuranceSalary = contract.InsuranceSalary,
                ProbationFrom = contract.ProbationFrom,
                ProbationTo = contract.ProbationTo,
                SignedDate = contract.SignedDate,
                EffectiveDate = contract.EffectiveDate,
                ExpiredDate = contract.ExpiredDate,
                Description = contract.Description,
                IsActive = contract.IsActive,
                Employee = contract.Employee.ToDto<Employee, EmployeeDetail>(),
                Contact = contract.Contact.ToDto<Contact, ContactDetail>(),
                Company = contract.Company.ToDto<Company, CompanyDetail>(),
                JobPosition = contract.JobPosition.ToDto<JobPosition, JobPositionDetail>()
            };
        }
        public string GenerateCode(int maxLetters)
        {
            return UnitOfWork.ContractRepository.GenerateCode(maxLetters);
        }
        public SelectList PopulateContractStatus(bool? selectedValue = true, bool isShowSelectText = false)
        {
            return UnitOfWork.ContractRepository.PopulateContractStatus(selectedValue, isShowSelectText);
        }

        public void InsertContract(ContractEntry entry)
        {
            ISpecification<ContractEntry> validator = new ContractEntryValidator(UnitOfWork, PermissionLevel.Create, CurrentClaimsIdentity);
            var violations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, violations);
            if (!isDataValid) throw new ValidationError(violations);

            var entity = entry.ToEntity<ContractEntry, Contract>();
            entity.ContractTypeId = ContractType.LaborContract;
            entity.CurrencyCode = CurrencyService.GetSelectedCurrency().CurrencyCode;

            UnitOfWork.ContractRepository.Insert(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateContract(ContractEditEntry entry)
        {
            ISpecification<ContractEditEntry> validator = new ContractEditEntryValidator(UnitOfWork, PermissionLevel.Edit, CurrentClaimsIdentity);
            var violations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, violations);
            if (!isDataValid) throw new ValidationError(violations);

            var entity = UnitOfWork.ContractRepository.FindById(entry.ContractId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullContract, "ContractEditEntry", null, ErrorMessage.Messages[ErrorCode.NullContract]));
                throw new ValidationError(violations);
            }

            if (entity.ContractNo != entry.ContractNo)
            {
                bool isDuplicate = UnitOfWork.ContractRepository.HasContractNoExisted(entry.ContractNo);
                if (isDuplicate)
                {
                    violations.Add(new RuleViolation(ErrorCode.DuplicateContractNo, "ContractNo", entry.ContractNo, ErrorMessage.Messages[ErrorCode.DuplicateContractNo]));
                    throw new ValidationError(violations);
                }
            }
            
            entity.ContractNo = entry.ContractNo;
            entity.CompanyId = entry.CompanyId;
            entity.EmployeeId = entry.EmployeeId;
            entity.PositionId = entry.PositionId;
            entity.ProbationSalary = entry.ProbationSalary;
            entity.InsuranceSalary = entry.InsuranceSalary;
            entity.ProbationFrom = entry.ProbationFrom;
            entity.ProbationTo = entry.ProbationTo;
            entity.SignedDate = entry.SignedDate;
            entity.EffectiveDate = entry.EffectiveDate;
            entity.ExpiredDate = entry.ExpiredDate;
            entity.Description = entry.Description;
            entity.IsActive = entry.IsActive;

            UnitOfWork.ContractRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateContractStatus(int id, bool status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ContractRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidContractId, "ContractId", id, ErrorMessage.Messages[ErrorCode.InvalidContractId]));
                throw new ValidationError(violations);
            }
            
            if (entity.IsActive == status) return;

            entity.IsActive = status;

            UnitOfWork.ContractRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region Dipose

        private bool _disposed = false;
        protected override void Dispose(bool isDisposing)
        {
            if (!this._disposed)
            {
                if (isDisposing)
                {
                    CurrencyService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}
