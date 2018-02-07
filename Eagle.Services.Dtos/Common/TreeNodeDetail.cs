using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Services.Dtos.Common
{
    public class TreeNodeDetail : DtoBase
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public string Text { get; set; }
        public int Depth { get; set; }
        public string State { get; set; }
        public bool? Ischecked { get; set; }
        public string Icon { get; set; }
        public object Attributes { get; set; }

        public IEnumerable<TreeNodeDetail> Children { get; set; }
        public string TreeNodeJson()
        {
            return ConvertTreeNodeToJson(this);
        }
        public TreeNodeDetail()
        {
            Children = new List<TreeNodeDetail>();
        }
        private string ConvertTreeNodeToJson(TreeNodeDetail node)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");

            sb.AppendFormat("\"id\":{0},", node.Id);

            if (!string.IsNullOrWhiteSpace(node.State))
            {
                sb.AppendFormat("\"state\":\"{0}\",", node.State);
            }
            if (!string.IsNullOrWhiteSpace(node.Icon))
            {
                sb.AppendFormat("\"icon\":\"{0}\",", node.Icon);
            }
            if (node.Ischecked != null)
            {
                sb.AppendFormat("\"checked\":\"{0},\"", node.Ischecked);
            }

            // to append attributes...
            if (node.Attributes != null)
            {
                var attributesType = node.Attributes.GetType();
                foreach (var item in attributesType.GetProperties())
                {
                    var value = item.GetValue(node.Attributes, null);
                    if (value != null)
                    {
                        sb.AppendFormat("\"{0}\":\"{1}\",", item.Name, value);
                    }
                }
            }

            //recursive append children
            if (node.Children != null && node.Children.ToArray().Length > 0)
            {
                StringBuilder sbChildren = new StringBuilder();
                foreach (var item in node.Children)
                {
                    sbChildren.AppendFormat("{0},", ConvertTreeNodeToJson(item));
                }

                sb.AppendFormat("\"children\":[{0}],", sbChildren.ToString().TrimEnd(','));
            }


            sb.AppendFormat("\"text\":\"{0}\"", node.Text);

            sb.Append("}");

            return sb.ToString();
        }
    }
}
