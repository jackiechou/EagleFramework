using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Eagle.Resources;

namespace Eagle.Entities.SystemManagement
{
    [Table("PageGroup")]
    public class PageGroup : EntityBase, IEntity<int>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(ResourceType = typeof(LanguageResource), Name = "PageGroupId")]
        public int Id { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PageGroupCode")]
        public string PageGroupCode { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "PageGroupName")]
        public string PageGroupName { get; set; }

       

        [Display(ResourceType = typeof(LanguageResource), Name = "Description")]
        public string Description { get; set; }

        [Display(ResourceType = typeof(LanguageResource), Name = "IsActive")]
        public bool? IsActive { get; set; }
    }
}