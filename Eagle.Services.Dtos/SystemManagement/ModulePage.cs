using System.Web.Mvc;

namespace Eagle.Services.Dtos.SystemManagement
{
    public class ModuleOnPageEntry : DtoBase
    {
        public MultiSelectList AvailableModules { get; set; }
        public MultiSelectList SelectedModulesOnPage { get; set; }
    }
}
