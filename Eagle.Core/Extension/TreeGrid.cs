using System.Collections.Generic;
using Eagle.Resources;

namespace Eagle.Core.Extension
{
    public class TreeGrid 
    {
        public TreeGrid()
        {
            if (hasChild != null && hasChild == true)
            {
                type = "glyphicon glyphicon-folder-open";
                icon = "glyphicon glyphicon-folder-open";
            }
            else
            {
                type = "glyphicon glyphicon-folder-close";
                icon = "glyphicon glyphicon-file";
            }
           
        }
        public string id { get; set; }
        public string parentId { get; set; }
        public int? level { get; set; }
        public bool? hasChild { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public string text { get; set; }
        public string icon { get; set; }
        public TreeGridData data { get; set; }
        public TreeGridState state { get; set; }
        public List<TreeGrid> children { get; set; }
    }

    public class TreeGridData
    {
        public string status { get; set; }
        public string action { get; set; }
    }

    public class TreeGridState
    {
        public bool? opened { get; set; }
        public bool? disabled { get; set; }
        public bool? selected { get; set; }
    }
}
