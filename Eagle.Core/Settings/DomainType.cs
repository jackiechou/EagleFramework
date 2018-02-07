using System.ComponentModel;

namespace Eagle.Core.Settings
{
    public enum DomainType
    {
        [Description("Unknown")]
        Unknown = 0,
        [Description("News")]
        News = 1,
        [Description("Event")]
        Event = 2,
        [Description("Document")]
        Document = 3,
        [Description("Media")]
        Media = 4
    }
}
