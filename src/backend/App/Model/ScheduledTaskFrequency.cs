using System;
using System.Collections.Generic;

namespace AuctionMaster.App.Model
{
    public partial class ScheduledTaskFrequency
    {
        public ScheduledTaskFrequency()
        {
            ScheduledTask = new HashSet<ScheduledTask>();
        }

        public int StfId { get; set; }
        public string StfNome { get; set; }

        public virtual ICollection<ScheduledTask> ScheduledTask { get; set; }
    }
}
