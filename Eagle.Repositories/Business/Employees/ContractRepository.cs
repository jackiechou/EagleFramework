using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Common.Utilities;
using Eagle.Core.Common;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Companies;
using Eagle.Entities.Business.Employees;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.Business.Employees
{
   public class ContractRepository : RepositoryBase<Contract>, IContractRepository
    {
        public ContractRepository(IDataContext dataContext) : base(dataContext) { }

        public IEnumerable<ContractInfo> GetContracts(string searchText, ContractType? contractTypeId, bool? status, ref int? recordCount,
           string orderBy = null, int? page = null, int? pageSize = null)
        {
            var queryable = from c in DataContext.Get<Contract>()
                            join e in DataContext.Get<Employee>() on c.EmployeeId equals e.EmployeeId into employeeInfo
                            from employee in employeeInfo.DefaultIfEmpty()
                            join co in DataContext.Get<Contact>() on employee.ContactId equals co.ContactId into contactInfo
                            from contact in contactInfo.DefaultIfEmpty()
                            join com in DataContext.Get<Company>() on c.CompanyId equals com.CompanyId into companyInfo
                            from company in companyInfo.DefaultIfEmpty()
                            join p in DataContext.Get<JobPosition>() on c.PositionId equals p.PositionId into positionInfo
                            from position in positionInfo.DefaultIfEmpty()
                            where status == null || c.IsActive == status
                            select new ContractInfo
                            {
                                ContractId = c.ContractId,
                                ContractNo = c.ContractNo,
                                ContractTypeId = c.ContractTypeId,
                                CompanyId = c.CompanyId,
                                EmployeeId = c.EmployeeId,
                                PositionId = c.PositionId,
                                CurrencyCode = c.CurrencyCode,
                                ProbationSalary = c.ProbationSalary,
                                InsuranceSalary = c.InsuranceSalary,
                                ProbationFrom = c.ProbationFrom,
                                ProbationTo = c.ProbationTo,
                                SignedDate = c.SignedDate,
                                EffectiveDate = c.EffectiveDate,
                                ExpiredDate = c.ExpiredDate,
                                Description = c.Description,
                                IsActive = c.IsActive,
                                Employee = employee,
                                Contact= contact,
                                Company = company,
                                JobPosition = position
                            };
            if (contractTypeId != null && contractTypeId > 0)
            {
                queryable = queryable.Where(x => x.ContractTypeId == contractTypeId);
            }

            if (!string.IsNullOrEmpty(searchText))
            {
                queryable = queryable.Where(x => x.ContractNo.ToString().Contains(searchText));
            }
            return queryable.WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }

       public ContractInfo GetDetails(int id)
       {
           return (from c in DataContext.Get<Contract>()
                   join e in DataContext.Get<Employee>() on c.EmployeeId equals e.EmployeeId into employeeInfo
                   from employee in employeeInfo.DefaultIfEmpty()
                   join co in DataContext.Get<Contact>() on employee.ContactId equals co.ContactId into contactInfo
                   from contact in contactInfo.DefaultIfEmpty()
                   join com in DataContext.Get<Company>() on c.CompanyId equals com.CompanyId into companyInfo
                   from company in companyInfo.DefaultIfEmpty()
                   join p in DataContext.Get<JobPosition>() on c.PositionId equals p.PositionId into positionInfo
                   from position in positionInfo.DefaultIfEmpty()
                   select new ContractInfo
                   {
                       ContractId = c.ContractId,
                       ContractNo = c.ContractNo,
                       ContractTypeId = c.ContractTypeId,
                       CompanyId = c.CompanyId,
                       EmployeeId = c.EmployeeId,
                       PositionId = c.PositionId,
                       CurrencyCode = c.CurrencyCode,
                       ProbationSalary = c.ProbationSalary,
                       InsuranceSalary = c.InsuranceSalary,
                       ProbationFrom = c.ProbationFrom,
                       ProbationTo = c.ProbationTo,
                       SignedDate = c.SignedDate,
                       EffectiveDate = c.EffectiveDate,
                       ExpiredDate = c.ExpiredDate,
                       Description = c.Description,
                       IsActive = c.IsActive,
                       Employee = employee,
                       Contact = contact,
                       Company = company,
                       JobPosition = position
                   }).FirstOrDefault();
        }

        public bool HasContractNoExisted(string contractNo)
        {
            var entity =
               DataContext.Get<Contract>().FirstOrDefault(x => x.ContractNo.ToLower().Contains(contractNo.ToLower()));
            return entity != null;
        }

        public SelectList PopulateContractStatus(bool? selectedValue = true, bool isShowSelectText = false)
        {
            List<SelectListItem> lst = new List<SelectListItem>
            {
                new SelectListItem {Text = LanguageResource.Active, Value = "True", Selected = (selectedValue!=null && selectedValue==true) },
                new SelectListItem {Text = LanguageResource.InActive, Value = "False", Selected = (selectedValue!=null && selectedValue==false) }
            };
            if (isShowSelectText)
                lst.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectStatus} ---", Value = "" });
            return new SelectList(lst, "Value", "Text", selectedValue);
        }

        public string GenerateCode(int maxLetters)
        {
            int newId = 1;
            var query = from u in DataContext.Get<Contract>() select u.ContractId;
            if (query.Any())
            {
                newId = query.Max() + 1;
            }
            return StringUtils.GenerateCode(newId.ToString(), maxLetters);
        }
    }
}
