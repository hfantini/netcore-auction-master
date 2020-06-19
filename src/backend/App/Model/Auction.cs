using System;
using System.Collections.Generic;

namespace AuctionMaster.App.Model
{
    public partial class Auction
    {
        public int Id { get; set; }
        public int TaskAuctionScan { get; set; }
        public int Item { get; set; }
        public int Buyout { get; set; }
        public int Bid { get; set; }
        public int Quantity { get; set; }

        public virtual Item ItemNavigation { get; set; }
        public virtual TaskAuctionScan TaskAuctionScanNavigation { get; set; }
    }
}
