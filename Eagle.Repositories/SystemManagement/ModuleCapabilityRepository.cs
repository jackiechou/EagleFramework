using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Common;
using Eagle.Core.Settings;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework;

namespace Eagle.Repositories.SystemManagement
{
    public class ModuleCapabilityRepository : RepositoryBase<ModuleCapability>, IModuleCapabilityRepository
    {
        public ModuleCapabilityRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<ModuleCapability> GetList(ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            return (DataContext.Get<ModuleCapability>().WithRecordCount(ref recordCount)
                            .WithSortingAndPaging(orderBy, page, pageSize)).AsEnumerable();
        }

        public IEnumerable<ModuleCapability> GetModuleCapabilitiesByModuleId(int moduleId, int? isActive)
        {
            var query = DataContext.Get<ModuleCapability>().Where(x => x.ModuleId == moduleId);
            if (isActive != null)
                query = query.Where(x => x.IsActive == (ModuleCapabilityStatus)isActive);
            return query.AsEnumerable();
        }

        public IEnumerable<ModuleCapabilityInfo> GetModuleCapabilities(int moduleId, ModuleCapabilityStatus? status=null)
        {
            var query = from c in DataContext.Get<ModuleCapability>()
                join m in DataContext.Get<Module>() on c.ModuleId equals m.ModuleId into mcJoin
                from module in mcJoin.DefaultIfEmpty()
                where c.ModuleId == moduleId && (status == null || c.IsActive == status)
                select new ModuleCapabilityInfo
                {
                    CapabilityId = c.CapabilityId,
                    CapabilityName = c.CapabilityName,
                    CapabilityCode = c.CapabilityCode,
                    Description = c.Description,
                    DisplayOrder = c.DisplayOrder,
                    IsActive = c.IsActive,
                    ModuleId = c.ModuleId,
                    Module = module
                };

            return query.AsEnumerable();
        }

        public ModuleCapability GetDetails(int moduleId, string capabilityName)
        {
            return DataContext.Get<ModuleCapability>().FirstOrDefault(p => p.ModuleId == moduleId && p.CapabilityName.ToLower() == capabilityName.ToLower());
        }
        public MultiSelectList PopulateModuleCapabilityMultiSelectList(int moduleId, ModuleCapabilityStatus? isActive, string[] selectedValues)
        {
            var list = (from m in DataContext.Get<ModuleCapability>()
                        where m.ModuleId == moduleId && (isActive == null || m.IsActive == isActive)
                        select new SelectListItem
                        {
                            Text = m.CapabilityName,
                            Value = m.CapabilityId.ToString()
                        }).OrderByDescending(m => m.Text).ToList();
            return new MultiSelectList(list, "Value", "Text", selectedValues);
        }
        public int GetListOrder()
        {
            return DataContext.Get<ModuleCapability>().DefaultIfEmpty().Max(x => x == null ? 1 : x.DisplayOrder + 1);
        }
        public bool HasModuleCapabilityNameExisted(string capabilityName, int moduleId)
        { 
            var result = DataContext.Get<ModuleCapability>().FirstOrDefault(p => p.ModuleId == moduleId && p.CapabilityName.ToLower() == capabilityName.ToLower());
            return (result != null);
        }
        public bool HasModuleCapabilityCodeExisted(string capabilityCode, int moduleId)
        {
            var result = DataContext.Get<ModuleCapability>().FirstOrDefault(p => p.ModuleId == moduleId && p.CapabilityCode.ToLower() == capabilityCode.ToLower());
            return (result != null);
        }
    }
}
