using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Common
{
    [NotMapped]
    public class TreeEntity : EntityBase
    {
        public int id { get; set; }
        public int key { get; set; }
        public int? parentid { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public string text { get; set; }
        public string tooltip { get; set; }
        public int? depth { get; set; }
        public bool? hasChild { get; set; }
        public bool? selected { get; set; } = false;
        public bool state { get; set; } = false;

        public bool folder { get; set; }
        public bool lazy { get; set; }
        public bool expanded { get; set; }

        public List<TreeEntity> children { get; set; }
    }
}
