using System;
using System.Collections.Generic;

namespace AuctionMaster.App.Model
{
    public partial class ScheduledTask
    {
        public ScheduledTask()
        {
            ScheduledTaskInterval = new HashSet<ScheduledTaskInterval>();
            TaskAuctionScan = new HashSet<TaskAuctionScan>();
            TaskRealmScan = new HashSet<TaskRealmScan>();
        }

        public int StaId { get; set; }
        public string StaName { get; set; }
        public int ScheduledTaskType { get; set; }
        public int SheduledTaskFrequency { get; set; }

        public virtual ScheduledTaskType ScheduledTaskTypeNavigation { get; set; }
        public virtual ScheduledTaskFrequency SheduledTaskFrequencyNavigation { get; set; }
        public virtual ICollection<ScheduledTaskInterval> ScheduledTaskInterval { get; set; }
        public virtual ICollection<TaskAuctionScan> TaskAuctionScan { get; set; }
        public virtual ICollection<TaskRealmScan> TaskRealmScan { get; set; }
    }
}
