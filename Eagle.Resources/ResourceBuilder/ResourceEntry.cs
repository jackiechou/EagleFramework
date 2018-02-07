namespace Eagle.Resources.ResourceBuilder
{
    public class ResourceEntry
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Culture { get; set; }
        public string Type { get; set; }

        public ResourceEntry()
        {
            Type = "string";
        }
    }
}
