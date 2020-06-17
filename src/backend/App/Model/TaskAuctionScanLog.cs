using System;
using System.Collections.Generic;

namespace AuctionMaster.App.Model
{
    public partial class TaskAuctionScanLog
    {
        public int TaslId { get; set; }
        public int TaskAuctionScan { get; set; }
        public int TaslCode { get; set; }
        public string TaslMessage { get; set; }

        public virtual TaskAuctionScan TaskAuctionScanNavigation { get; set; }
    }
}
