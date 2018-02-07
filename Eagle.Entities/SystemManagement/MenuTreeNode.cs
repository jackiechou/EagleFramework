using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.SystemManagement
{
    [NotMapped]
    public class MenuTreeNode : EntityBase
    {
        public int id { get; set; }
        public int key { get; set; }
        public int? parentId { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public string text { get; set; }
        public string tooltip { get; set; }
        public string url { get; set; }
        public bool? isParent { get; set; }
        public bool? open { get; set; }
        public List<MenuTreeNode> children { get; set; } = new List<MenuTreeNode>();
    }
}
