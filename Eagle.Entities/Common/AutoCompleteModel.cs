namespace Eagle.Entities.Common
{
    public class AutoComplete
    {
        public AutoComplete()
        {
            Level = 1;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public int? Level { get; set; }
    }
}
