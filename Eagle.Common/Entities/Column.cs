using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eagle.Common.Entities
{
    public class Column
    {
        public int id { get; set; }
        public string name { get; set; }
        public string display_name { get; set; }
        public string type { get; set; }
        public string default_value { get; set; }
        public bool allow_nulls { get; set; }
        public string relation { get; set; }
        public bool display_column { get; set; }
        public string input_restrictions { get; set; }
        public string sample_value { get; set; }
        public int? max_length { get; set; }
        public string required_by_software_version { get; set; }
        public bool viewable_by_user { get; set; }
        public bool editable_by_user { get; set; }
        public bool viewable_by_administrator { get; set; }
        public bool editable_by_administrator { get; set; }
        public int editing_ui_element { get; set; }
        public string description { get; set; }
    }
}
