using System;
using System.Collections.Generic;

namespace AuctionMaster.App.Model
{
    public partial class ScheduledTaskType
    {
        public ScheduledTaskType()
        {
            ScheduledTask = new HashSet<ScheduledTask>();
        }

        public int SttId { get; set; }
        public string SttName { get; set; }
        public string SttClass { get; set; }

        public virtual ICollection<ScheduledTask> ScheduledTask { get; set; }
    }
}
