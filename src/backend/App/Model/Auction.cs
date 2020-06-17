using System;
using System.Collections.Generic;

namespace AuctionMaster.App.Model
{
    public partial class Auction
    {
        public int AucId { get; set; }
        public int TaskAuctionScan { get; set; }
        public int Item { get; set; }
        public int AucBuyout { get; set; }
        public int AucBid { get; set; }
        public int AucQuantity { get; set; }

        public virtual Item ItemNavigation { get; set; }
        public virtual TaskAuctionScan TaskAuctionScanNavigation { get; set; }
    }
}
