using System.Collections.Generic;

namespace Eagle.Entities.SystemManagement
{
    public class MenuTree: EntityBase
    {
        public int Id { get; set; }
        public int Key { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Tooltip { get; set; }
        public string Url { get; set; }
        public bool? IsParent { get; set; }
        public bool? Open { get; set; }
        public List<MenuTree> Children { get; set; }

        public MenuTree()
        {
            Children = new List<MenuTree>();
        }
    }
}
