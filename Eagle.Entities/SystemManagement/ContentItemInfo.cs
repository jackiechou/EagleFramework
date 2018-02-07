using System;

namespace Eagle.Entities.SystemManagement
{
    public class ContentItemInfo : EntityBase
    {
        public int ContentItemId { get; set; }
        public string ContentItemName { get; set; }
        public string ContentItemTitle { get; set; }
        public string Content { get; set; }
        public string ContentKey { get; set; }
      
        public bool? IsActive { get; set; }

        public string PageName { get; set; }
        public string ModuleName { get; set; }
        public Guid ApplicationId { get; set; }
        public int ContentTypeId { get; set; }
        public int? PageId { get; set; }
        public int? ModuleId { get; set; }
        public string LanguageCode { get; set; }
    }
}
