using System.Collections.Generic;

namespace Eagle.Entities.SystemManagement
{
    public class ContentTreeModel : EntityBase, IEntity<int>
    {
        public int Id { get; set; }
        public int Key { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Tooltip { get; set; }
        public bool? IsParent { get; set; }
        public bool? Open { get; set; }
        public List<ContentTreeModel> Children { get; set; }

        public ContentTreeModel()
        {
            Children = new List<ContentTreeModel>();
        }
    }
}
