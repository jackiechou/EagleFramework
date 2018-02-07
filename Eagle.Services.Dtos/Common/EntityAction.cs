using System.Collections.Generic;

namespace Eagle.Services.Dtos.Common
{
    public class EntityAction
    {
        protected EntityAction(){}

        public string Id { get; set; }
        public EntityActionType Code { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public string ActionUrl { get; set; }

        public IEnumerable<EntityAction> Children { get; set; }
    }
}
