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

        public int Id { get; set; }
        public string Nome { get; set; }

        public virtual ICollection<ScheduledTask> ScheduledTask { get; set; }
    }
}
