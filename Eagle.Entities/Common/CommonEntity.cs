using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Eagle.Common.Utilities;

namespace Eagle.Entities.Common
{
    public class CommonEntity : EntityBase
    {
        public int Id { get; set; }
        public bool IsNew()
        {
            return Id == 0;
        }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        private DateTime? _createdDate = DateTime.MinValue.ToUniversalTime();

        [DataType(DataType.DateTime)]
        public DateTime CreatedDate
        {
            get { return _createdDate ?? DateTime.UtcNow; }
            set { _createdDate = value; }
        }
        public DateTime ModifiedDate { get; set; }
        public int SortOrder { get; set; }
        public int Priority { get; set; } = 1;

        public bool ShowTitle { get; set; } = true;

        [Browsable(false), XmlIgnore]
        public Guid CreatedByUserId { get; } = Null.NullGuid;

        [Browsable(false), XmlIgnore]
        public DateTime CreatedOnDate { get; } = DateTime.UtcNow;

        [Browsable(false), XmlIgnore]
        public Guid LastModifiedByUserId { get; } = Null.NullGuid;

        [Browsable(false), XmlIgnore]
        public DateTime LastModifiedOnDate { get; } = DateTime.UtcNow;
    }
}
