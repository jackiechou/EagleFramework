using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Entities.Common;
using Eagle.Services.Dtos.Common;

namespace Eagle.Services
{
    public interface ICommonService : IBaseService
    {
        SelectList GetMonths(Guid applicationId, string selectedValue, bool? isShowSelectText = false);
        SelectList GetYears(int numberOfYears, string selectedValue, bool? isShowSelectText = false);
        SelectList GenerateThreeStatusModeList(string selectedValue, bool? isShowSelectText);
        IEnumerable<TreeNodeDetail> RecursiveFillTreeNodes(IEnumerable<TreeNodeDetail> elements, int? parentid=0);
        List<TreeDetail> RecursiveFillTree(IEnumerable<TreeDetail> elements, int? parentid);
    }
}
