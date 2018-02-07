using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.SystemManagement
{
    [NotMapped]
    public class MenuPage : Menu
    {
        public bool? IsExtenalLink { get; set; }
        public string PagePath { get; set; }
        public string PageUrl { get; set; }
        public int? TemplateId { get; set; }
        public virtual ICollection<MenuPage> Parents { get; set; }
        public virtual PageInfo Page { get; set; }
    }
}
