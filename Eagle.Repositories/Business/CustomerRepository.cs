using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Common.Utilities;
using Eagle.Core.Common;
using Eagle.Core.Settings;
using Eagle.Entities.Business.Customers;
using Eagle.Entities.Common;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.Business
{
    public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
    {
        public CustomerRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<CustomerInfo> GetCustomers(CustomerStatus? status)
        {
            return (from c in DataContext.Get<Customer>()
                         join a in DataContext.Get<Address>() on c.AddressId equals a.AddressId into caJoin
                         from address in caJoin.DefaultIfEmpty()
                         where status == null || c.IsActive == status
                         select new CustomerInfo
                         {
                             CustomerId = c.CustomerId,
                             CustomerTypeId = c.CustomerTypeId,
                             CustomerNo = c.CustomerNo,
                             FirstName = c.FirstName,
                             LastName = c.LastName,
                             ContactName = c.ContactName,
                             Email = c.Email,
                             Mobile = c.Mobile,
                             HomePhone = c.HomePhone,
                             WorkPhone = c.WorkPhone,
                             Fax = c.Fax,
                             CardNo = c.CardNo,
                             IdCardNo = c.IdCardNo,
                             PassPortNo = c.PassPortNo,
                             TaxCode = c.TaxCode,
                             Gender = c.Gender,
                             BirthDay = c.BirthDay,
                             AddressId = c.AddressId,
                             IsActive = c.IsActive,
                             Address = address
                         }).AsEnumerable();
        }
        public IEnumerable<Customer> GetCustomers(string searchText, int? customerTypeId, CustomerStatus? status, ref int? recordCount,
           string orderBy = null, int? page = null, int? pageSize = null)
        {
            var queryable = DataContext.Get<Customer>().Where(x => (status == null || x.IsActive == status));
            if (customerTypeId != null && customerTypeId > 0)
            {
                queryable = queryable.Where(x => x.CustomerTypeId == customerTypeId);
            }

            if (!string.IsNullOrEmpty(searchText))
                queryable = queryable.Where(x => x.CustomerNo.ToString().Contains(searchText)
                            || x.Mobile.Contains(searchText)
                            || x.HomePhone.Contains(searchText)
                            || x.WorkPhone.Contains(searchText)
                            || x.Email.Contains(searchText)
                            || x.FirstName.Contains(searchText)
                            || x.LastName.Contains(searchText));
            return queryable.AsEnumerable().WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }
        
        public IEnumerable<AutoComplete> GetCustomerAutoCompleteList(string searchTerm, CustomerStatus? status, out int recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var query = (from x in DataContext.Get<Customer>()
                         where status == null || x.IsActive == status
                         select new AutoComplete
                         {
                             Id = x.CustomerId,
                             Name = x.FirstName + " " + x.LastName,
                             Text = x.FirstName + " " + x.LastName,
                             Level = 1
                         }).AsEnumerable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                string search = StringUtils.ConvertToUnSign(searchTerm.ToLower());
                query = query.Where(x => StringUtils.ConvertToUnSign(x.Text.ToLower()).Contains(search));
            }
            return query.WithRecordCount(out recordCount)
                           .WithSortingAndPaging(orderBy, page, pageSize);
        }

        public SelectList PopulateCustomerSelectList(CustomerStatus? status = null, int? selectedValue = null, bool? isShowSelectText = true)
        {
            var listItems = new List<SelectListItem>();
            var lst = (from c in DataContext.Get<Customer>()
                       where status == null || c.IsActive == status
                       select c).ToList();

            if (lst.Any())
            {
                listItems = lst.Select(p => new SelectListItem { Text = $"{p.FirstName} {p.LastName}", Value = p.CustomerId.ToString(), Selected = (selectedValue!=null && p.CustomerId == selectedValue) }).ToList();
                if (isShowSelectText != null && isShowSelectText == true)
                    listItems.Insert(0, new SelectListItem { Text = $@"--- {LanguageResource.SelectCustomer} ---", Value = "" });
            }
            else
            {
                listItems.Insert(0, new SelectListItem { Text = $@"-- {LanguageResource.None} --", Value = "-1" });
            }
            return new SelectList(listItems, "Value", "Text", selectedValue);
        }

        public Customer FindByCustomerNo(string customerNo)
        {
            return DataContext.Get<Customer>().FirstOrDefault(x => x.CustomerNo == customerNo);
        }
        public Customer FindByEmail(string email)
        {
            return DataContext.Get<Customer>().FirstOrDefault(x => x.Email == email);
        }
        public Customer FindByUserAndPassword(string email, string hashedPassword)
        {
            return (from u in DataContext.Get<Customer>()
                    where u.Email.ToLower() == email.ToLower() && u.PasswordHash == hashedPassword
                    select u).FirstOrDefault();
        }

        public bool HasCustomerNameExisted(string firstName, string lastName)
        {
            var entity =
                DataContext.Get<Customer>().FirstOrDefault(x => String.Equals(x.FirstName, firstName, StringComparison.CurrentCultureIgnoreCase) && String.Equals(x.LastName, lastName, StringComparison.CurrentCultureIgnoreCase));
            return entity != null;
        }

        public bool HasEmailExisted(string email)
        {
            var entity =
               DataContext.Get<Customer>().FirstOrDefault(x => x.Email.Contains(email));
            return entity != null;
        }

        public bool HasPhoneExisted(string phone)
        {
            var entity =
               DataContext.Get<Customer>().FirstOrDefault(x => x.Mobile == phone ||
               x.WorkPhone == phone || x.HomePhone == phone);
            return entity != null;
        }

        public bool HasCustomerNumberExisted(string customerNo)
        {
            var entity =
               DataContext.Get<Customer>().FirstOrDefault(x => x.CustomerNo.Contains(customerNo));
            return entity != null;
        }
        public string GenerateCode(int maxLetters)
        {
            int newId = 1;
            var query = from u in DataContext.Get<Customer>() select u.CustomerId;
            if (query.Any())
            {
                newId = query.Max() + 1;
            }
            return StringUtils.GenerateCode(newId.ToString(), maxLetters);
        }
    }
}
