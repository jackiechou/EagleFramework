using System.Collections.Generic;
using Eagle.Entities.Common;
using Eagle.Entities.Contents;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework;
using Eagle.Resources;

namespace Eagle.Repositories.Contents
{
    public class BloodTypeRepository: RepositoryBase<BloodType>, IBloodTypeRepository
    {
        public BloodTypeRepository(IDataContext dataContext) : base(dataContext) { }
        
        public IEnumerable<BloodType> GetList()
        {
            return new List<BloodType>
            {
                    new BloodType { BloodTypeId = "A", BloodTypeName = LanguageResource.GroupA },
                    new BloodType { BloodTypeId = "B", BloodTypeName = LanguageResource.GroupB },
                    new BloodType { BloodTypeId = "AB", BloodTypeName = LanguageResource.GroupAB },
                    new BloodType { BloodTypeId = "O", BloodTypeName = LanguageResource.GroupO }
                };
        }
    }
}
