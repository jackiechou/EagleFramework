namespace Eagle.Services.Dtos.Common
{
    public class BasicAction : EntityAction
    {
        private BasicAction() { }

        public static EntityAction EditAction(EntityActionType code = EntityActionType.Edit, string id = null, string caption = null, string description = null, string url = null)
        {
            const string defaultValue = "Edit";

            var result = new BasicAction
            {
                Id = id,
                Code = code,
                Caption = caption ?? id ?? defaultValue,
                Description = description ?? caption ?? id ?? defaultValue,
                ActionUrl = url
            };

            return result;
        }

        public static EntityAction DeleteAction(EntityActionType code = EntityActionType.Delete, string id = null, string caption = null, string description = null, string url = null)
        {
            const string defaultValue = "Delete";

            var result = new BasicAction
            {
                Id = id,
                Code = code,
                Caption = caption ?? id ?? defaultValue,
                Description = description ?? caption ?? id ?? defaultValue,
                ActionUrl = url
            };

            return result;
        }

        public static EntityAction AddAction(EntityActionType code = EntityActionType.Add, string id = null, string caption = null, string description = null, string url = null)
        {
            const string defaultValue = "Add";

            var result = new BasicAction
            {
                Id = id,
                Code = code,
                Caption = caption ?? id ?? defaultValue,
                Description = description ?? caption ?? id ?? defaultValue,
                ActionUrl = url
            };

            return result;
        }
    }
}
