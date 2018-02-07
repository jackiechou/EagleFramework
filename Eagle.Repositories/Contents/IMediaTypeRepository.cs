using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Entities.Contents.Media;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Contents
{
    public interface IMediaTypeRepository : IRepositoryBase<MediaType>
    {
        IEnumerable<MediaType> GetList(MediaTypeStatus? status);
        IEnumerable<MediaType> GetMediaTypes(MediaTypeStatus? status, ref int? recordCount, int? page = null, int? pageSize = null);
        int GetNewListOrder();
        bool HasDataExisted(string typeName);
        SelectList PopulateMediaTypeSelectList(int? selectedValue = null, bool? isShowSelectText = null, MediaTypeStatus? status = null);
    }
}
