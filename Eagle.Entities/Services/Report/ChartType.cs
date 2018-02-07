using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Entities.Services.Report
{
    public enum ChartType
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "PieChart")]
        PieChart = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "ColumnChart")]
        ColumnChart = 2,
        [Display(ResourceType = typeof(LanguageResource), Name = "LineChart")]
        LineChart = 3
    }
}
