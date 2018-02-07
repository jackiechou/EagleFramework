namespace Eagle.Services.Dtos.Common
{ 
    public class Select2Result
    {
        public string id { get; set; }
        public string name { get; set; }
        public string text { get; set; }
        public int? level { get; set; }
        public bool selected { get; set; }
    }
}
