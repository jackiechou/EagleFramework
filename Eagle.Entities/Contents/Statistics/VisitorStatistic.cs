using System;

namespace Eagle.Entities.Contents.Statistics
{
    public class VisitorStatistic
    {
        public Guid? ApplicationId { get; set; }
        public int VisitorStatisticId { get; set; }
        public int? TotalVisitorsInDay { get; set; }
        public DateTime? CreatedOnDate { get; set; }
    }
}
