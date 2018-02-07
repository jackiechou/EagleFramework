using System.Collections.Generic;

namespace Eagle.Services.Dtos.Business.Report
{
    public class LineChart
    {
        public string label { get; set; }
        public List<double[]> data { get; set; }
    }
}
