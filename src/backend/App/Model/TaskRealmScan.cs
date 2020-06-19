using System;
using System.Collections.Generic;

namespace AuctionMaster.App.Model
{
    public partial class TaskRealmScan
    {
        public int Id { get; set; }
        public int ScheduledTask { get; set; }
        public DateTime Starttime { get; set; }
        public DateTime Endtime { get; set; }
        public int? ConnectRealmCount { get; set; }
        public int? RealmCount { get; set; }

        public virtual ScheduledTask ScheduledTaskNavigation { get; set; }
    }
}
