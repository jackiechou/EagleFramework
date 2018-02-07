using System;
using Eagle.Services.Dtos.Business;
using Eagle.Services.Dtos.Business.Personnel;

namespace Eagle.Services.Dtos.Common
{
    public class CalendarEventItem
    {
        public CalendarEventItem()
        {
            AllDay = false;
        }
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string SomeKey { get; set; }
        public string Url { get; set; }
        public string Color { get; set; }
        public bool AllDay { get; set; }

        public CustomerDetail Customer { get; set; }
        public EmployeeInfoDetail Employee { get; set; }
    }
}
