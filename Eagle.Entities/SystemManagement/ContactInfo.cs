using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Entities.SystemManagement.FileStorage;

namespace Eagle.Entities.SystemManagement
{
    [NotMapped]
    public class ContactInfo : Contact
    {
        public string FullName { get; set; }
        public DocumentInfo DocumentInfo { get; set; }
    }
}
