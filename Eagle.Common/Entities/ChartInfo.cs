using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Common.Entities
{
    public class ChartInfo
    {
        public string label { get; set; }
        public string data { get; set; }
        public string color { get; set; }
    }

    public class LineChartInfo
    {
        public string label { get; set; }
        public string data { get; set; }
        public ChartLine lines { get; set; }
        public ChartPoint points { get; set; }
        public string color { get; set; }
    }

    public class ChartLine
    {
        public bool show { get; set; }
    }
  
    public class ChartPoint
    {
        public bool show { get; set; }
        public string symbol { get; set; }
        public string fillColor { get; set; }
    }
}
