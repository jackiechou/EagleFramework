namespace Eagle.Services.Dtos.Business.Report
{
    public class Chart
    {
        public string label { get; set; }
        public string data { get; set; }
        public int? total { get; set; }
        public string percent => data + "%";
    }
}
