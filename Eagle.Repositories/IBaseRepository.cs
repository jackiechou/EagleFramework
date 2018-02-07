using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Entities.Common;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories
{
    public interface IBaseRepository : IRepository
    {
        MessageBoxModel ShowMessageBox(string messageType, string message);
        SelectList GetGenders(string selectedValue);
        SelectList GetGenderList(string selectedValue);
        SelectList GetWorkTypes(string selectedValue);
        SelectList GetMonths(string selectedValue, int languageId);
        SelectList GetYears(int numberOfYears, string selectedValue, bool isShowSelectText = false);
        SelectList PopulateArticleImageSizes(string selectedValue, bool isShowSelectText = false);
        SelectList PopulateSummaryImageSizes(string selectedValue, bool isShowSelectText = false);
        SelectList PopulatePermisionGroups(string selectedValue, bool isShowSelectText = false);
        SelectList PopulateScopeTypeList(string selectedValue, bool isShowSelectText = false);
        SelectList PopulateActionList(string selectedValue);
        SelectList GetAlignmentList(string selectedValue, bool isShowSelectText = false);
        SelectList GetEventStatus(string selectedValue);
        SelectList GetObjectStatus(string selectedValue);
        SelectList GenerateThreeStatusModeList(string selectedValue, bool? isShowSelectText);
        SelectList GenerateThreeStatusModeListWithOptionText(string optionText = null, bool? isShowSelectText = true);
        SelectList GenerateThreeStatusModeListWithOptionText(string selectedValue, string optionText,
            bool? isShowSelectText = true);

        SelectList GenerateTwoStatusModeList(string selectedValue, bool? isShowSelectText);
        SelectList GenerateTwoStatusModeListWithOptionText(string optionText = null, bool? isShowSelectText = true);

        SelectList GenerateTwoStatusModeListWithOptionText(string selectedValue = null, string optionText = null,
            bool? isShowSelectText = true);

        SelectList PopulateLinkTargets(string selectedValue, bool isShowSelectText = false);
        Dictionary<string, string> GetMonths();
        List<DropdownListItem> GetPaymentTypes();
        List<DropdownListItem> GetMoneyTypes();
        List<DropdownListItem> GeContractTypes();
        List<DropdownListItem> GetTaxes();
        List<DropdownListItem> GetFileTypes();
    }
}
