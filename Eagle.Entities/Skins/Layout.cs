namespace Eagle.Entities.Skins
{
    public class ThemeLayout : EntityBase
    {
        public long Id { get; set; }
        public string PivotName { get; set; }
        public string UserName { get; set; }
        public string Url { get; set; }
        public string LayOutString { get; set; }

        public string LayoutReport { get; set; }
        public int? Tab { get; set; }
    }
}
