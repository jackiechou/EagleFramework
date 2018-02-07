using System;
using System.Linq;
using Eagle.Core.Settings;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework;

namespace Eagle.Repositories.SystemManagement
{
    public class ContactRepository : RepositoryBase<Contact>, IContactRepository
    {
        public ContactRepository(IDataContext dataContext) : base(dataContext)
        {
        }
       
        public bool HasDataExisted(string email, string mobile, string firstName, string lastName, SexType? sex, 
             DateTime? dob, string linePhone1, string linePhone2, string taxNo)
        {
            var query = DataContext.Get<Contact>().Where(c => 
                    c.Email.ToLower() == email.ToLower() && c.Mobile.ToLower() == mobile.ToLower());

            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
            {
                query = query.Where(x => x.FirstName.ToLower() == firstName.ToLower() && x.LastName.ToLower() == lastName.ToLower());
            }

            if (sex!=null)
            {
                query = query.Where(x=>x.Sex == sex);
            }

            if (dob != null)
            {
                query = query.Where(x => x.Dob == dob);
            }

            if (!string.IsNullOrEmpty(linePhone1) || !string.IsNullOrEmpty(linePhone2))
            {
                query = query.Where(x => x.LinePhone1 == linePhone1 || x.LinePhone2 == linePhone2);
            }

            if (!string.IsNullOrEmpty(taxNo))
            {
                query = query.Where(x => x.TaxNo == taxNo);
            }

            return (query.Any());
        }
    }
}
