using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Common;
using Eagle.Core.Settings;
using Eagle.Entities.Services.Booking;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.Services.Booking
{
    public class ServicePackRepository : RepositoryBase<ServicePack>, IServicePackRepository
    {
        public ServicePackRepository(IDataContext dataContext) : base(dataContext)
        {
        }
        public IEnumerable<ServicePackInfo> GetServicePacks(int? categoryId, ServicePackStatus? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var queryable = from p in DataContext.Get<ServicePack>()
                            join c in DataContext.Get<ServiceCategory>() on p.CategoryId equals c.CategoryId into pcJoin
                            from category in pcJoin.DefaultIfEmpty()
                            join d in DataContext.Get<ServicePackDuration>() on p.DurationId equals d.DurationId into pdJoin
                            from duration in pdJoin.DefaultIfEmpty()
                            join t in DataContext.Get<ServicePackType>() on p.TypeId equals t.TypeId into ptJoin
                            from type in ptJoin.DefaultIfEmpty()
                            join r in DataContext.Get<ServiceTaxRate>() on p.TaxRateId equals r.TaxRateId into prJoin
                            from tax in prJoin.DefaultIfEmpty()
                            join dis in DataContext.Get<ServiceDiscount>() on p.DiscountId equals dis.DiscountId into pdisJoin
                            from discount in pdisJoin.DefaultIfEmpty()
                            where (status == null || p.Status == status)
                            select new ServicePackInfo
                            {
                                PackageId = p.PackageId,
                                PackageCode = p.PackageCode,
                                PackageName = p.PackageName,
                                CategoryId = p.CategoryId,
                                TypeId = p.TypeId,
                                AvailableQuantity = p.AvailableQuantity,
                                Capacity = p.Capacity,
                                DurationId = p.DurationId,
                                DiscountId = p.DiscountId,
                                TaxRateId = p.TaxRateId,
                                PackageFee = p.PackageFee,
                                TotalFee = p.TotalFee,
                                Weight = p.Weight,
                                CurrencyCode = p.CurrencyCode,
                                FileId = p.FileId,
                                Description = p.Description,
                                Specification = p.Specification,
                                Rating = p.Rating,
                                TotalViews = p.TotalViews,
                                ListOrder = p.ListOrder,
                                Status = p.Status,
                                CreatedDate = p.CreatedDate,
                                LastModifiedDate = p.LastModifiedDate,
                                Category = category,
                                Type = type,
                                Duration = duration,
                                Discount = discount,
                                Tax = tax
                            };
            if (categoryId != null && categoryId > 0)
            {
                queryable = queryable.Where(x => x.CategoryId == categoryId);
            }
            return queryable.AsEnumerable().WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }
        public IEnumerable<ServicePackInfo> GetServicePacks(string servicePackName, int? categoryId, int? typeId, ServicePackStatus? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var queryable = from p in DataContext.Get<ServicePack>()
                            join c in DataContext.Get<ServiceCategory>() on p.CategoryId equals c.CategoryId into pcJoin
                            from category in pcJoin.DefaultIfEmpty()
                            join d in DataContext.Get<ServicePackDuration>() on p.DurationId equals d.DurationId into pdJoin
                            from duration in pdJoin.DefaultIfEmpty()
                            join t in DataContext.Get<ServicePackType>() on p.TypeId equals t.TypeId into ptJoin
                            from type in ptJoin.DefaultIfEmpty()
                            join r in DataContext.Get<ServiceTaxRate>() on p.TaxRateId equals r.TaxRateId into prJoin
                            from tax in prJoin.DefaultIfEmpty()
                            join dis in DataContext.Get<ServiceDiscount>() on p.DiscountId equals dis.DiscountId into pdisJoin
                            from discount in pdisJoin.DefaultIfEmpty()
                            where (status == null || p.Status == status)
                            && (typeId == null || p.TypeId == typeId)
                            select new ServicePackInfo
                            {
                                PackageId = p.PackageId,
                                PackageCode = p.PackageCode,
                                PackageName = p.PackageName,
                                CategoryId = p.CategoryId,
                                TypeId = p.TypeId,
                                AvailableQuantity = p.AvailableQuantity,
                                Capacity = p.Capacity,
                                DurationId = p.DurationId,
                                DiscountId = p.DiscountId,
                                TaxRateId = p.TaxRateId,
                                PackageFee = p.PackageFee,
                                TotalFee = p.TotalFee,
                                Weight = p.Weight,
                                CurrencyCode = p.CurrencyCode,
                                FileId = p.FileId,
                                Description = p.Description,
                                Specification = p.Specification,
                                Rating = p.Rating,
                                TotalViews = p.TotalViews,
                                ListOrder = p.ListOrder,
                                Status = p.Status,
                                CreatedDate = p.CreatedDate,
                                LastModifiedDate = p.LastModifiedDate,
                                Category = category,
                                Type = type,
                                Duration = duration,
                                Discount = discount,
                                Tax = tax
                            };

            if (categoryId != null && categoryId >0)
            {
                queryable = queryable.Where(x => x.CategoryId== categoryId);
            }

            if (!string.IsNullOrEmpty(servicePackName))
            {
                queryable = queryable.Where(x => x.PackageName.ToLower().Contains(servicePackName.ToLower()));
            }

            return queryable.AsEnumerable().WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }
        public IEnumerable<ServicePackInfo> GetServicePacks(int categoryId, ServicePackStatus? status)
        {
            var queryable = from p in DataContext.Get<ServicePack>()
                            join c in DataContext.Get<ServiceCategory>() on p.CategoryId equals c.CategoryId into pcJoin
                            from category in pcJoin.DefaultIfEmpty()
                            join d in DataContext.Get<ServicePackDuration>() on p.DurationId equals d.DurationId into pdJoin
                            from duration in pdJoin.DefaultIfEmpty()
                            join t in DataContext.Get<ServicePackType>() on p.TypeId equals t.TypeId into ptJoin
                            from type in ptJoin.DefaultIfEmpty()
                            join r in DataContext.Get<ServiceTaxRate>() on p.TaxRateId equals r.TaxRateId into prJoin
                            from tax in prJoin.DefaultIfEmpty()
                            join dis in DataContext.Get<ServiceDiscount>() on p.DiscountId equals dis.DiscountId into pdisJoin
                            from discount in pdisJoin.DefaultIfEmpty()
                            where p.CategoryId == categoryId && (status == null || p.Status == status)
                            orderby p.ListOrder descending
                            select new ServicePackInfo
                            {
                                PackageId = p.PackageId,
                                PackageCode = p.PackageCode,
                                PackageName = p.PackageName,
                                CategoryId = p.CategoryId,
                                TypeId = p.TypeId,
                                AvailableQuantity = p.AvailableQuantity,
                                Capacity = p.Capacity,
                                DurationId = p.DurationId,
                                DiscountId = p.DiscountId,
                                TaxRateId = p.TaxRateId,
                                PackageFee = p.PackageFee,
                                TotalFee = p.TotalFee,
                                Weight = p.Weight,
                                CurrencyCode = p.CurrencyCode,
                                FileId = p.FileId,
                                Description = p.Description,
                                Specification = p.Specification,
                                Rating = p.Rating,
                                TotalViews = p.TotalViews,
                                ListOrder = p.ListOrder,
                                Status = p.Status,
                                CreatedDate = p.CreatedDate,
                                LastModifiedDate = p.LastModifiedDate,
                                Category = category,
                                Type = type,
                                Duration = duration,
                                Discount = discount,
                                Tax = tax
                            };

            return queryable.AsEnumerable();
        }
        public IEnumerable<ServicePackInfo> GetServicePacks(int typeId, int categoryId, ServicePackStatus? status)
        {
            var queryable = from p in DataContext.Get<ServicePack>()
                            join c in DataContext.Get<ServiceCategory>() on p.CategoryId equals c.CategoryId into pcJoin
                            from category in pcJoin.DefaultIfEmpty()
                            join d in DataContext.Get<ServicePackDuration>() on p.DurationId equals d.DurationId into pdJoin
                            from duration in pdJoin.DefaultIfEmpty()
                            join t in DataContext.Get<ServicePackType>() on p.TypeId equals t.TypeId into ptJoin
                            from type in ptJoin.DefaultIfEmpty()
                            join r in DataContext.Get<ServiceTaxRate>() on p.TaxRateId equals r.TaxRateId into prJoin
                            from tax in prJoin.DefaultIfEmpty()
                            join dis in DataContext.Get<ServiceDiscount>() on p.DiscountId equals dis.DiscountId into pdisJoin
                            from discount in pdisJoin.DefaultIfEmpty()
                            where p.TypeId == typeId && p.CategoryId == categoryId && (status == null || p.Status == status)
                            orderby p.ListOrder descending
                            select new ServicePackInfo
                            {
                                PackageId = p.PackageId,
                                PackageCode = p.PackageCode,
                                PackageName = p.PackageName,
                                CategoryId = p.CategoryId,
                                TypeId = p.TypeId,
                                AvailableQuantity = p.AvailableQuantity,
                                Capacity = p.Capacity,
                                DurationId = p.DurationId,
                                DiscountId = p.DiscountId,
                                TaxRateId = p.TaxRateId,
                                PackageFee = p.PackageFee,
                                TotalFee = p.TotalFee,
                                Weight = p.Weight,
                                CurrencyCode = p.CurrencyCode,
                                FileId = p.FileId,
                                Description = p.Description,
                                Specification = p.Specification,
                                Rating = p.Rating,
                                TotalViews = p.TotalViews,
                                ListOrder = p.ListOrder,
                                Status = p.Status,
                                CreatedDate = p.CreatedDate,
                                LastModifiedDate = p.LastModifiedDate,
                                Category = category,
                                Type = type,
                                Duration = duration,
                                Discount = discount,
                                Tax = tax
                            };

            return queryable.AsEnumerable();
        }
        public IEnumerable<ServicePackInfo> GetDiscountedServicePacks(int? categoryId, ServicePackStatus? status, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var query = from p in DataContext.Get<ServicePack>()
                        join c in DataContext.Get<ServiceCategory>() on p.CategoryId equals c.CategoryId into pcLst
                        from category in pcLst.DefaultIfEmpty()
                        join t in DataContext.Get<ServicePackType>() on p.TypeId equals t.TypeId into ptLst
                        from type in ptLst.DefaultIfEmpty()
                        join d in DataContext.Get<ServicePackDuration>() on p.DurationId equals d.DurationId into pdJoin
                        from duration in pdJoin.DefaultIfEmpty()
                        join tr in DataContext.Get<ServiceTaxRate>() on p.TaxRateId equals tr.TaxRateId into ptrLst
                        from tax in ptrLst.DefaultIfEmpty()
                        join d in DataContext.Get<ServiceDiscount>() on p.DiscountId equals d.DiscountId into pdLst
                        from discount in pdLst.DefaultIfEmpty()
                        where (p.DiscountId != null && p.DiscountId > 1) && (status == null || p.Status == status)
                        orderby p.ListOrder descending
                        select new ServicePackInfo
                        {
                            PackageId = p.PackageId,
                            PackageCode = p.PackageCode,
                            PackageName = p.PackageName,
                            CategoryId = p.CategoryId,
                            TypeId = p.TypeId,
                            AvailableQuantity = p.AvailableQuantity,
                            Capacity = p.Capacity,
                            DurationId = p.DurationId,
                            DiscountId = p.DiscountId,
                            TaxRateId = p.TaxRateId,
                            PackageFee = p.PackageFee,
                            TotalFee = p.TotalFee,
                            Weight = p.Weight,
                            CurrencyCode = p.CurrencyCode,
                            FileId = p.FileId,
                            Description = p.Description,
                            Specification = p.Specification,
                            Rating = p.Rating,
                            TotalViews = p.TotalViews,
                            ListOrder = p.ListOrder,
                            Status = p.Status,
                            CreatedDate = p.CreatedDate,
                            LastModifiedDate = p.LastModifiedDate,
                            Category = category,
                            Type = type,
                            Duration = duration,
                            Discount = discount,
                            Tax = tax,
                        };

            if (categoryId != null && categoryId > 0)
            {
                query = query.Where(x => x.CategoryId == categoryId);
            }
            return query.AsEnumerable().WithRecordCount(ref recordCount).WithSortingAndPaging(orderBy, page, pageSize);
        }

        public ServicePackInfo GetDetail(int servicePackId)
        {
            return (from p in DataContext.Get<ServicePack>()
                    join c in DataContext.Get<ServiceCategory>() on p.CategoryId equals c.CategoryId into pcJoin
                    from category in pcJoin.DefaultIfEmpty()
                    join d in DataContext.Get<ServicePackDuration>() on p.DurationId equals d.DurationId into pdJoin
                    from duration in pdJoin.DefaultIfEmpty()
                    join t in DataContext.Get<ServicePackType>() on p.TypeId equals t.TypeId into ptJoin
                    from type in ptJoin.DefaultIfEmpty()
                    join r in DataContext.Get<ServiceTaxRate>() on p.TaxRateId equals r.TaxRateId into prJoin
                    from tax in prJoin.DefaultIfEmpty()
                    join dis in DataContext.Get<ServiceDiscount>() on p.DiscountId equals dis.DiscountId into pdisJoin
                    from discount in pdisJoin.DefaultIfEmpty()
                    where p.PackageId == servicePackId
                    orderby p.ListOrder descending
                    select new ServicePackInfo
                    {
                        PackageId = p.PackageId,
                        PackageCode = p.PackageCode,
                        PackageName = p.PackageName,
                        CategoryId = p.CategoryId,
                        TypeId = p.TypeId,
                        AvailableQuantity = p.AvailableQuantity,
                        Capacity = p.Capacity,
                        DurationId = p.DurationId,
                        DiscountId = p.DiscountId,
                        TaxRateId = p.TaxRateId,
                        PackageFee = p.PackageFee,
                        TotalFee = p.TotalFee,
                        Weight = p.Weight,
                        CurrencyCode = p.CurrencyCode,
                        FileId = p.FileId,
                        Description = p.Description,
                        Specification = p.Specification,
                        Rating = p.Rating,
                        TotalViews = p.TotalViews,
                        ListOrder = p.ListOrder,
                        Status = p.Status,
                        CreatedDate = p.CreatedDate,
                        LastModifiedDate = p.LastModifiedDate,
                        Category = category,
                        Type = type,
                        Duration = duration,
                        Discount = discount,
                        Tax = tax,
                    }).FirstOrDefault();
        }
        public bool HasDataExisted(string servicePackName)
        {
            var query = DataContext.Get<ServicePack>().FirstOrDefault(c => c.PackageName.Equals(servicePackName, StringComparison.OrdinalIgnoreCase));
            return (query != null);
        }

        public SelectList PopulateServicePackStatus(bool? selectedValue = true, bool isShowSelectText = true)
        {
            List<SelectListItem> lst = new List<SelectListItem>
            {
                new SelectListItem {Text = LanguageResource.Active, Value = "True", Selected = (selectedValue!=null && selectedValue==true) },
                new SelectListItem {Text = LanguageResource.InActive, Value = "False", Selected = (selectedValue!=null && selectedValue==false) }
            };

            if (isShowSelectText)
            {
                lst.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.SelectStatus} ---", Value = "" });
            }
            return new SelectList(lst, "Value", "Text", selectedValue);
        }

        public SelectList PopulateServicePackSelectList(ServicePackStatus? status = null, int? selectedValue = null, bool? isShowSelectText = true)
        {
            var listItems = new List<SelectListItem>();
            var lst = (from p in DataContext.Get<ServicePack>()
                       join c in DataContext.Get<ServiceCategory>() on p.CategoryId equals c.CategoryId into pcLst
                       from category in pcLst.DefaultIfEmpty()
                       join t in DataContext.Get<ServicePackType>() on p.TypeId equals t.TypeId into ptLst
                       from type in ptLst.DefaultIfEmpty()
                       join d in DataContext.Get<ServicePackDuration>() on p.DurationId equals d.DurationId into pdJoin
                       from duration in pdJoin.DefaultIfEmpty()
                       join pd in DataContext.Get<ServicePeriod>() on p.DurationId equals pd.PeriodId into periodJoin
                       from period in periodJoin.DefaultIfEmpty()
                       join tr in DataContext.Get<ServiceTaxRate>() on p.TaxRateId equals tr.TaxRateId into ptrLst
                       from tax in ptrLst.DefaultIfEmpty()
                       join d in DataContext.Get<ServiceDiscount>() on p.DiscountId equals d.DiscountId into pdLst
                       from discount in pdLst.DefaultIfEmpty()
                       where (status == null || p.Status == status)
                       orderby p.ListOrder descending
                       select new ServicePackInfo
                       {
                           PackageId = p.PackageId,
                           PackageCode = p.PackageCode,
                           PackageName = p.PackageName,
                           CategoryId = p.CategoryId,
                           TypeId = p.TypeId,
                           AvailableQuantity = p.AvailableQuantity,
                           Capacity = p.Capacity,
                           DurationId = p.DurationId,
                           DiscountId = p.DiscountId,
                           TaxRateId = p.TaxRateId,
                           PackageFee = p.PackageFee,
                           TotalFee = p.TotalFee,
                           Weight = p.Weight,
                           CurrencyCode = p.CurrencyCode,
                           FileId = p.FileId,
                           Description = p.Description,
                           Specification = p.Specification,
                           Rating = p.Rating,
                           TotalViews = p.TotalViews,
                           ListOrder = p.ListOrder,
                           Status = p.Status,
                           CreatedDate = p.CreatedDate,
                           LastModifiedDate = p.LastModifiedDate,
                           Category = category,
                           Type = type,
                           Duration = duration,
                           Discount = discount,
                           Tax = tax,
                       }).ToList();

            if (lst.Any())
            {
                listItems = lst.Select(p => new SelectListItem
                {
                    Text = (p.Duration != null) ? $"{p.PackageName} ({p.Duration.DurationName}, {p.TotalFee} {p.CurrencyCode})" : $"{p.PackageName} ({p.TotalFee} {p.CurrencyCode})",
                    Value = p.PackageId.ToString(),
                    Selected = (selectedValue != null && p.PackageId == selectedValue)
                }).ToList();
                if (isShowSelectText != null && isShowSelectText == true)
                    listItems.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.Select} ---", Value = "" });
            }
            else
            {
                listItems.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "-1" });
            }
            return new SelectList(listItems, "Value", "Text", selectedValue);
        }

        public SelectList PopulateServicePackSelectListNotCode(ServicePackStatus? status = null, int? selectedValue = null, bool? isShowSelectText = true)
        {
            var listItems = new List<SelectListItem>();
            var lst = (from c in DataContext.Get<ServicePack>()
                       where (status == null || c.Status == status) && c.DiscountId == null
                       orderby c.ListOrder descending
                       select c).ToList();

            if (lst.Any())
            {
                listItems = lst.Select(p => new SelectListItem { Text = p.PackageName, Value = p.PackageId.ToString(), Selected = (selectedValue != null && p.PackageId == selectedValue) }).ToList();
                if (isShowSelectText != null && isShowSelectText == true)
                    listItems.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.Select} ---", Value = "" });
            }
            else
            {
                listItems.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "-1" });
            }
            return new SelectList(listItems, "Value", "Text", selectedValue);
        }

        public SelectList PopulateServicePackSelectListByCode(string discountCode, ServicePackStatus? status = null, int? selectedValue = null, bool? isShowSelectText = true)
        {
            var listItems = new List<SelectListItem>();
            var lst = (from c in DataContext.Get<ServicePack>()
                       join d in DataContext.Get<ServiceDiscount>() on c.DiscountId equals d.DiscountId
                       where (status == null || c.Status == status) && d.DiscountCode == discountCode
                       orderby c.ListOrder descending
                       select c).ToList();

            if (lst.Any())
            {
                listItems = lst.Select(p => new SelectListItem { Text = p.PackageName, Value = p.PackageId.ToString(), Selected = (selectedValue != null && p.PackageId == selectedValue) }).ToList();
                if (isShowSelectText != null && isShowSelectText == true)
                    listItems.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.Select} ---", Value = "" });
            }
            else
            {
                listItems.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "-1" });
            }
            return new SelectList(listItems, "Value", "Text", selectedValue);
        }

        public SelectList PopulateServicePackSelectListByCateId(int categoryId, ServicePackStatus? status = null, int? selectedValue = null, bool? isShowSelectText = true)
        {
            var listItems = new List<SelectListItem>();
            var lst = (from p in DataContext.Get<ServicePack>()
                       join d in DataContext.Get<ServicePackDuration>() on p.DurationId equals d.DurationId into pdJoin
                       from duration in pdJoin.DefaultIfEmpty()
                       where p.CategoryId == categoryId && (status == null || p.Status == status)
                       orderby p.ListOrder descending
                       select new ServicePackInfo
                       {
                           PackageId = p.PackageId,
                           PackageCode = p.PackageCode,
                           PackageName = p.PackageName,
                           CategoryId = p.CategoryId,
                           TypeId = p.TypeId,
                           AvailableQuantity = p.AvailableQuantity,
                           Capacity = p.Capacity,
                           DurationId = p.DurationId,
                           DiscountId = p.DiscountId,
                           TaxRateId = p.TaxRateId,
                           PackageFee = p.PackageFee,
                           TotalFee = p.TotalFee,
                           Weight = p.Weight,
                           CurrencyCode = p.CurrencyCode,
                           FileId = p.FileId,
                           Description = p.Description,
                           Specification = p.Specification,
                           Rating = p.Rating,
                           TotalViews = p.TotalViews,
                           ListOrder = p.ListOrder,
                           Status = p.Status,
                           CreatedDate = p.CreatedDate,
                           LastModifiedDate = p.LastModifiedDate,
                           Duration = duration
                       }).ToList();

            if (lst.Any())
            {
                listItems = lst.Select(p => new SelectListItem
                {
                    Text = (p.Duration != null) ? $"{p.PackageName} ({p.Duration.DurationName}, {p.TotalFee} {p.CurrencyCode})" : $"{p.PackageName} ({p.TotalFee} {p.CurrencyCode})",
                    Value = p.PackageId.ToString(),
                    Selected = (selectedValue != null && p.PackageId == selectedValue)
                }).ToList();

                if (isShowSelectText != null && isShowSelectText == true)
                    listItems.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.Select} ---", Value = "" });
            }
            else
            {
                listItems.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "-1" });
            }
            return new SelectList(listItems, "Value", "Text", selectedValue);
        }

        public int GetNewListOrder()
        {
            int listOrder = 1;
            var query = from u in DataContext.Get<ServicePack>() select (int)u.ListOrder;
            if (query.Any())
            {
                listOrder = query.Max() + 1;
            }
            return listOrder;
        }
    }
}
