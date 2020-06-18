using System;
using System.Collections.Generic;

namespace AuctionMaster.App.Model
{
    public partial class ScheduledTaskInterval
    {
        public int StiId { get; set; }
        public int ScheduledTask { get; set; }
        public DateTime? StiLastexecution { get; set; }
        public int StiInterval { get; set; }

        public virtual ScheduledTask ScheduledTaskNavigation { get; set; }
    }
}
