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
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<EmployeeInfo> GetEmployees(int vendorId, string employeeName, EmployeeStatus? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var query = from e in DataContext.Get<Employee>()
                        join c in DataContext.Get<Contact>() on e.ContactId equals c.ContactId into contactInfo
                        from contact in contactInfo.DefaultIfEmpty()
                        join co in DataContext.Get<Company>() on e.CompanyId equals co.CompanyId into companyInfo
                        from company in companyInfo.DefaultIfEmpty()
                        join p in DataContext.Get<JobPosition>() on e.PositionId equals p.PositionId into positionInfo
                        from position in positionInfo.DefaultIfEmpty()
                        join a in DataContext.Get<Address>() on e.EmergencyAddressId equals a.AddressId into emergencyAddressInfo
                        from emergencyAddress in emergencyAddressInfo.DefaultIfEmpty()
                        join b in DataContext.Get<Address>() on e.PermanentAddressId equals b.AddressId into permanentAddressInfo
                        from permanentAddress in permanentAddressInfo.DefaultIfEmpty()
                        where e.VendorId == vendorId && (status == null || e.Status == status)
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
                            EmergencyAddress = emergencyAddress,
                            PermanentAddress = permanentAddress,
                            Company = company,
                            JobPosition = position
                        };

            if (!string.IsNullOrEmpty(employeeName))
            {
                query = query.Where(x => x.Contact.FirstName.ToLower() == employeeName.ToLower() || x.Contact.LastName.ToLower() == employeeName.ToLower());
            }

            return query.AsEnumerable().WithRecordCount(ref recordCount)
                            .WithSortingAndPaging(orderBy, page, pageSize);
        }

        public IEnumerable<EmployeeInfo> GetEmployees(int vendorId, EmployeeStatus? status)
        {
            return (from e in DataContext.Get<Employee>()
                    join c in DataContext.Get<Contact>() on e.ContactId equals c.ContactId into contactInfo
                    from contact in contactInfo.DefaultIfEmpty()
                    join co in DataContext.Get<Company>() on e.CompanyId equals co.CompanyId into companyInfo
                    from company in companyInfo.DefaultIfEmpty()
                    join p in DataContext.Get<JobPosition>() on e.PositionId equals p.PositionId into positionInfo
                    from position in positionInfo.DefaultIfEmpty()
                    join a in DataContext.Get<Address>() on e.EmergencyAddressId equals a.AddressId into emergencyAddressInfo
                    from emergencyAddress in emergencyAddressInfo.DefaultIfEmpty()
                    join b in DataContext.Get<Address>() on e.PermanentAddressId equals b.AddressId into permanentAddressInfo
                    from permanentAddress in permanentAddressInfo.DefaultIfEmpty()
                    where e.VendorId == vendorId && (status == null || e.Status == status)
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
                        EmergencyAddress = emergencyAddress,
                        PermanentAddress = permanentAddress,
                        Company = company,
                        JobPosition = position
                    }).AsEnumerable();
        }
        public SelectList PopulateEmployeeSelectList(int vendorId, EmployeeStatus? status = null, int? selectedValue = null, bool? isShowSelectText = false)
        {
            var listItems = new List<SelectListItem>();
            var lst = (from e in DataContext.Get<Employee>()
                       join c in DataContext.Get<Contact>() on e.ContactId equals c.ContactId into contactInfo
                       from contact in contactInfo.DefaultIfEmpty()
                       join co in DataContext.Get<Company>() on e.CompanyId equals co.CompanyId into companyInfo
                       from company in companyInfo.DefaultIfEmpty()
                       join p in DataContext.Get<JobPosition>() on e.PositionId equals p.PositionId into positionInfo
                       from position in positionInfo.DefaultIfEmpty()
                       join a in DataContext.Get<Address>() on e.EmergencyAddressId equals a.AddressId into emergencyAddressInfo
                       from emergencyAddress in emergencyAddressInfo.DefaultIfEmpty()
                       join b in DataContext.Get<Address>() on e.PermanentAddressId equals b.AddressId into permanentAddressInfo
                       from permanentAddress in permanentAddressInfo.DefaultIfEmpty()
                       where e.VendorId == vendorId && (status == null || e.Status == status)
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
                           EmergencyAddress = emergencyAddress,
                           PermanentAddress = permanentAddress,
                           Company = company,
                           JobPosition = position
                       }).ToList();

            if (lst.Any())
            {
                listItems = lst.Select(p => new SelectListItem { Text = $"{p.Contact.FirstName} {p.Contact.LastName}", Value = p.EmployeeId.ToString(), Selected = (selectedValue != null && p.EmployeeId == selectedValue) }).ToList();
                if (isShowSelectText != null && isShowSelectText == true)
                    listItems.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectEmployee} ---", Value = "" });
            }
            else
            {
                listItems.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "-1" });
            }

            return new SelectList(listItems, "Value", "Text", selectedValue);
        }

        public MultiSelectList PopulateEmployeeMultiSelectList(int vendorId, EmployeeStatus? status = null, int[] selectedValues = null)
        {
            var listItems = new List<SelectListItem>();
            var lst = (from e in DataContext.Get<Employee>()
                       join c in DataContext.Get<Contact>() on e.ContactId equals c.ContactId into contactInfo
                       from contact in contactInfo.DefaultIfEmpty()
                       join co in DataContext.Get<Company>() on e.CompanyId equals co.CompanyId into companyInfo
                       from company in companyInfo.DefaultIfEmpty()
                       join p in DataContext.Get<JobPosition>() on e.PositionId equals p.PositionId into positionInfo
                       from position in positionInfo.DefaultIfEmpty()
                       join a in DataContext.Get<Address>() on e.EmergencyAddressId equals a.AddressId into emergencyAddressInfo
                       from emergencyAddress in emergencyAddressInfo.DefaultIfEmpty()
                       join b in DataContext.Get<Address>() on e.PermanentAddressId equals b.AddressId into permanentAddressInfo
                       from permanentAddress in permanentAddressInfo.DefaultIfEmpty()
                       where e.VendorId == vendorId && (status == null || e.Status == status)
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
                           EmergencyAddress = emergencyAddress,
                           PermanentAddress = permanentAddress,
                           Company = company,
                           JobPosition = position
                       }).ToList();

            if (lst.Any())
            {
                listItems = lst.Select(p => new SelectListItem { Text = $"{p.Contact.FirstName} {p.Contact.LastName}", Value = p.EmployeeId.ToString() }).ToList();
            }
            else
            {
                listItems.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "-1" });
            }

            return new MultiSelectList(listItems, "Value", "Text", selectedValues);
        }

        public EmployeeInfo GetDetails(int employeedId)
        {
            return (from e in DataContext.Get<Employee>()
                    join c in DataContext.Get<Contact>() on e.ContactId equals c.ContactId into contactInfo
                    from contact in contactInfo.DefaultIfEmpty()
                    join co in DataContext.Get<Company>() on e.CompanyId equals co.CompanyId into companyInfo
                    from company in companyInfo.DefaultIfEmpty()
                    join p in DataContext.Get<JobPosition>() on e.PositionId equals p.PositionId into positionInfo
                    from position in positionInfo.DefaultIfEmpty()
                    join a in DataContext.Get<Address>() on e.EmergencyAddressId equals a.AddressId into emergencyAddressInfo
                    from emergencyAddress in emergencyAddressInfo.DefaultIfEmpty()
                    join b in DataContext.Get<Address>() on e.PermanentAddressId equals b.AddressId into permanentAddressInfo
                    from permanentAddress in permanentAddressInfo.DefaultIfEmpty()
                    where e.EmployeeId == employeedId
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
                        EmergencyAddress = emergencyAddress,
                        PermanentAddress = permanentAddress,
                        Company = company,
                        JobPosition = position
                    }).FirstOrDefault();
        }

        public bool HasDataExisted(string employeeName)
        {
            var entity = (from e in DataContext.Get<Employee>()
                          join c in DataContext.Get<Contact>() on e.ContactId equals c.ContactId into contactInfo
                          from contact in contactInfo.DefaultIfEmpty()
                          where contact.FirstName.ToLower().Contains(employeeName.ToLower())
                          select e).FirstOrDefault();
            return entity != null;
        }

        public bool HasEmployeeNumberExisted(string employeeNumber)
        {
            var entity = (from e in DataContext.Get<Employee>()
                          where e.EmployeeNo.ToLower().Contains(employeeNumber.ToLower())
                          select e).FirstOrDefault();
            return entity != null;
        }

        public bool HasEmailExisted(string email)
        {
            var entity = (from e in DataContext.Get<Employee>()
                          join c in DataContext.Get<Contact>() on e.ContactId equals c.ContactId into contactInfo
                          from contact in contactInfo.DefaultIfEmpty()
                          where contact.Email == email
                          select e).FirstOrDefault();
            return (entity != null);
        }
        public bool HasMobileExisted(string mobile)
        {
            var entity = (from e in DataContext.Get<Employee>()
                          join c in DataContext.Get<Contact>() on e.ContactId equals c.ContactId into contactInfo
                          from contact in contactInfo.DefaultIfEmpty()
                          where contact.Mobile == mobile
                          select e).FirstOrDefault();
            return (entity != null);
        }

        public bool HasPhoneExisted(string phone)
        {
            var entity = (from e in DataContext.Get<Employee>()
                          join c in DataContext.Get<Contact>() on e.ContactId equals c.ContactId into contactInfo
                          from contact in contactInfo.DefaultIfEmpty()
                          where contact.LinePhone1 == phone || contact.LinePhone2 == phone
                          select e).FirstOrDefault();
            return (entity != null);
        }

        public string GenerateCode(int maxLetters)
        {
            int newId = 1;
            var query = from u in DataContext.Get<Employee>() select u.EmployeeId;
            if (query.Any())
            {
                newId = query.Max() + 1;
            }
            return StringUtils.GenerateCode(newId.ToString(), maxLetters);
        }

    }
}
