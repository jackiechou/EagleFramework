using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Entities.Business.Orders
{
    public enum MarkAsRead
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "UnRead")]
        UnRead = 0,
        [Display(ResourceType = typeof(LanguageResource), Name = "Read")]
        Read = 1
    }
}
