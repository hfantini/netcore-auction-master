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

        public int Id { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }

        public virtual ICollection<ScheduledTask> ScheduledTask { get; set; }
    }
}
