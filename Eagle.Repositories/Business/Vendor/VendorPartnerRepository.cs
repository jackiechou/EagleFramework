using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Common;
using Eagle.Entities.Business.Vendors;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.Business.Vendor
{
    public class VendorPartnerRepository : RepositoryBase<VendorPartner>, IVendorPartnerRepository
    {
        public VendorPartnerRepository(IDataContext dataContext) : base(dataContext) { }

        public IEnumerable<VendorPartner> GetPartners(string searchText, bool? status, ref int? recordCount,
           string orderBy = null, int? page = null, int? pageSize = null)
        {
            var query = DataContext.Get<VendorPartner>().Where(x =>
            (status == null || x.Status == status));
            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(x => x.PartnerName.ToLower().Contains(searchText));
            }
            return query.WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }
        public VendorPartner FindByPartnerName(string partnerName)
        {
            return DataContext.Get<VendorPartner>().FirstOrDefault(x => x.PartnerName.ToLower() == partnerName.ToLower());
        }
        public bool HasDataExisted(string partnerName)
        {
            var entity =
                DataContext.Get<VendorPartner>().FirstOrDefault(x => x.PartnerName.Contains(partnerName));
            return entity != null;
        }

        public bool HasEmailExisted(string email)
        {
            var entity = (from v in DataContext.Get<VendorPartner>()
                          where v.Email == email
                          select v).FirstOrDefault();
            return entity != null;
        }

        public SelectList PopulatePartnerStatus(bool? selectedValue = true, bool isShowSelectText = false)
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
    }
}
