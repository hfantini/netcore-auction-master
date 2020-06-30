using System;
using System.Collections.Generic;

namespace AuctionMaster.App.Model
{
    public partial class ScheduledTask
    {
        public ScheduledTask()
        {
            ScheduledTaskInterval = new HashSet<ScheduledTaskInterval>();
            ScheduledTaskLog = new HashSet<ScheduledTaskLog>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int ScheduledTaskType { get; set; }
        public int SheduledTaskFrequency { get; set; }
        public DateTime? LastExecution { get; set; }
        public sbyte Enabled { get; set; }
        public string Param { get; set; }

        public virtual ScheduledTaskType ScheduledTaskTypeNavigation { get; set; }
        public virtual ScheduledTaskFrequency SheduledTaskFrequencyNavigation { get; set; }
        public virtual ICollection<ScheduledTaskInterval> ScheduledTaskInterval { get; set; }
        public virtual ICollection<ScheduledTaskLog> ScheduledTaskLog { get; set; }
    }
}
