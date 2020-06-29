using System;
using System.Collections.Generic;

namespace AuctionMaster.App.Model
{
    public partial class TaskAuctionScan
    {
        public TaskAuctionScan()
        {
            Auction = new HashSet<Auction>();
            TaskAuctionScanLog = new HashSet<TaskAuctionScanLog>();
        }

        public int Id { get; set; }
        public int ScheduledTask { get; set; }
        public int ConnectedRealm { get; set; }
        public DateTime Starttime { get; set; }
        public DateTime? Endtime { get; set; }
        public sbyte Status { get; set; }
        public string Param { get; set; }

        public virtual ConnectedRealm ConnectedRealmNavigation { get; set; }
        public virtual ScheduledTask ScheduledTaskNavigation { get; set; }
        public virtual ICollection<Auction> Auction { get; set; }
        public virtual ICollection<TaskAuctionScanLog> TaskAuctionScanLog { get; set; }
    }
}
