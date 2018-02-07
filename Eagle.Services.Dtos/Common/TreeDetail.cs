using System.Collections.Generic;

namespace Eagle.Services.Dtos.Common
{
    public class TreeDetail: DtoBase
    {
        public int id { get; set; }
        public int key { get; set; }
        public int? parentid { get; set; }
        public int? depth { get; set; }
        public bool? hasChild { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public string text { get; set; }
        public string tooltip { get; set; }
        public bool folder { get; set; }
        public bool lazy { get; set; }
        public bool expanded { get; set; }
        public bool? selected { get; set; } = false;
        public List<TreeDetail> children { get; set; }
    }
}
