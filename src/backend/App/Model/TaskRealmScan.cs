using System;
using System.Collections.Generic;

namespace AuctionMaster.App.Model
{
    public partial class TaskRealmScan
    {
        public int TrsId { get; set; }
        public int ScheduledTask { get; set; }
        public DateTime TrsStarttime { get; set; }
        public DateTime TrsEndtime { get; set; }
        public int? TrsCreCount { get; set; }
        public int? TrsReaCount { get; set; }

        public virtual ScheduledTask ScheduledTaskNavigation { get; set; }
    }
}
