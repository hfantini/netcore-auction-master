using System;
using System.Collections.Generic;

namespace AuctionMaster.App.Model
{
    public partial class TaskAuctionScanLog
    {
        public int Id { get; set; }
        public int TaskAuctionScan { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }

        public virtual TaskAuctionScan TaskAuctionScanNavigation { get; set; }
    }
}
