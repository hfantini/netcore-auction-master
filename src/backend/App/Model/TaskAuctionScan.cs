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

        public int TasId { get; set; }
        public int ConnectedRealm { get; set; }
        public DateTime TasStarttime { get; set; }
        public DateTime TasEndtime { get; set; }
        public sbyte TasStatus { get; set; }

        public virtual ConnectedRealm ConnectedRealmNavigation { get; set; }
        public virtual ICollection<Auction> Auction { get; set; }
        public virtual ICollection<TaskAuctionScanLog> TaskAuctionScanLog { get; set; }
    }
}
