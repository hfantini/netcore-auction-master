using System;
using System.Collections.Generic;

namespace AuctionMaster.App.Model
{
    public partial class ScheduledTaskLog
    {
        public ScheduledTaskLog()
        {
            Auction = new HashSet<Auction>();
        }

        public int Id { get; set; }
        public int ScheduledTask { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int Tentatives { get; set; }
        public sbyte Status { get; set; }
        public string Message { get; set; }

        public virtual ScheduledTask ScheduledTaskNavigation { get; set; }
        public virtual ICollection<Auction> Auction { get; set; }
    }
}
