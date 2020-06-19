using System;
using System.Collections.Generic;

namespace AuctionMaster.App.Model
{
    public partial class ScheduledTaskInterval
    {
        public int Id { get; set; }
        public int ScheduledTask { get; set; }
        public DateTime? Lastexecution { get; set; }
        public int Interval { get; set; }

        public virtual ScheduledTask ScheduledTaskNavigation { get; set; }
    }
}
