using System.Collections.Generic;
using Eagle.Entities.Common;
using Eagle.Entities.Contents;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Contents
{
   public interface IBloodTypeRepository : IRepositoryBase<BloodType>
   {
       IEnumerable<BloodType> GetList();
   }
}
