using System.Collections.Generic;
using System.Text;

namespace Eagle.Core.Extension
{
    public class TreeNode
    {
        public TreeNode()
        {
            children = new List<TreeNode>();
        }

        public string id { get; set; }
        public string key { get; set; }
        public string parentid { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public string text { get; set; }
        public string tooltip { get; set; }
        public string icon { get; set; }
        public string url { get; set; }
        public int? level { get; set; }
        public bool? selected { get; set; }
        public bool? haschild { get; set; }
        public bool? opened { get; set; }
        public object attributes { get; set; }
        public string state { get; set; }
        public List<TreeNode> children { get; set; }
    }
}
