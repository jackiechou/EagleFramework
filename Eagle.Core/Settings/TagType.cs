using System.ComponentModel.DataAnnotations;
using Eagle.Resources;

namespace Eagle.Core.Settings
{
    public enum TagType
    {
        [Display(ResourceType = typeof(LanguageResource), Name = "Unknown", Description = "Unknown", Order = 0)]
        Unknown = 0,
        [Display(ResourceType = typeof(LanguageResource), Name = "News", Description = "News", Order = 1)]
        News = 1,
        [Display(ResourceType = typeof(LanguageResource), Name = "Event", Description = "Event", Order = 2)]
        Event = 2,
        [Display(ResourceType = typeof(LanguageResource), Name = "Document", Description = "Document", Order = 3)]
        Document = 3,
        [Display(ResourceType = typeof(LanguageResource), Name = "Media", Description = "Media", Order = 4)]
        Media = 4
    }
}
