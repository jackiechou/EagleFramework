using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Companies;
using Eagle.Entities.Business.Employees;
using Eagle.Entities.Services.Booking;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.Services.Booking
{
    public class ServicePackProviderRepository : RepositoryBase<ServicePackProvider>, IServicePackProviderRepository
    {
        public ServicePackProviderRepository(IDataContext dataContext) : base(dataContext) { }

        public IEnumerable<ServicePackProvider> GetServicePackProviders(int packageId)
        {
            return (from p in DataContext.Get<ServicePackProvider>()
                    where p.PackageId == packageId
                    select p).AsEnumerable();
        }
        public IEnumerable<EmployeeInfo> GetServicePackProviders(int packageId, EmployeeStatus? status)
        {
            var lst = from e in DataContext.Get<Employee>()
                      join p in DataContext.Get<ServicePackProvider>() on e.EmployeeId equals p.EmployeeId into epJoin
                      from ep in epJoin.DefaultIfEmpty()
                      join c in DataContext.Get<Contact>() on e.ContactId equals c.ContactId into contactInfo
                      from contact in contactInfo.DefaultIfEmpty()
                      join com in DataContext.Get<Company>() on e.CompanyId equals com.CompanyId into companyInfo
                      from company in companyInfo.DefaultIfEmpty()
                      join jp in DataContext.Get<JobPosition>() on e.PositionId equals jp.PositionId into positionInfo
                      from jobPostion in positionInfo.DefaultIfEmpty()
                      where ep.PackageId == packageId && (status == null || e.Status == status)
                      select new EmployeeInfo
                      {
                          EmployeeId = e.EmployeeId,
                          EmployeeNo = e.EmployeeNo,
                          ContactId = e.ContactId,
                          EmergencyAddressId = e.EmergencyAddressId,
                          PermanentAddressId = e.PermanentAddressId,
                          VendorId = e.VendorId,
                          CompanyId = e.CompanyId,
                          PositionId = e.PositionId,
                          PasswordHash = e.PasswordHash,
                          PasswordSalt = e.PasswordSalt,
                          JoinedDate = e.JoinedDate,
                          Status = e.Status,
                          Contact = contact,
                          Company = company,
                          JobPosition = jobPostion
                      };
            return lst.AsEnumerable();
        }
        public ServicePackProvider GetDetails(int packageId, int employeeId)
        {
            return (from p in DataContext.Get<ServicePackProvider>()
                    where p.PackageId == packageId && p.EmployeeId == employeeId
                    select p).FirstOrDefault();
        }

        public SelectList PopulateProviderSelectList(int packageId, EmployeeStatus? status = null, int? selectedValue = null, bool? isShowSelectText = true)
        {
            var list = (from e in DataContext.Get<Employee>()
                        join p in DataContext.Get<ServicePackProvider>() on e.EmployeeId equals p.EmployeeId into epJoin
                        from ep in epJoin.DefaultIfEmpty()
                        join c in DataContext.Get<Contact>() on e.ContactId equals c.ContactId into contactInfo
                        from contact in contactInfo.DefaultIfEmpty()
                        join com in DataContext.Get<Company>() on e.CompanyId equals com.CompanyId into companyInfo
                        from company in companyInfo.DefaultIfEmpty()
                        join jp in DataContext.Get<JobPosition>() on e.PositionId equals jp.PositionId into positionInfo
                        from jobPostion in positionInfo.DefaultIfEmpty()
                        where ep.PackageId == packageId && (status == null || e.Status == status)
                        select new EmployeeInfo
                        {
                            EmployeeId = e.EmployeeId,
                            EmployeeNo = e.EmployeeNo,
                            ContactId = e.ContactId,
                            EmergencyAddressId = e.EmergencyAddressId,
                            PermanentAddressId = e.PermanentAddressId,
                            VendorId = e.VendorId,
                            CompanyId = e.CompanyId,
                            PositionId = e.PositionId,
                            PasswordHash = e.PasswordHash,
                            PasswordSalt = e.PasswordSalt,
                            JoinedDate = e.JoinedDate,
                            Status = e.Status,
                            Contact = contact,
                            Company = company,
                            JobPosition = jobPostion
                        }).ToList();

            var listItems = new List<SelectListItem>();
            if (list.Any())
            {
                listItems = list.Select(x => new SelectListItem
                {
                    Text = $"{x.Contact.FirstName} {x.Contact.LastName}",
                    Value = x.EmployeeId.ToString(),
                    Selected = (selectedValue != null && x.EmployeeId == selectedValue)
                }).ToList();

                if (isShowSelectText != null && isShowSelectText == true)
                    listItems.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectProvider} ---", Value = "" });
            }
            else
            {
                listItems.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "-1" });
            }
            return new SelectList(listItems, "Value", "Text", selectedValue);
        }

        public MultiSelectList PopulateAvailableProviderMultiSelectList(int? packageId = null, EmployeeStatus? status = null, string[] selectedValues = null)
        {
            var employees = (from e in DataContext.Get<Employee>()
                             join c in DataContext.Get<Contact>() on e.ContactId equals c.ContactId into contactInfo
                             from contact in contactInfo.DefaultIfEmpty()
                             join com in DataContext.Get<Company>() on e.CompanyId equals com.CompanyId into companyInfo
                             from company in companyInfo.DefaultIfEmpty()
                             join jp in DataContext.Get<JobPosition>() on e.PositionId equals jp.PositionId into positionInfo
                             from jobPostion in positionInfo.DefaultIfEmpty()
                             where (status == null || e.Status == status)
                             select new EmployeeInfo
                             {
                                 EmployeeId = e.EmployeeId,
                                 EmployeeNo = e.EmployeeNo,
                                 ContactId = e.ContactId,
                                 EmergencyAddressId = e.EmergencyAddressId,
                                 PermanentAddressId = e.PermanentAddressId,
                                 VendorId = e.VendorId,
                                 CompanyId = e.CompanyId,
                                 PositionId = e.PositionId,
                                 PasswordHash = e.PasswordHash,
                                 PasswordSalt = e.PasswordSalt,
                                 JoinedDate = e.JoinedDate,
                                 Status = e.Status,
                                 Contact = contact,
                                 Company = company,
                                 JobPosition = jobPostion
                             }).ToList();

            if (packageId != null && packageId > 0)
            {
                var selectedProviders = (from pm in DataContext.Get<ServicePackProvider>()
                                         join p in DataContext.Get<Employee>() on pm.EmployeeId equals p.EmployeeId into empLst
                                         from pl in empLst.DefaultIfEmpty()
                                         where pm.PackageId == packageId
                                         select pl).ToList();

                var selectedEmployeeIds = selectedProviders.Select(x => x.EmployeeId).ToList();
                var empIds = employees.Select(x => x.EmployeeId).ToList();
                var differentIds = empIds.Except(selectedEmployeeIds).ToList();
                employees = employees.Where(x => differentIds.Contains(x.EmployeeId)).ToList();
            }

            var list = employees.Select(x => new SelectListItem
            {
                Text = $"{x.Contact.FirstName} {x.Contact.LastName}",
                Value = x.EmployeeId.ToString()
            }).OrderByDescending(m => m.Text).ToList();

            return new MultiSelectList(list, "Value", "Text", selectedValues);
        }

        public MultiSelectList PopulateSelectedProviderMultiSelectList(int? packageId = null, EmployeeStatus? status = null, string[] selectedValues = null)
        {
            if (packageId == null)
            {
                return new MultiSelectList(new List<SelectListItem>(), "Value", "Text", selectedValues);
            }
            var employees = (from e in DataContext.Get<Employee>()
                             join p in DataContext.Get<ServicePackProvider>() on e.EmployeeId equals p.EmployeeId into epJoin
                             from ep in epJoin.DefaultIfEmpty()
                             join c in DataContext.Get<Contact>() on e.ContactId equals c.ContactId into contactInfo
                             from contact in contactInfo.DefaultIfEmpty()
                             join com in DataContext.Get<Company>() on e.CompanyId equals com.CompanyId into companyInfo
                             from company in companyInfo.DefaultIfEmpty()
                             join jp in DataContext.Get<JobPosition>() on e.PositionId equals jp.PositionId into positionInfo
                             from jobPostion in positionInfo.DefaultIfEmpty()
                             where ep.PackageId == packageId
                             select new EmployeeInfo
                             {
                                 EmployeeId = e.EmployeeId,
                                 EmployeeNo = e.EmployeeNo,
                                 ContactId = e.ContactId,
                                 EmergencyAddressId = e.EmergencyAddressId,
                                 PermanentAddressId = e.PermanentAddressId,
                                 VendorId = e.VendorId,
                                 CompanyId = e.CompanyId,
                                 PositionId = e.PositionId,
                                 PasswordHash = e.PasswordHash,
                                 PasswordSalt = e.PasswordSalt,
                                 JoinedDate = e.JoinedDate,
                                 Status = e.Status,
                                 Contact = contact,
                                 Company = company,
                                 JobPosition = jobPostion
                             }).ToList();

            var list = employees.Select(x => new SelectListItem
            {
                Text = $"{x.Contact.FirstName} {x.Contact.LastName}",
                Value = x.EmployeeId.ToString()
            }).OrderByDescending(m => m.Text).ToList();

            return new MultiSelectList(list, "Value", "Text", selectedValues);

        }
        public bool HasDataExisted(int packageId, int employeeId)
        {
            var query = DataContext.Get<ServicePackProvider>().FirstOrDefault(p => p.PackageId == packageId && p.EmployeeId == employeeId);
            return (query != null);
        }
    }
}
