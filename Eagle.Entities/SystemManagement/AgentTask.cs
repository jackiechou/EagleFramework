using System;

namespace Eagle.Entities.SystemManagement
{
    public class AgentTask : EntityBase
    {
        public int AgentTaskId { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public DateTime StartDate { get; set; }
        public int FrequencyInSeconds { get; set; }
        public bool Enabled { get; set; }
        public bool Active { get; set; }
        public DateTime LastRun { get; set; }
        public string ServiceId { get; set; }
        public string Url { get; set; }
    }
}
