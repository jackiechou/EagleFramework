using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.SystemManagement
{
    [Table("PageBanner")]
    public class PageBanner: EntityBase, IEntity<int>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? BannerId { get; set; }
        public int? PageId { get; set; }
    }
}
