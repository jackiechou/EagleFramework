using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eagle.Entities.Services.Messaging
{
    [NotMapped]
    public class NotificationTypeTreeNode
    {
        public NotificationTypeTreeNode()
        {
            Children = new List<NotificationTypeTreeNode>();
        }
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public string Text { get; set; }
        public int Depth { get; set; }
        public string State { get; set; }
        public bool? IsChecked { get; set; }
        public string Icon { get; set; }
        public object Attributes { get; set; }

        public IEnumerable<NotificationTypeTreeNode> Children { get; set; }

    }
}
